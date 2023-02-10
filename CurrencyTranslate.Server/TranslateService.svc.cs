using CurrencyTranslater.Server.Algorithm;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CurrencyTranslater.Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TranslateService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TranslateService.svc or TranslateService.svc.cs at the Solution Explorer and start debugging.

    /// <summary>
    /// The implementation of the translate service. 
    /// </summary>
    public sealed class TranslateService : ITranslateService
    {
        #region Fields

        private readonly CurrencyRepresenter _currencyRepresenter = new CurrencyRepresenter();

        #endregion

        #region Interface-Implementation

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Task<State> GetState()
        {
            return Task.FromResult(State.Ready);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Task<string[]> GetSupportedLanguages()
        {
            var languages = _currencyRepresenter.GetSupportedCultures();

            return Task.FromResult(languages);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Task<string> GetConvertedWord(double number)
        {
            try
            {
                var result = _currencyRepresenter.GetWord(number);

                return Task.FromResult(result);
            }
            catch (Exception e)
            {
                return Task.FromException<string>(new FaultException(e.Message));
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Task UpdateLanguage(string language)
        {
            if (_currencyRepresenter.UpdateLanguage(language))
            {
                return Task.CompletedTask;
            }
            else
            {
                return Task.FromException(new FaultException($"Does not support language {language}"));
            }
        }

        #endregion
    }
}
