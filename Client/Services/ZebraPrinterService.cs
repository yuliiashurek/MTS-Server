using Client.Models;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    public class ZebraPrinterService : IPrinterService
    {
        private readonly string _printerIp;
        private readonly int _printerPort;

        public ZebraPrinterService(string printerIp, int printerPort = 9100)
        {
            _printerIp = printerIp;
            _printerPort = printerPort;
        }

        public async Task PrintMaterialMovementAsync(MaterialMovement movement, PrintSettings settings)
        {
            if (movement == null) return;

            string zpl = BuildZpl(movement, settings);

            try
            {
                using var tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(_printerIp, _printerPort);
                using var stream = tcpClient.GetStream();
                using var writer = new StreamWriter(stream, Encoding.ASCII);
                for (int i = 0; i < settings.Copies; i++)
                {
                    await writer.WriteAsync(zpl);
                    await writer.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка під час друку: {ex.Message}");
            }
        }

        private string BuildZpl(MaterialMovement movement, PrintSettings settings)
        {
            int widthDots = 812;
            int heightDots = 1218;
            int margin = 30;
            int contentWidth = widthDots - margin * 2;
            int fontSizeLarge = 50;
            int fontSizeMedium = 40;
            int fontSizeSmall = 35;
            int barcodeHeight = 200;
            int barcodeModuleWidth = 3;

            string barcodeData = !string.IsNullOrEmpty(movement.BarcodeNumber) ? movement.BarcodeNumber : movement.Id.ToString();
            int currentY = margin;

            var zpl = new StringBuilder();
            zpl.AppendLine("^XA");
            zpl.AppendLine($"^PW{widthDots}");
            zpl.AppendLine($"^LL{heightDots}");

            // Рамка
            zpl.AppendLine($"^FO10,10^GB{widthDots - 20},{heightDots - 20},4^FS");

            // Заголовок
            zpl.AppendLine($"^FO{margin},{currentY}^A0N,{fontSizeLarge},{fontSizeLarge}^FDMaterial Tracking System^FS");
            currentY += fontSizeLarge + 10;

            // Лінія під заголовком
            zpl.AppendLine($"^FO{margin},{currentY}^GB{contentWidth},2,2^FS");
            currentY += 20;

            if (settings.PrintMode == PrintMode.BarcodeAndText)
            {
                // Детальна інформація
                zpl.AppendLine($"^FO{margin},{currentY}^A0N,{fontSizeSmall},{fontSizeSmall}^FDMaterial: {movement.MaterialItemName ?? "-"}^FS");
                currentY += fontSizeSmall + 5;

                zpl.AppendLine($"^FO{margin},{currentY}^A0N,{fontSizeSmall},{fontSizeSmall}^FDQuantity: {movement.Quantity:F2}^FS");
                currentY += fontSizeSmall + 5;

                zpl.AppendLine($"^FO{margin},{currentY}^A0N,{fontSizeSmall},{fontSizeSmall}^FDSupplier: {movement.SupplierName ?? "-"}^FS");
                currentY += fontSizeSmall + 5;

                zpl.AppendLine($"^FO{margin},{currentY}^A0N,{fontSizeSmall},{fontSizeSmall}^FDCategory: {movement.CategoryName ?? "-"}^FS");
                currentY += fontSizeSmall + 5;

                zpl.AppendLine($"^FO{margin},{currentY}^A0N,{fontSizeSmall},{fontSizeSmall}^FDWarehouse: {movement.WarehouseName ?? "-"}^FS");
                currentY += fontSizeSmall + 5;

                if (movement.ExpirationDate.HasValue)
                {
                    zpl.AppendLine($"^FO{margin},{currentY}^A0N,{fontSizeSmall},{fontSizeSmall}^FDExpiry: {movement.ExpirationDate:dd.MM.yyyy}^FS");
                    currentY += fontSizeSmall + 5;
                }

                // Лінія перед штрихкодом
                currentY += 10;
                zpl.AppendLine($"^FO{margin},{currentY}^GB{contentWidth},2,2^FS");
                currentY += 30;
            }
            else
            {
                // Якщо тільки штрихкод
                zpl.AppendLine($"^FO{margin},{currentY}^GB{contentWidth},2,2^FS");
                currentY += 30;

                // Текст над штрихкодом
                zpl.AppendLine($"^FO{margin},{currentY}^FB{contentWidth},1,0,C^A0N,{fontSizeMedium},{fontSizeMedium}^FD{barcodeData}^FS");
                currentY += fontSizeMedium + 20;
            }

            // Штрихкод
            int barcodeWidthEstimate = (barcodeData.Length + 2) * barcodeModuleWidth * 11;
            int barcodeX = (widthDots - barcodeWidthEstimate) / 2;

            zpl.AppendLine($"^FO{barcodeX},{currentY}^BY{barcodeModuleWidth},3,{barcodeHeight}");
            zpl.AppendLine("^BCN,,Y,N,N");
            zpl.AppendLine($"^FD{barcodeData}^FS");

            zpl.AppendLine("^XZ");

            return zpl.ToString();
        }



    }
}
