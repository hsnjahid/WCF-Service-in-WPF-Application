using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CurrencyTranslater.Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITranslateService" in both code and config file together.
    [ServiceContract]
    public interface ITranslateService
    {
        /// <summary>
        /// Translate the requested decimal number into words.
        /// </summary>
        [OperationContract]
        string ToWord(double number);
    }
}
