using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace CompuStore.Register.Converter
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string v = (string)value;
            if (v == "C")
                return new SolidColorBrush(Colors.White);
            if (v == "E")
                return new SolidColorBrush(Colors.Red);
            return new SolidColorBrush(Colors.Yellow);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
