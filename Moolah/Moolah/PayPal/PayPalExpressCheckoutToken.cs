using System;
using System.Collections.Specialized;

namespace Moolah.PayPal
{
    public class PayPalExpressCheckoutToken
    {
        public PayPalExpressCheckoutToken(NameValueCollection payPalResponse)
        {
            if (payPalResponse == null) throw new ArgumentNullException("payPalResponse");
            PayPalResponse = payPalResponse;
        }

        public NameValueCollection PayPalResponse { get; private set; }

        public string PayPalToken { get; internal set; }

        public string RedirectUrl { get; internal set; }

        public PaymentStatus Status { get; internal set; }

        public bool IsSystemFailure { get; internal set; }

        public string FailureMessage { get; internal set; }
    }
}