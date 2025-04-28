using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Client.Helpers
{
    public class WarehouseNameMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is Guid warehouseId && values[1] is ObservableCollection<Warehouse> warehouses)
            {
                return warehouses.FirstOrDefault(x => x.Id == warehouseId)?.Name ?? "—";
            }
            return "—";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
