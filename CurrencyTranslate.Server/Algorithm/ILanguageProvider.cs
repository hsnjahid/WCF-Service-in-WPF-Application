using System.Globalization;

namespace CurrencyTranslater.Server.Algorithm
{
    public interface ILanguageProvider
    {
        /// <summary>
        /// Gets all supported languages
        /// </summary>
        string[] GetLanguages();

        /// <summary>
        /// Return a boolean value which indicates a culture is supported or not.
        /// </summary>
        bool IsSupported(string cultureName);
        
        /// <summary>
        /// Gets active language
        /// </summary>
        string GetActiveLanguage();
        
        /// <summary>
        /// Sets active language
        /// </summary>
        void UpdateActiveLanguage(string languageName);

        /// <summary>
        /// Gets active CultureInfo from cultureName text.
        /// </summary>
        CultureInfo GetActiveCultureInfo();
    }
}