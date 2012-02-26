using System;

namespace Moolah.PayPal
{
    public class PayPalExpressCheckoutToken
    {
        public PayPalExpressCheckoutToken(string payPalResponse)
        {
            if (string.IsNullOrEmpty(payPalResponse)) throw new ArgumentNullException("payPalResponse");
            PayPalResponse = payPalResponse;
        }

        public string PayPalResponse { get; private set; }

        public string PayPalToken { get; internal set; }

        public string RedirectUrl { get; internal set; }

        public PaymentStatus Status { get; internal set; }

        public bool IsSystemFailure { get; internal set; }

        public string FailureMessage { get; internal set; }
    }
}