using System;
using GCheckout.Checkout;

namespace Moolah.GoogleCheckout
{
    public interface IGoogleCheckoutRequestBuilder
    {
        CheckoutShoppingCartRequestWrapper CreateRequest(ShoppingCart shoppingCart);
        void AddOptions(CheckoutShoppingCartRequest request, CheckoutOptions options);
    }

    public class GoogleCheckoutRequestBuilder : IGoogleCheckoutRequestBuilder
    {
        private readonly GoogleCheckoutConfiguration _configuration;

        public GoogleCheckoutRequestBuilder(GoogleCheckoutConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            _configuration = configuration;
        }

        public CheckoutShoppingCartRequestWrapper CreateRequest(ShoppingCart shoppingCart)
        {
            var request = new CheckoutShoppingCartRequestWrapper(_configuration.MerchantId, _configuration.MerchantKey,
                                                                 _configuration.EnvironmentType, "GBP", 0);

            foreach (var item in shoppingCart.Items)
                request.AddItem(item);

            foreach (var discount in shoppingCart.Discounts)
                request.AddDiscount(discount);

            // TODO: Support different tax rates
            request.SetGlobalTaxRate(.2);

            return request;
        }

        public void AddOptions(CheckoutShoppingCartRequest request, CheckoutOptions options)
        {
            request.EditCartUrl = options.EditCartUrl;
            request.ContinueShoppingUrl = options.ContinueShoppingUrl;
            request.RequestBuyerPhoneNumber = options.RequestBuyerPhoneNumber;
            request.AnalyticsData = options.AnalyticsData;
        }
    }
}