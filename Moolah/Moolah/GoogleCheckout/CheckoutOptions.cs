namespace Moolah.GoogleCheckout
{
    public class CheckoutOptions
    {
        /// <summary>
        /// Optional. The URL that allows the buyer to make changes to the shopping cart before confirming an order.
        /// </summary>
        public string EditCartUrl { get; set; }
        /// <summary>
        /// Optional. The URL that allows the buyer to continue shopping after confirming an order.
        /// </summary>
        public string ContinueShoppingUrl { get; set; }
        /// <summary>
        /// Whether the customer must enter a phone number to complete an order.
        /// </summary>
        public bool RequestBuyerPhoneNumber { get; set; }
        /// <summary>
        /// Optional. Enables merchants to use Google Analytics to track Checkout orders.
        /// See http://code.google.com/apis/checkout/developer/checkout_analytics_integration.html
        /// </summary>
        public string AnalyticsData { get; set; }
    }
}