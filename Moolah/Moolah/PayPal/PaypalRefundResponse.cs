namespace Moolah.PayPal
{
    public class PayPalRefundResponse
    {
        public PaymentStatus Status { get; set; }
        public string TransactionId { get; set; }
        public decimal FeeRefundAmount { get; set; }
        public decimal GrossRefundAmount { get; set; }
        public decimal NetRefundAmount { get; set; }
        public decimal TotalRefundAmount { get; set; }
    }
}