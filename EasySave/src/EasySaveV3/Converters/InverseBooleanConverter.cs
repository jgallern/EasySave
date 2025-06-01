using System;
using System.Globalization;
using System.Windows.Data;

namespace EasySaveV3.Converters
{
    /// <summary>
    /// Inverse a boolean
    /// </summary>
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => !(value is bool b) ? false : !b;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => !(value is bool b) ? false : !b;
    }
}
