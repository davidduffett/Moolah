using System;
using System.Xml.Linq;

namespace Moolah.DataCash
{
    public interface ICancelTransactionResponse : IPaymentResponse
    {
    }

    public class CancelTransactionResponse : ICancelTransactionResponse
    {
        public CancelTransactionResponse(XDocument dataCashResponse)
        {
            if (dataCashResponse == null) throw new ArgumentNullException("dataCashResponse");
            DataCashResponse = dataCashResponse;
        }

        public XDocument DataCashResponse { get; private set; }

        public string TransactionReference { get; internal set; }

        public PaymentStatus Status { get; internal set; }

        public bool IsSystemFailure { get; internal set; }

        public string FailureMessage { get; internal set; }
    }
}