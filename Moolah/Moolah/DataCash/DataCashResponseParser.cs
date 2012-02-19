using System;
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

            var dataCashStatus = (DataCashStatus)Enum.Parse(typeof(DataCashStatus), document.XPathValue("Response/status"));
            var dataCashReason = (DataCashReason)Enum.Parse(typeof(DataCashReason), document.XPathValue("Response/reason"));
            
            var response = new DataCashPaymentResponse(document);
            response.TransactionReference = document.XPathValue("Response/datacash_reference");
            response.Status = dataCashStatus == DataCashStatus.Success
                                  ? PaymentStatus.Successful
                                  : PaymentStatus.Failed;
            response.Reason = null;
            return response;
        }
    }
}
