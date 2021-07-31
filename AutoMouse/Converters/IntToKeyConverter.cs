using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace AutoMouse.Converters
{
    public class IntToKeyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value is int) == false)
                throw new ArgumentException("Value is not Integer Type.");
            return (Key)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value is Key) == false)
                throw new ArgumentException("Value is not Key Type.");
            return (int)value;
        }
    }
}
