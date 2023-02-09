using CurrencyTranslater.Server.Algorithm;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CurrencyTranslater.Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TranslateService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TranslateService.svc or TranslateService.svc.cs at the Solution Explorer and start debugging.

    /// <summary>
    /// The implemntation of the translate service. 
    /// </summary>
    public sealed class TranslateService : ITranslateService
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<State> GetState()
        {
            return await Task.FromResult(State.Ready);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Task<string[]> GetSupportedLanguages()
        {
            var languages = new[] { "de-DE", "en-US", "en-GB" };

            return Task.FromResult(languages);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Task<string> GetConvertedWord(double number)
        {
            try
            {
                var result = CurrencyRepresenter.RepresentsToDollar(number);

                return Task.FromResult(result);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
    }
}
