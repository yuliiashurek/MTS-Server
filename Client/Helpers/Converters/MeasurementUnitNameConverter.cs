using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Collections.ObjectModel;
using Client.Models;

namespace Client.Helpers
{
    public class MeasurementUnitNameMultiConverter : IMultiValueConverter
    {
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                return string.Empty;

            if (values[0] is Guid id && values[1] is ObservableCollection<MeasurementUnit> units)
            {
                var unit = units.FirstOrDefault(x => x.Id == id);
                return unit?.ShortName ?? string.Empty;
            }

            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
