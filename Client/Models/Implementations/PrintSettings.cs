using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public enum PrintMode
    {
        BarcodeOnly,
        BarcodeAndText
    }

    public class PrintSettings
    {
        public PrintMode PrintMode { get; set; }
        public int Copies { get; set; } = 1;
    }
}
