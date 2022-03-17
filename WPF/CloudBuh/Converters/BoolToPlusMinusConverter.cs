using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudBuh.Converters
{
    [System.Windows.Data.ValueConversion(typeof(bool), typeof(string))]
    class BoolToPlusMinusConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "+" : "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value as string == "+";
        }
    }
}
