using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Client.Models;

namespace Client.Helpers
{
    public class MinStockWithUnitMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3 || values[0] == null || values[1] == null || values[2] == null)
                return "";

            var minStock = values[0] is decimal stock ? stock : 0;
            var measurementUnitId = values[1] as Guid?;
            var measurementUnits = values[2] as ObservableCollection<MeasurementUnit>;

            var unit = measurementUnits?.FirstOrDefault(u => u.Id == measurementUnitId);

            var minStockText = Properties.Resources.MiMinStock;
            if (unit == null)
                return $"{minStockText}: {minStock}";

            return $"{minStockText}: {minStock} {unit.ShortName} ({unit.FullName})";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
