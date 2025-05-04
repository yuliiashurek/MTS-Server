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
                ? $"A-{DateTime.Now:yyyy}-{new Random().Next(1000, 9999)}"
                : request.ContractNumber;

            return BuildHtml(contractNumber, supplier, movements);
        }


        private string BuildHtml(string contractNumber, Supplier supplier, List<MaterialMovement> movements)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset=\"utf-8\">");

            // Підключаємо шрифт Google Fonts
            sb.AppendLine("<style>");
            sb.AppendLine("@import url('https://fonts.googleapis.com/css2?family=Noto+Sans&display=swap');");
            sb.AppendLine("body { font-family: 'Noto Sans', sans-serif; font-size: 12pt; }");
            sb.AppendLine("table { border-collapse: collapse; width: 100%; }");
            sb.AppendLine("td, th { border: 1px solid black; padding: 4px; }");
            sb.AppendLine("h2 { text-align: center; }");
            sb.AppendLine("</style>");

            sb.AppendLine("</head>");
            sb.AppendLine("<body>");

            sb.AppendLine($"<h2>Акт приймання-передачі товару</h2>");
            sb.AppendLine($"<p>Договір № {contractNumber}</p>");
            sb.AppendLine($"<p>Постачальник: {supplier.Name}, ЄДРПОУ: {supplier.EdrpouCode}, Адреса: {supplier.Address}</p>");

            sb.AppendLine("<table>");
            sb.AppendLine("<tr><th>№</th><th>Назва</th><th>Од.вим.</th><th>К-сть</th><th>Ціна</th><th>Сума</th></tr>");

            int index = 1;
            foreach (var m in movements)
            {
                var sum = m.Quantity * m.PricePerUnit;
                sb.AppendLine($"<tr><td>{index++}</td><td>{m.MaterialItem.Name}</td><td>{m.MaterialItem.MeasurementUnit.ShortName}</td><td>{m.Quantity}</td><td>{m.PricePerUnit:F2}</td><td>{sum:F2}</td></tr>");
            }

            sb.AppendLine("</table>");
            sb.AppendLine("</body></html>");

            return sb.ToString();
        }

    }
}
