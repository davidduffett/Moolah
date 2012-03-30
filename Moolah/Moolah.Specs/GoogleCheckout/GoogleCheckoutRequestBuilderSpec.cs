using System;
using System.Linq;
using GCheckout.AutoGen;
using GCheckout.Checkout;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.GoogleCheckout;
using ShoppingCart = Moolah.GoogleCheckout.ShoppingCart;
using ShoppingCartItem = Moolah.GoogleCheckout.ShoppingCartItem;

namespace Moolah.Specs.GoogleCheckout
{
    [Subject(typeof(GoogleCheckoutRequestBuilder))]
    public class When_building_checkout_shopping_cart_request : WithFakes
    {
        It should_set_merchant_id = () =>
            Result.MerchantID.ShouldEqual(Configuration.MerchantId);

        It should_set_merchant_key = () =>
            Result.MerchantKey.ShouldEqual(Configuration.MerchantKey);

        It should_set_environment_type = () =>
            Result.Environment.ShouldEqual(Configuration.EnvironmentType);

        It should_set_global_tax_rule = () =>
            Result.DefaultTaxRules.First().taxarea.Item.ShouldBeOfType<WorldArea>();

        It should_set_global_tax_rate_of_20pc = () =>
            Result.DefaultTaxRules.First().rate.ShouldEqual(0.2d);

        It should_set_shipping_to_be_taxed = () =>
            Result.DefaultTaxRules.First().shippingtaxed.ShouldBeTrue();

        It should_contain_all_items = () =>
        {
            foreach (var item in ShoppingCart.Items)
                Result.Items.ShouldContain(x => x.Name == item.Name &&
                                                x.Description == item.Description &&
                                                x.MerchantItemID == item.MerchantItemId &&
                                                x.Quantity == item.Quantity &&
                                                x.Price == item.UnitPriceExTax);
        };

        It should_contain_all_discounts = () =>
        {
            foreach (var discount in ShoppingCart.Discounts)
                Result.Items.ShouldContain(x => x.Name == discount.Name &&
                                                x.Description == discount.Description &&
                                                x.Quantity == discount.Quantity &&
                                                x.Price == -Math.Abs(discount.AmountExTax));
        };

        Establish context = () =>
        {
            ShoppingCart = new ShoppingCart
            {
                Items = new[]
                {
                    new ShoppingCartItem { Name = "Name 1", Description = "Desc 1", MerchantItemId = "Id 1", Quantity = 1, UnitPriceExTax = 1.99m },
                    new ShoppingCartItem { Name = "Name 2", Description = "Desc 2", MerchantItemId = "Id 2", Quantity = 2, UnitPriceExTax = 2.99m }
                },
                Discounts = new[]
                {
                    new ShoppingCartDiscount { Name = "Discount Name 1", Description = "Discount Desc 1", Quantity = 1, AmountExTax = 1.99m },
                    new ShoppingCartDiscount { Name = "Discount Name 2", Description = "Discount Desc 2", Quantity = 2, AmountExTax = -2.99m }
                }
            };
            Configuration = new GoogleCheckoutConfiguration(PaymentEnvironment.Test, "merchantId", "merchantKey");
            SUT = new GoogleCheckoutRequestBuilder(Configuration);
        };

        Because of = () =>
            Result = SUT.CreateRequest(ShoppingCart);

        static GoogleCheckoutRequestBuilder SUT;
        static CheckoutShoppingCartRequest Result;
        static GoogleCheckoutConfiguration Configuration;
        static ShoppingCart ShoppingCart;
    }

    [Subject(typeof(GoogleCheckoutRequestBuilder))]
    public class When_adding_checkout_options_to_request : WithFakes
    {
        It should_set_edit_cart_url = () =>
            Request.EditCartUrl.ShouldEqual(Options.EditCartUrl);

        It should_set_continue_shopping_url = () =>
            Request.ContinueShoppingUrl.ShouldEqual(Options.ContinueShoppingUrl);

        It should_set_request_buyer_phone_number = () =>
            Request.RequestBuyerPhoneNumber.ShouldEqual(Options.RequestBuyerPhoneNumber);

        It should_set_analytics_data = () =>
            Request.AnalyticsData.ShouldEqual(Options.AnalyticsData);

        Establish context = () =>
        {
            Configuration = new GoogleCheckoutConfiguration(PaymentEnvironment.Test, "merchantId", "merchantKey");
            Options = new CheckoutOptions
                            {
                                AnalyticsData = "analytics",
                                ContinueShoppingUrl = "continue",
                                EditCartUrl = "edit",
                                RequestBuyerPhoneNumber = true
                            };
            Request = new CheckoutShoppingCartRequest(Configuration.MerchantId, Configuration.MerchantKey,
                                                      Configuration.EnvironmentType, "GBP", 0);
            SUT = new GoogleCheckoutRequestBuilder(Configuration);
        };

        Because of = () =>
            SUT.AddOptions(Request, Options);

        static GoogleCheckoutRequestBuilder SUT;
        static GoogleCheckoutConfiguration Configuration;
        static CheckoutShoppingCartRequest Request;
        static CheckoutOptions Options;
    }
}