using System;
using System.Collections.Generic;
using GCheckout.Checkout;

namespace Moolah.GoogleCheckout
{
    public interface IGoogleCheckoutRequestBuilder
    {
        CheckoutShoppingCartRequest CreateRequest(ShoppingCart shoppingCart);
        void AddOptions(CheckoutShoppingCartRequest request, CheckoutOptions options);
        void AddShippingMethods(CheckoutShoppingCartRequest request, IEnumerable<ShippingMethod> shippingMethods);
    }

    public class GoogleCheckoutRequestBuilder : IGoogleCheckoutRequestBuilder
    {
        private readonly GoogleCheckoutConfiguration _configuration;

        public GoogleCheckoutRequestBuilder(GoogleCheckoutConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            _configuration = configuration;
        }

        public CheckoutShoppingCartRequest CreateRequest(ShoppingCart shoppingCart)
        {
            var request = new CheckoutShoppingCartRequest(_configuration.MerchantId, _configuration.MerchantKey,
                                                          _configuration.EnvironmentType, "GBP", 0);

            foreach (var item in shoppingCart.Items)
                request.AddItem(item.Name, item.Description, item.MerchantItemId, item.UnitPriceExTax, item.Quantity);

            foreach (var discount in shoppingCart.Discounts)
                request.AddItem(discount.Name, discount.Description, -Math.Abs(discount.AmountExTax), discount.Quantity ?? 1);

            // TODO: Support different tax rates
            request.AddWorldAreaTaxRule(.2d, true);

            return request;
        }

        public void AddOptions(CheckoutShoppingCartRequest request, CheckoutOptions options)
        {
            request.EditCartUrl = options.EditCartUrl;
            request.ContinueShoppingUrl = options.ContinueShoppingUrl;
            request.RequestBuyerPhoneNumber = options.RequestBuyerPhoneNumber;
            request.AnalyticsData = options.AnalyticsData;
        }

        public void AddShippingMethods(CheckoutShoppingCartRequest request, IEnumerable<ShippingMethod> shippingMethods)
        {
            foreach (var shippingMethod in shippingMethods)
            {
                // Is there a way for me to test this method call?
                request.AddFlatRateShippingMethod(shippingMethod.Name, shippingMethod.PriceExTax,
                                                  shippingMethod.ToShippingRestrictions());
            }
        }
    }
}