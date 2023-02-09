using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CurrencyTranslate.Client.Converters
{
    /// <summary>
    /// This class inverts a boolean value to vsibility (true => Collapsed, false => Visible)
    /// </summary>
    internal class BooleanToVisibilityInverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!bool.TryParse(value.ToString(), out bool visibility))
                return Visibility.Collapsed;

            return visibility ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
