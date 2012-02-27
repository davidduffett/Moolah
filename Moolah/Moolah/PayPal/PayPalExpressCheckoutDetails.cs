using System;
using System.Collections.Specialized;

namespace Moolah.PayPal
{
    public class PayPalExpressCheckoutDetails
    {
        public PayPalExpressCheckoutDetails(NameValueCollection payPalResponse)
        {
            if (payPalResponse == null) throw new ArgumentNullException("payPalResponse");
            PayPalResponse = payPalResponse;
        }

        public NameValueCollection PayPalResponse { get; private set; }

        public string PayPalPayerId { get; internal set; }
        public string PayPalEmail { get; internal set; }

        public string CustomerTitle { get; internal set; }
        public string CustomerFirstName { get; internal set; }
        public string CustomerLastName { get; internal set; }
        public string CustomerPhoneNumber { get; internal set; }
        public string CustomerMarketingEmail { get; internal set; }

        public string DeliveryName { get; internal set; }
        public string DeliveryPhoneNumber { get; internal set; }
        public string DeliveryCompanyName { get; internal set; }
        public string DeliveryStreet1 { get; internal set; }
        public string DeliveryStreet2 { get; internal set; }
        public string DeliveryCity { get; internal set; }
        public string DeliveryState { get; internal set; }
        public string DeliveryCountryCode { get; internal set; }
        public string DeliveryPostcode { get; internal set; }
    }
}