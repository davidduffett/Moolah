namespace Moolah
{
    public interface IPaymentResponse
    {
        string TransactionReference { get; }
        PaymentStatus Status { get; }
        string Reason { get; }
    }
}