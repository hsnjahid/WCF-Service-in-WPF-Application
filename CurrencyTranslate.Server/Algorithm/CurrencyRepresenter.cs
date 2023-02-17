using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CurrencyTranslater.Server.Algorithm
{
    /// <summary>
    /// This class represents a number to currency in words.
    /// </summary>
    public sealed class CurrencyRepresenter
    {
        #region Fields

        private readonly object _cultureInfoLock = new object();
        private static string _cultureName;
        private static Dictionary<string, CultureInfo> _supportedCultures = new Dictionary<string, CultureInfo>
        {
            {"en-US", new CultureInfo("en-US") },
            {"en-GB", new CultureInfo("en-GB") },
            {"de-DE", new CultureInfo("de-DE") },
        };

        #endregion

        #region Methods

        /// <summary>
        /// Gets the name of the supported culture.
        /// </summary>
        public string[] GetSupportedCultures()
        {
            var cultureNames = _supportedCultures.Select(culture => culture.Value.Name).ToArray();

            return cultureNames;
        }

        /// <summary>
        /// Update the name of the culture to be translated.
        /// </summary>
        /// <returns>False when the the language is not supported</returns>
        public bool UpdateLanguage(string language)
        {
            lock (_cultureInfoLock)
            {
                if (_cultureName == language)
                    return true;

                if (!_supportedCultures.ContainsKey(language))
                    return false;
                // update 
                _cultureName = language;
                return true;
            }
        }

        /// <summary>
        /// Represents into verbal words from numbers.
        /// </summary>
        public string GetWord(string number)
        {
            if (_cultureName == null)
            {
                throw new InvalidOperationException($"The target language is not set");
            }

            if (!_supportedCultures.TryGetValue(_cultureName, out var cultureInfo))
            {
                throw new InvalidOperationException($"Does not support translate service for the language {_cultureName}");
            }

            if (!double.TryParse(number, NumberStyles.Number, cultureInfo, out var givenNumber))
            {
                throw new InvalidOperationException("Supports number only");
            }

            var wholeNumberPart = (int)givenNumber; // whole number part
            var wholeNumberPartToWord = NumberTranslator.Translate(wholeNumberPart);
            var decimalPartToWords = string.Empty;

            // gets the index of decimal point
            var decimalPointIndex = number.IndexOf(cultureInfo.NumberFormat.CurrencyDecimalSeparator);
         
            var currency = GetCurrency(cultureInfo);

            if (decimalPointIndex > 0)
            {
                var decimalPart = number.Substring(decimalPointIndex);
                double.TryParse(decimalPart, NumberStyles.Number, cultureInfo, out double decimalNumberPart);

                int decimalPartInteger = (int)(decimalNumberPart * 100);
                if (decimalPartInteger > 0)
                {
                    decimalPartToWords = string.Format("and {0} {1}",
                      NumberTranslator.Translate(decimalPartInteger),
                      decimalPartInteger != 1 ? $"{currency.DecimalPart}s" : currency.DecimalPart);
                }
            }

            var result = string.Format("{0} {1} {2}",
                wholeNumberPartToWord.Trim(),
                wholeNumberPart != 1 ? $"{currency.WholePart}s" : currency.WholePart,
                decimalPartToWords.Trim());

            return result.Trim();
        }

        /// <summary>
        /// Gets the name of the currency from the given CaltureInfo.
        /// </summary>
        private (string WholePart, string DecimalPart) GetCurrency(CultureInfo cultureInfo)
        {
            var currency = new RegionInfo(cultureInfo.LCID).CurrencyEnglishName;

            return (currency, "Cent");
        }

        #endregion
    }
}
