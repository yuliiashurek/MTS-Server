using Server.Data.Db;
using Server.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Data.Db;
using Server.Data.Entities;
using Server.Shared.DTOs;
using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Server.Core.Interfaces;

namespace Server.Core.Services
{
    public class ReportGenerationService : IReportGenerationService
    {
        private readonly AppDbContext _context;

        public ReportGenerationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateAcceptanceActHtml(AcceptanceActRequestDto request)
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

            return await FillTemplateAsync(
                templateName: "AcceptanceActTemplate.html",
                contractNumber: request.ContractNumber,
                supplierName: supplier?.Name,
                supplierEdrpou: supplier?.EdrpouCode,
                supplierAddress: supplier?.Address,
                buyerName: supplier?.Organization?.Name,
                buyerEdrpou: supplier?.Organization?.EdrpouCode,
                buyerAddress: supplier?.Organization?.Address,
                buyerFio: supplier?.Organization?.FioForDocs,
                supplierContact: supplier?.ContactPerson,
                city: supplier?.Organization?.CityForDocs,
                movements: movements);
        }

        public async Task<string> GenerateTransferActHtml(TransferActRequestDto request)
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
                    m.MovementType == 1 &&
                    m.MovementDate >= request.DateFrom &&
                    m.MovementDate <= request.DateTo)
                .ToListAsync();

            return await FillTemplateAsync(
                templateName: "AcceptanceActTemplate.html",
                contractNumber: request.ContractNumber,
                supplierName: organization?.Name,
                supplierEdrpou: organization?.EdrpouCode,
                supplierAddress: organization?.Address,
                buyerName: recipient?.Name,
                buyerEdrpou: recipient?.Edrpou,
                buyerAddress: recipient?.Address,
                buyerFio: recipient?.ContactPerson,
                supplierContact: organization?.FioForDocs,
                city: organization?.CityForDocs,
                movements: movements);
        }

        private async Task<string> FillTemplateAsync(string templateName, string? contractNumber,
            string? supplierName, string? supplierEdrpou, string? supplierAddress,
            string? buyerName, string? buyerEdrpou, string? buyerAddress,
            string? buyerFio, string? supplierContact, string? city,
            List<MaterialMovement> movements)
        {
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", templateName);
            var template = await File.ReadAllTextAsync(templatePath);

            var sbRows = new StringBuilder();
            int index = 1;
            decimal total = 0;
            const decimal VAT = 0.2m;

            foreach (var m in movements)
            {
                var priceWithVat = m.PricePerUnit * (1 + VAT);
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

            var totalVat = total * VAT;
            var totalWithVat = total + totalVat;
            var today = DateTime.Today;
            var month = new CultureInfo("uk-UA").DateTimeFormat.MonthGenitiveNames[today.Month - 1];

            string F15(string? v) => string.IsNullOrWhiteSpace(v) ? "_______________" : v;
            string F30(string? v) => string.IsNullOrWhiteSpace(v) ? "______________________________" : v;
            string F50(string? v) => string.IsNullOrWhiteSpace(v) ? "__________________________________________________" : v;

            return template
                .Replace("{Year}", today.Year.ToString())
                .Replace("{ContractNumber}", F15(contractNumber))
                .Replace("{SupplierName}", F30(supplierName))
                .Replace("{SupplierEDRPOU}", F15(supplierEdrpou))
                .Replace("{SupplierAddress}", F50(supplierAddress))
                .Replace("{BuyerName}", F30(buyerName))
                .Replace("{BuyerEDRPOU}", F15(buyerEdrpou))
                .Replace("{BuyerAddress}", F50(buyerAddress))
                .Replace("{Items}", sbRows.ToString())
                .Replace("{TotalWithoutVat:F2}", total.ToString("F2"))
                .Replace("{TotalVat:F2}", totalVat.ToString("F2"))
                .Replace("{TotalWithVat:F2}", totalWithVat.ToString("F2"))
                .Replace("{City}", F15(city))
                .Replace("{SupplierContactPerson}", F15(supplierContact))
                .Replace("{BuyerFio}", F15(buyerFio))
                .Replace("{Day}", today.Day.ToString("D2"))
                .Replace("{Month}", month);
        }
    }
}
