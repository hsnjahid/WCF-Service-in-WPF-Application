using CurrencyTranslater.Server.Algorithm;
using System;
using System.ServiceModel;

namespace CurrencyTranslater.Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TranslateService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TranslateService.svc or TranslateService.svc.cs at the Solution Explorer and start debugging.

    /// <summary>
    /// Implemntation of the translate service. 
    /// </summary>
    public sealed class TranslateService : ITranslateService
    {
        /// <summary>
        /// Translate the requested decimal number into words.
        /// </summary>
        public string ToWord(double number)
        {
            try
            {
                return CurrencyRepresenter.RepresentsToDollar(number);
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }
    }
}
