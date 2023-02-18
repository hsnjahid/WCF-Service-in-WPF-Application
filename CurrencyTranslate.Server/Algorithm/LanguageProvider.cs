using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CurrencyTranslater.Server.Algorithm
{
    public class LanguageProvider : ILanguageProvider
    {
        private static Dictionary<string, CultureInfo> _supportedCultures = new Dictionary<string, CultureInfo>
        {
            {"en-US", new CultureInfo("en-US") },
            {"en-GB", new CultureInfo("en-GB") },
            {"de-DE", new CultureInfo("de-DE") },
        };

        private string _activeLanguageName;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string[] GetLanguages()
        {
            var cultureNames = _supportedCultures.Select(culture => culture.Value.Name).ToArray();

            return cultureNames;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsSupported(string cultureName)
        {
            return _supportedCultures.ContainsKey(cultureName);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string GetActiveLanguage()
        {
            return _activeLanguageName;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void UpdateActiveLanguage(string languageName)
        {
            _activeLanguageName = languageName;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public CultureInfo GetActiveCultureInfo()
        {
            if (_supportedCultures.TryGetValue(_activeLanguageName, out var cultureInfo))
                return cultureInfo;

            return CultureInfo.CurrentCulture;
        }
    }
}