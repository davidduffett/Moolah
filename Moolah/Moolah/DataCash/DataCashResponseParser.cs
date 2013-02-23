using System.Xml.Linq;

namespace Moolah.DataCash
{
    public interface IDataCashResponseParser
    {
        ICardPaymentResponse Parse(string dataCashResponse);
    }

    public class DataCashResponseParser : IDataCashResponseParser
    {
        public ICardPaymentResponse Parse(string dataCashResponse)
        {
            var document = XDocument.Parse(dataCashResponse);

            var response = new CardPaymentPaymentResponse(document);

            string transactionReference;
            if (document.TryGetXPathValue("Response/datacash_reference", out transactionReference))
                response.TransactionReference = transactionReference;

            string avsCv2Result;
            if (document.TryGetXPathValue("Response/CardTxn/Cv2Avs/cv2avs_status", out avsCv2Result))
                response.AvsCv2Result = avsCv2Result;

            var dataCashStatus = int.Parse(document.XPathValue("Response/status"));
            response.Status = dataCashStatus == DataCashStatus.Success
                                  ? PaymentStatus.Successful
                                  : PaymentStatus.Failed;

            if (response.Status == PaymentStatus.Failed)
            {
                response.IsSystemFailure = DataCashStatus.IsSystemFailure(dataCashStatus);
                var failureReason = DataCashStatus.FailureReason(dataCashStatus);
                response.FailureMessage = failureReason.Message;
                response.FailureType = failureReason.Type;
            }
            
            return response;
        }
    }
}
