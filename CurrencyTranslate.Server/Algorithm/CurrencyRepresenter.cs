using System;
using System.Globalization;

namespace CurrencyTranslater.Server.Algorithm
{
    /// <summary>
    /// This class represents a number to currency in words.
    /// </summary>
    public sealed class CurrencyRepresenter
    {
        #region Fields

        private readonly object _cultureInfoLock = new object();
        private readonly ILanguageProvider _languageProvider;

        #endregion

        #region Constructors

        public CurrencyRepresenter(ILanguageProvider languageProvider)
        {
            _languageProvider = languageProvider;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Gets the name of the supported culture.
        /// </summary>
        public string[] GetSupportedCultures()
        {
            var cultureNames = _languageProvider.GetLanguages();

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
                if (!_languageProvider.IsSupported(language))
                    return false;

                // update 
                _languageProvider.UpdateActiveLanguage(language);

                return true;
            }
        }

        /// <summary>
        /// Represents into verbal words from numbers.
        /// </summary>
        public string GetWord(string number)
        {
            if (string.IsNullOrEmpty(_languageProvider.GetActiveLanguage()))
            {
                throw new InvalidOperationException($"The target language is not set");
            }

            var cultureInfo = _languageProvider.GetActiveCultureInfo();
 
            if (!double.TryParse(number, NumberStyles.Number, cultureInfo, out var givenNumber))
            {
                throw new NotSupportedException("Supports number only");
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
