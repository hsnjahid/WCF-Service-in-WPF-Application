using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CurrencyTranslate.Client.Converters
{
    /// <summary>
    /// This class converts diameter and thickness of loading spinner to stroke dash array. 
    /// </summary>
    internal class DiameterAndThicknessToStrokeDashArrayConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 ||
                !double.TryParse(values[0].ToString(), out double diameter) ||
                !double.TryParse(values[1].ToString(), out double thickness))

            {
                return 0;
            }

            double circunference = Math.PI * diameter;
            double lineLength = circunference * 0.75;
            double gapLength = circunference - lineLength;

            return new DoubleCollection(new[] { lineLength / thickness, gapLength / thickness });
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
