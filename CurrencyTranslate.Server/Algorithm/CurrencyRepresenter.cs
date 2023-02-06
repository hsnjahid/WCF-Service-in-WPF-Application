using System.Globalization;

namespace CurrencyTranslater.Server.Algorithm
{
    /// <summary>
    /// This class represents a number to currency in words.
    /// </summary>
    public sealed class CurrencyRepresenter
    {
        #region Fields

        // decimal point
        private const string _decimalPoint = ".";

        #endregion

        #region Static Methods

        /// <summary>
        /// Represents into verbal words from numbers.
        /// </summary>
        public static string RepresentsToDollar(double number)
        {
            var wholeNumberPart = (int)number; // whole number part
            var wholeNumberPartToWord = NumberTranslator.Translate(wholeNumberPart);
            var decimalPartToWords = string.Empty;

            // covert force-fully
            var cultureInfo = new CultureInfo("en-US", false);
            var decimalNumbur = number.ToString(cultureInfo);

            // gets the index of decimal point
            var decimalPointIndex = decimalNumbur.IndexOf(_decimalPoint);

            if (decimalPointIndex > 0)
            {
                var decimalPart = decimalNumbur.Substring(decimalPointIndex);
                double.TryParse(decimalPart, NumberStyles.Number, cultureInfo, out double decimalNumberPart);

                int decimalPartInteger = (int)(decimalNumberPart * 100);
                if (decimalPartInteger > 0)
                {
                    decimalPartToWords = string.Format("and {0} {1}",
                      NumberTranslator.Translate(decimalPartInteger),
                      decimalPartInteger != 1 ? "cents" : "cent");
                }
            }

            var result = string.Format("{0} {1} {2}",
                wholeNumberPartToWord.Trim(),
                wholeNumberPart != 1 ? "dollars" : "dollar",
                decimalPartToWords.Trim());

            return result.Trim();
        }

        #endregion
    }
}
