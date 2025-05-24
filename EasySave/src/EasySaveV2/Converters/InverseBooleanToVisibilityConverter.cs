using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EasySaveV2.Converters
{
    /// <summary>
    /// Transforme false→Visible et true→Collapsed.
    /// </summary>
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool b)) return Visibility.Collapsed;
            return b ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
