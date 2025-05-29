using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Core.Interfaces;
using Server.Core.Services;
using Server.Data.Db;
using Server.Data.Entities;
using Server.Shared.DTOs;
using System.Text;

namespace Server.API.Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly PdfService _pdfService;
        private readonly IReportGenerationService _reportService;

        public ReportsController(PdfService pdfService, IReportGenerationService reportService)
        {
            _pdfService = pdfService;
            _reportService = reportService;
        }

        /// <summary>
        /// Генерація акта приймання у форматі PDF.
        /// </summary>
        /// <param name="request">Дані для акта приймання.</param>
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [HttpPost("generate-acceptance-act")]
        public async Task<IActionResult> GenerateAcceptanceAct([FromBody] AcceptanceActRequestDto request)
        {
            var html = await _reportService.GenerateAcceptanceActHtml(request);
            var pdf = _pdfService.GeneratePdfFromHtml(html);
            return File(pdf, "application/pdf", "AcceptanceAct.pdf");
        }

        /// <summary>
        /// Отримання HTML-прев'ю акта приймання.
        /// </summary>
        /// <param name="request">Дані для акта приймання.</param>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost("generate-html-preview")]
        public async Task<IActionResult> GenerateHtmlPreview([FromBody] AcceptanceActRequestDto request)
        {
            var html = await _reportService.GenerateAcceptanceActHtml(request);
            return Ok(html);
        }

        /// <summary>
        /// Генерація акта передачі у форматі PDF.
        /// </summary>
        /// <param name="request">Дані для акта передачі.</param>
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [HttpPost("generate-transfer-act")]
        public async Task<IActionResult> GenerateTransferAct([FromBody] TransferActRequestDto request)
        {
            var html = await _reportService.GenerateTransferActHtml(request);
            var pdf = _pdfService.GeneratePdfFromHtml(html);
            return File(pdf, "application/pdf", "TransferAct.pdf");
        }

        /// <summary>
        /// Отримання HTML-прев'ю акта передачі.
        /// </summary>
        /// <param name="request">Дані для акта передачі.</param>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost("generate-transfer-html-preview")]
        public async Task<IActionResult> GenerateTransferHtmlPreview([FromBody] TransferActRequestDto request)
        {
            var html = await _reportService.GenerateTransferActHtml(request);
            return Ok(html);
        }

        /// <summary>
        /// Генерація порожнього HTML-шаблону акта приймання.
        /// </summary>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpGet("generate-empty-template")]
        public async Task<IActionResult> GenerateEmptyTemplate()
        {
            const string emptyPlaceholder = "_______________";

            var html = await System.IO.File.ReadAllTextAsync(
                Path.Combine(Directory.GetCurrentDirectory(), "Templates", "AcceptanceActTemplate.html"));

            html = html
                .Replace("{Year}", DateTime.Now.Year.ToString())
                .Replace("{ContractNumber}", emptyPlaceholder)
                .Replace("{BuyerName}", emptyPlaceholder)
                .Replace("{BuyerEDRPOU}", emptyPlaceholder)
                .Replace("{BuyerAddress}", emptyPlaceholder)
                .Replace("{SupplierName}", emptyPlaceholder)
                .Replace("{SupplierEDRPOU}", emptyPlaceholder)
                .Replace("{SupplierAddress}", emptyPlaceholder)
                .Replace("{Items}", string.Empty)
                .Replace("{TotalWithoutVat:F2}", "0.00")
                .Replace("{TotalVat:F2}", "0.00")
                .Replace("{TotalWithVat:F2}", "0.00")
                .Replace("{City}", emptyPlaceholder)
                .Replace("{SupplierContactPerson}", emptyPlaceholder)
                .Replace("{BuyerFio}", emptyPlaceholder)
                .Replace("{Day}", DateTime.Today.Day.ToString("D2"))
                .Replace("{Month}", new System.Globalization.CultureInfo("uk-UA").DateTimeFormat.MonthGenitiveNames[DateTime.Today.Month - 1]);

            return Ok(html);
        }

    }

}
