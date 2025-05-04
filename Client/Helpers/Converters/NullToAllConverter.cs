using System;
using System.Globalization;
using System.Windows.Data;

namespace Client.Helpers
{
    public class NullToAllConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ?? "Усі";
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() == "Усі" ? null : value;
        }
    }
}
