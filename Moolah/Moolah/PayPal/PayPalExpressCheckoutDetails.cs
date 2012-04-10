using System;
using System.Collections.Specialized;

namespace Moolah.PayPal
{
    [Serializable]
    public class PayPalExpressCheckoutDetails
    {
        public PayPalExpressCheckoutDetails()
        {
            OrderDetails = new OrderDetails();
        }

        public PayPalExpressCheckoutDetails(NameValueCollection payPalResponse) : this()
        {
            if (payPalResponse == null) throw new ArgumentNullException("payPalResponse");
            PayPalResponse = payPalResponse;
        }

        public NameValueCollection PayPalResponse { get; private set; }

        public string PayPalPayerId { get; set; }
        public string PayPalEmail { get; set; }

        public string CustomerTitle { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerMarketingEmail { get; set; }

        public string DeliveryName { get; set; }
        public string DeliveryPhoneNumber { get; set; }
        public string DeliveryCompanyName { get; set; }
        public string DeliveryStreet1 { get; set; }
        public string DeliveryStreet2 { get; set; }
        public string DeliveryCity { get; set; }
        public string DeliveryState { get; set; }
        public string DeliveryCountryCode { get; set; }
        public string DeliveryPostcode { get; set; }

        public OrderDetails OrderDetails { get; set; }
    }
}