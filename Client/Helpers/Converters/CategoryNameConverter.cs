using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Collections.ObjectModel;
using Client.Models;

namespace Client.Helpers
{
    public class CategoryNameMultiConverter : IMultiValueConverter
    {
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                return string.Empty;

            if (values[0] is Guid id && values[1] is ObservableCollection<Category> categories)
            {
                var category = categories.FirstOrDefault(x => x.Id == id);
                return category?.Name ?? string.Empty;
            }

            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
