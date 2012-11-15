using System.Collections.Specialized;

namespace Moolah.PayPal
{
    public class PaypalRefundResponse
    {
        public PaymentStatus Status { get; set; }
        public string TransactionId { get; set; }
        public decimal FeeRefundAmount { get; set; }
        public decimal GrossRefundAmount { get; set; }
        public decimal NetRefundAmount { get; set; }
        public decimal TotalRefundAmount { get; set; }
    }
}