using System.ServiceModel;
using System.Threading.Tasks;

namespace CurrencyTranslater.Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITranslateService" in both code and config file together.
    [ServiceContract]
    public partial interface ITranslateService
    {
        /// <summary>
        /// Gets the state of the translate service.
        /// </summary>
        [OperationContract]
        Task<State> GetState();

        /// <summary>
        /// Gets all supported languages to translate. 
        /// </summary>
        [OperationContract]
        Task<string[]> GetSupportedLanguages();

        /// <summary>
        /// Gets the translate word from the requested decimal number.
        /// </summary>
        [OperationContract]
        Task<string> GetConvertedWord(double number);
    }
}
