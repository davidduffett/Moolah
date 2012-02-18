namespace Moolah
{
    public interface IPaymentGateway
    {
        /// <summary>
        /// Attempts to transact the specified amount using the card details provided.
        /// </summary>
        /// <param name="merchantReference">The merchant's reference for the transaction.</param>
        /// <param name="amount">The amount to transact.</param>
        /// <param name="card">Credit or debit card details.</param>
        IPaymentResponse Payment(string merchantReference, decimal amount, CardDetails card);
    }
}