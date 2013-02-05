namespace Moolah.PayPal
{
    public interface IPayPalRefundResponse : IPaymentResponse
    {
        decimal FeeRefundAmount { get; set; }
        decimal GrossRefundAmount { get; set; }
        decimal NetRefundAmount { get; set; }
        decimal TotalRefundAmount { get; set; }
    }

    public class PayPalRefundResponse : IPayPalRefundResponse
    {
        public PaymentStatus Status { get; set; }
        public bool IsSystemFailure { get; set; }
        public string FailureMessage { get; set; }
        public string TransactionReference { get; set; }
        public decimal FeeRefundAmount { get; set; }
        public decimal GrossRefundAmount { get; set; }
        public decimal NetRefundAmount { get; set; }
        public decimal TotalRefundAmount { get; set; }
    }
}