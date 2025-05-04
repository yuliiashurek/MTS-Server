using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Client.Helpers
{
    public class BoolToBrushConverter : IValueConverter
    {
        public Brush ForecastBrush { get; set; } = Brushes.Gray;
        public Brush ActualBrush { get; set; } = Brushes.SteelBlue;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? ForecastBrush : ActualBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
