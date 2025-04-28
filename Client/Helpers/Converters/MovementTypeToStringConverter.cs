using System;
using System.Globalization;
using System.Windows.Data;
using Server.Shared.Enums;

namespace Client.Helpers
{
    public class MovementTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int movementType)
            {
                return movementType == 0 ? "Прийом" : "Витрата";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
