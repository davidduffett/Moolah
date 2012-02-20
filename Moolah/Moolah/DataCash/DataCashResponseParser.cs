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

            var response = new DataCashPaymentResponse(document);
            
            var dataCashStatus = int.Parse(document.XPathValue("Response/status"));
            response.Status = dataCashStatus == (int) DataCashStatus.Success
                                  ? PaymentStatus.Successful
                                  : PaymentStatus.Failed;

            string transactionReference;
            if (document.TryGetXPathValue("Response/datacash_reference", out transactionReference))
                response.TransactionReference = transactionReference;

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
