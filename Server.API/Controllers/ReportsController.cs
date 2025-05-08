using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Core.Services;
using Server.Data.Db;
using Server.Data.Entities;
using Server.Shared.DTOs;
using System.Text;

namespace Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PdfService _pdfService;

        public ReportsController(AppDbContext context, PdfService pdfService)
        {
            _context = context;
            _pdfService = pdfService;
        }

        [HttpPost("generate-acceptance-act")]
        public async Task<IActionResult> GenerateAcceptanceAct([FromBody] AcceptanceActRequestDto request)
        {
            var html = await GetHtml(request);

            var pdfBytes = _pdfService.GeneratePdfFromHtml(html);
            return File(pdfBytes, "application/pdf", "AcceptanceAct.pdf");
        }

        [HttpPost("generate-html-preview")]
        public async Task<IActionResult> GenerateHtmlPreview([FromBody] AcceptanceActRequestDto request)
        {
            var html = await GetHtml(request);
            return Ok(html);
        }

        private async Task<string> GetHtml(AcceptanceActRequestDto request)
        {
            var supplier = await _context.Suppliers
                .Include(s => s.Organization)
                .FirstOrDefaultAsync(s => s.Id == request.SupplierId);

            var movements = await _context.MaterialMovements
                .Include(m => m.MaterialItem)
                    .ThenInclude(i => i.MeasurementUnit)
            .Where(m =>
                    m.MaterialItem.SupplierId == request.SupplierId &&
                    m.MovementType == 0 &&
                    m.MovementDate >= request.DateFrom &&
                    m.MovementDate <= request.DateTo)
                .ToListAsync();


            var contractNumber = string.IsNullOrWhiteSpace(request.ContractNumber)
                ? "__________"
                : request.ContractNumber;

            return await GetHtmlFromTemplateAsync(contractNumber, supplier, movements);
        }

        private async Task<string> GetHtmlFromTemplateAsync(string contractNumber, Supplier supplier, List<MaterialMovement> movements)
        {
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "AcceptanceActTemplate.html");
            var template = await System.IO.File.ReadAllTextAsync(templatePath);

            var year = DateTime.Now.Year;
            var buyerName = supplier.Organization?.Name ?? "";
            var buyerCode = supplier.Organization?.EdrpouCode ?? "";
            var buyerAddress = supplier.Organization?.Address ?? "";

            var sbRows = new StringBuilder();
            int index = 1;
            decimal total = 0;
            const decimal VAT_RATE = 0.2m;

            foreach (var m in movements)
            {
                var priceWithVat = m.PricePerUnit * (1 + VAT_RATE);
                var sum = m.Quantity * m.PricePerUnit;
                var sumWithVat = m.Quantity * priceWithVat;
                total += sum;

                sbRows.AppendLine($"""
            <tr>
                <td>{index++}</td>
                <td>{m.MaterialItem.Name}</td>
                <td>{m.MaterialItem.MeasurementUnit.ShortName}</td>
                <td>{m.Quantity}</td>
                <td>{m.PricePerUnit:F2}</td>
                <td>{priceWithVat:F2}</td>
                <td>{sum:F2}</td>
                <td>{sumWithVat:F2}</td>
            </tr>
        """);
            }

            var totalVat = total * VAT_RATE;
            var totalWithVat = total + totalVat;

            var today = DateTime.Today;
            var city = supplier.Organization?.CityForDocs;
            var day = today.Day.ToString("D2");
            var ukCulture = new System.Globalization.CultureInfo("uk-UA");
            var month = ukCulture.DateTimeFormat.GetMonthName(today.Month); 
            var monthGenitive = ukCulture.DateTimeFormat.MonthGenitiveNames[today.Month - 1]; 



            string F15(string? val) => string.IsNullOrWhiteSpace(val) ? "_______________" : val;
            string F30(string? val) => string.IsNullOrWhiteSpace(val) ? "______________________________" : val;
            string F50(string? val) => string.IsNullOrWhiteSpace(val) ? "__________________________________________________" : val;

            return template
                .Replace("{Year}", year.ToString())
                .Replace("{ContractNumber}", F15(contractNumber))
                .Replace("{BuyerName}", F30(buyerName))
                .Replace("{BuyerEDRPOU}", F15(buyerCode))
                .Replace("{BuyerAddress}", F50(buyerAddress))
                .Replace("{SupplierName}", F30(supplier.Name))
                .Replace("{SupplierEDRPOU}", F15(supplier.EdrpouCode))
                .Replace("{SupplierAddress}", F50(supplier.Address))
                .Replace("{Items}", sbRows.ToString())
                .Replace("{TotalWithoutVat:F2}", total.ToString("F2"))
                .Replace("{TotalVat:F2}", totalVat.ToString("F2"))
                .Replace("{TotalWithVat:F2}", totalWithVat.ToString("F2"))
                .Replace("{City}", F15(city))
                .Replace("{SupplierContactPerson}", F15(supplier.ContactPerson))
                .Replace("{BuyerFio}", F15(supplier.Organization?.FioForDocs))
                .Replace("{Day}", day)
                .Replace("{Month}", monthGenitive);
        }

        [HttpPost("generate-transfer-act")]
        public async Task<IActionResult> GenerateTransferAct([FromBody] TransferActRequestDto request)
        {
            var html = await GetTransferHtml(request);
            var pdfBytes = _pdfService.GeneratePdfFromHtml(html);
            return File(pdfBytes, "application/pdf", "TransferAct.pdf");
        }

        [HttpPost("generate-transfer-html-preview")]
        public async Task<IActionResult> GenerateTransferHtmlPreview([FromBody] TransferActRequestDto request)
        {
            var html = await GetTransferHtml(request);
            return Ok(html);
        }

        private async Task<string> GetTransferHtml(TransferActRequestDto request)
        {
            var orgId = await _context.Recipients
                .Where(r => r.Id == request.RecipientId)
                .Select(r => r.OrganizationId)
                .FirstOrDefaultAsync();

            var organization = await _context.Organizations.FirstOrDefaultAsync(o => o.Id == orgId);
            var recipient = await _context.Recipients.FirstOrDefaultAsync(r => r.Id == request.RecipientId);

            var movements = await _context.MaterialMovements
                .Include(m => m.MaterialItem)
                    .ThenInclude(i => i.MeasurementUnit)
                .Where(m =>
                    m.RecipientId == request.RecipientId &&
                    m.MovementType == 1 && // Тип "передача"
                    m.MovementDate >= request.DateFrom &&
                    m.MovementDate <= request.DateTo)
                .ToListAsync();

            var contractNumber = string.IsNullOrWhiteSpace(request.ContractNumber)
                ? "__________"
                : request.ContractNumber;

            return await GetTransferHtmlFromTemplate(contractNumber, organization, recipient, movements);
        }

        private async Task<string> GetTransferHtmlFromTemplate(string contractNumber, Organization organization, Recipient recipient, List<MaterialMovement> movements)
        {
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "AcceptanceActTemplate.html");
            var template = await System.IO.File.ReadAllTextAsync(templatePath);

            var year = DateTime.Now.Year;
            var orgName = organization?.Name ?? "";
            var orgCode = organization?.EdrpouCode ?? "";
            var orgAddress = organization?.Address ?? "";
            var orgFio = organization?.FioForDocs ?? "";
            var city = organization?.CityForDocs ?? "";

            var sbRows = new StringBuilder();
            int index = 1;
            decimal total = 0;
            const decimal VAT_RATE = 0.2m;

            foreach (var m in movements)
            {
                var priceWithVat = m.PricePerUnit * (1 + VAT_RATE);
                var sum = m.Quantity * m.PricePerUnit;
                var sumWithVat = m.Quantity * priceWithVat;
                total += sum;

                sbRows.AppendLine($"""
            <tr>
                <td>{index++}</td>
                <td>{m.MaterialItem.Name}</td>
                <td>{m.MaterialItem.MeasurementUnit.ShortName}</td>
                <td>{m.Quantity}</td>
                <td>{m.PricePerUnit:F2}</td>
                <td>{priceWithVat:F2}</td>
                <td>{sum:F2}</td>
                <td>{sumWithVat:F2}</td>
            </tr>
        """);
            }

            var totalVat = total * VAT_RATE;
            var totalWithVat = total + totalVat;
            var today = DateTime.Today;
            var day = today.Day.ToString("D2");
            var ukCulture = new System.Globalization.CultureInfo("uk-UA");
            var monthGenitive = ukCulture.DateTimeFormat.MonthGenitiveNames[today.Month - 1];

            string F15(string? val) => string.IsNullOrWhiteSpace(val) ? "_______________" : val;
            string F30(string? val) => string.IsNullOrWhiteSpace(val) ? "______________________________" : val;
            string F50(string? val) => string.IsNullOrWhiteSpace(val) ? "__________________________________________________" : val;

            return template
                .Replace("{Year}", year.ToString())
                .Replace("{ContractNumber}", F15(contractNumber))
                .Replace("{SupplierName}", F30(orgName))
                .Replace("{SupplierEDRPOU}", F15(orgCode))
                .Replace("{SupplierAddress}", F50(orgAddress))
                .Replace("{BuyerName}", F30(recipient.Name))
                .Replace("{BuyerEDRPOU}", F15(recipient.Edrpou))
                .Replace("{BuyerAddress}", F50(recipient.Address))
                .Replace("{Items}", sbRows.ToString())
                .Replace("{TotalWithoutVat:F2}", total.ToString("F2"))
                .Replace("{TotalVat:F2}", totalVat.ToString("F2"))
                .Replace("{TotalWithVat:F2}", totalWithVat.ToString("F2"))
                .Replace("{City}", F15(city))
                .Replace("{SupplierContactPerson}", F15(orgFio))
                .Replace("{BuyerFio}", F15(recipient.ContactPerson))
                .Replace("{Day}", day)
                .Replace("{Month}", monthGenitive);
        }



    }
}
