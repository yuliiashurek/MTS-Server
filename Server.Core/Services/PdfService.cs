using DinkToPdf.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DinkToPdf;

namespace Server.Core.Services
{
    public class PdfService
    {
        private readonly IConverter _converter;

        public PdfService(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] GeneratePdfFromHtml(string html)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects = { new ObjectSettings { HtmlContent = html, WebSettings = { DefaultEncoding = "utf-8" } } }
            };

            return _converter.Convert(doc);
        }
    }
}
