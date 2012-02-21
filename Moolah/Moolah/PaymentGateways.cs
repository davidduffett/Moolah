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

    public interface I3DSecurePaymentGateway
    {
        /// <summary>
        /// Attempts to make a card payment.  If the card is enrolled in 3D-Secure, then the Access Control Server URL (ACSUrl)
        /// and Payer Authentication Request (PARes) are returned for the calling site to redirect or show in an iframe.
        /// This should then be followed by a call to <see cref="Authorise"/> with the resulting PARes returned by the ACS.
        /// </summary>
        /// <param name="merchantReference">The merchant's reference for the transaction.</param>
        /// <param name="amount">The amount to transact.</param>
        /// <param name="card">Card details.</param>
        I3DSecureResponse Payment(string merchantReference, decimal amount, CardDetails card);

        /// <summary>
        /// Attempts to authorise a 3D-Secure transaction previously submitted to the <see cref="Payment"/> method.
        /// </summary>
        /// <param name="transactionReference">Transaction reference returned by the Gateway for the original 3D-Secure payment request.</param>
        /// <param name="PARes">Payer Authentication Response (PARes) returned by the bank in response to the user entering their 3D-Secure credentials.</param>
        I3DSecureResponse Authorise(string transactionReference, string PARes);
    }
}
