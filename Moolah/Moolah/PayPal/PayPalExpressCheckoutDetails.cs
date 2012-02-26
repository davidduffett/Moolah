namespace Moolah.PayPal
{
    public class PayPalExpressCheckoutDetails
    {
        public string PayPalPayerId { get; internal set; }
        public string PayPalEmail { get; internal set; }
        public bool IsVerified { get; internal set; }

        public string PayerName { get; internal set; }
        public string ContactPhone { get; internal set; }
        public string CompanyName { get; internal set; }
        public string AddressName { get; internal set; }
        public string Street1 { get; internal set; }
        public string Street2 { get; internal set; }
        public string City { get; internal set; }
        public string State { get; internal set; }
        public string Country { get; internal set; }
        public string Postcode { get; internal set; }
        public string AddressPhone { get; internal set; }
    }
}