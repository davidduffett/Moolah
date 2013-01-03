using System;
using System.Xml.Linq;

namespace Moolah.DataCash
{
    public interface IRefundTransactionResponse : IPaymentResponse
    {
    }

    public class RefundTransactionResponse : IRefundTransactionResponse
    {
        public RefundTransactionResponse(XDocument dataCashResponse)
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