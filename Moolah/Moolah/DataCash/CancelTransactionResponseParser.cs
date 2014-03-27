using System.Xml.Linq;

namespace Moolah.DataCash
{
    public interface ICancelTransactionResponseParser
    {
        ICancelTransactionResponse Parse(string dataCashResponse);
    }

    public class CancelTransactionResponseParser : ICancelTransactionResponseParser
    {
        public ICancelTransactionResponse Parse(string dataCashResponse)
        {
            var document = XDocument.Parse(dataCashResponse);

            var response = new CancelTransactionResponse(document);

            string transactionReference;
            if (document.TryGetXPathValue("Response/datacash_reference", out transactionReference))
                response.TransactionReference = transactionReference;

            var dataCashStatus = int.Parse(document.XPathValue("Response/status"));
            response.Status = dataCashStatus == DataCashStatus.Success
                                  ? PaymentStatus.Successful
                                  : PaymentStatus.Failed;

            if (response.Status == PaymentStatus.Failed)
            {
                response.IsSystemFailure = DataCashStatus.IsSystemFailure(dataCashStatus);
                var failureReason = DataCashStatus.FailureReason(dataCashStatus);
                response.FailureMessage = failureReason.Message;
            }

            return response;
        }
    }
}