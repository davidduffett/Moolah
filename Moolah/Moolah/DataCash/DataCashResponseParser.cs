using System.Xml.Linq;

namespace Moolah.DataCash
{
    public interface IDataCashResponseParser
    {
        IPaymentResponse Parse(string dataCashResponse);
    }

    public class DataCashResponseParser : IDataCashResponseParser
    {
        public IPaymentResponse Parse(string dataCashResponse)
        {
            var document = XDocument.Parse(dataCashResponse);


            var dataCashStatus = int.Parse(document.XPathValue("Response/status"));
            var response = new DataCashPaymentResponse(document)
                               {
                                   TransactionReference = document.XPathValue("Response/datacash_reference"),
                                   Status = dataCashStatus == (int) DataCashStatus.Success
                                                ? PaymentStatus.Successful
                                                : PaymentStatus.Failed
                               };

            if (response.Status == PaymentStatus.Failed)
            {
                response.IsSystemFailure = !DataCashFailureMessages.CleanFailures.ContainsKey(dataCashStatus);
                if (!response.IsSystemFailure)
                    response.FailureMessage = DataCashFailureMessages.CleanFailures[dataCashStatus];
                else
                {
                    string failureMessage;
                    response.FailureMessage = DataCashFailureMessages.SystemFailures.TryGetValue(dataCashStatus, out failureMessage) 
                        ? failureMessage 
                        : string.Format("Unknown DataCash status code: {0}", dataCashStatus);
                }
            }
            
            return response;
        }
    }
}
