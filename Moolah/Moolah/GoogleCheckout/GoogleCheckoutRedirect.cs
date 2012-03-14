namespace Moolah.GoogleCheckout
{
    public class GoogleCheckoutRedirect
    {
        public string RedirectUrl { get; set; }

        public string GoogleResponse { get; set; }

        public PaymentStatus Status { get; set; }

        public bool IsSystemFailure { get; set; }

        public string FailureMessage { get; set; }
    }
}