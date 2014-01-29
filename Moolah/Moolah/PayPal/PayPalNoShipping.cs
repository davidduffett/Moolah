namespace Moolah.PayPal
{
    /// <summary>
    /// Determines where or not PayPal displays shipping address fields on the PayPal pages.
    /// https://developer.paypal.com/docs/classic/api/merchant/SetExpressCheckout_API_Operation_NVP/
    /// </summary>
    public enum PayPalNoShipping
    {
        /// <summary>
        /// Shipping address fields will be displayed on payment pages.
        /// </summary>
        Display = 0,
        /// <summary>
        /// If selling digital goods, shipping address must not be displayed.
        /// </summary>
        DoNotDisplay = 1,
        /// <summary>
        /// If you do not pass the shipping address to PayPal, it will be obtained from the buyer's account profile.
        /// </summary>
        Required = 2
    }
}