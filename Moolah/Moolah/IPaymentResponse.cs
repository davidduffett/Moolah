namespace Moolah
{
    public interface IPaymentResponse
    {
        /// <summary>
        /// The reference returned by the payment gateway.
        /// </summary>
        string TransactionReference { get; }

        /// <summary>
        /// Whether the payment was successful or not.
        /// </summary>
        PaymentStatus Status { get; }

        /// <summary>
        /// Specifies whether the failure is due to a system level issue.  In these cases
        /// the failure message should NOT be shown to the customer, but a generic failure message
        /// should be shown.
        /// </summary>
        bool IsSystemFailure { get; }

        /// <summary>
        /// If <see cref="IsSystemFailure"/> is <value>false</value>, then this message can be
        /// shown directly to the customer in cases of failure.  Otherwise, an exception should be
        /// logged by the calling application, as a system error has occurred (for example, a configuration problem).
        /// </summary>
        string FailureMessage { get; }
    }
}