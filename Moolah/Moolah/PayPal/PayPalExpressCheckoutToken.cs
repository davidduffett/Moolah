using System.Collections.Specialized;

namespace Moolah.PayPal
{
    public class PayPalExpressCheckoutToken
    {
        public NameValueCollection PayPalResponse { get; set; }

        public string PayPalToken { get; set; }

        public string RedirectUrl { get; set; }

        public PaymentStatus Status { get; set; }

        public bool IsSystemFailure { get; set; }

        public string FailureMessage { get; set; }
    }
}