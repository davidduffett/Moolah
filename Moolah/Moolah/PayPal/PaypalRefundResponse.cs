using System.Collections.Specialized;

namespace Moolah.PayPal
{
    public class PaypalRefundResponse
    {
        public PaymentStatus Status { get; set; }
        public string TransactionId { get; set; }
    }
}