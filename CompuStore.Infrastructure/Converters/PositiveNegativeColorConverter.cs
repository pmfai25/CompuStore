using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace CompuStore.Infrastructure.Converters
{
    public class PositiveNegativeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new SolidColorBrush(Colors.Black);
            decimal v = (decimal)value;
            if (v > 0)
                return new SolidColorBrush(Colors.Green);
            else
                if (v < 0)
                return new SolidColorBrush(Colors.Red);
            else
                return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
