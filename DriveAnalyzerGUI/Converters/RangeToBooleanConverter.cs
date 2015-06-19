using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DriveAnalyzerGUI
{
    public class RangeToBooleanConverter : IValueConverter
    {
        private const long Gigabyte = 1073741824;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((long)value) >= Gigabyte;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("Nope");
        }
    }
}
