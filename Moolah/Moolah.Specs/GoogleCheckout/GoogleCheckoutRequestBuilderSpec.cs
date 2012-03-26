using System;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.GoogleCheckout;
using Moq;
using It = Machine.Specifications.It;

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

        It should_set_british_vat_20pc = () =>
            Result.GlobalTaxRate.ShouldEqual(.2);

        It should_contain_all_items = () =>
        {
            foreach (var item in ShoppingCart.Items)
                Result.Items.ShouldContain(x => x.Name == item.Name &&
                                                x.Description == item.Description &&
                                                x.MerchantItemID == item.MerchantItemId &&
                                                x.Quantity == item.Quantity);
        };

        It should_set_unit_prices_as_ex_tax = () =>
        {
            foreach (var item in ShoppingCart.Items)
                Result.Items.ShouldContain(x => x.MerchantItemID == item.MerchantItemId &&
                                                x.Price == (item.UnitPrice - item.Tax));
        };

        It should_contain_all_discounts = () =>
        {
            foreach (var discount in ShoppingCart.Discounts)
                Result.Items.ShouldContain(x => x.Name == discount.Name &&
                                                x.Description == discount.Description &&
                                                x.Quantity == discount.Quantity);
        };

        It should_set_discount_amounts_as_ex_tax = () =>
        {
            foreach (var discount in ShoppingCart.Discounts)
                Result.Items.ShouldContain(x => x.Name == discount.Name &&
                                                x.Price == -(Math.Abs(discount.Amount) - Math.Abs(discount.Tax)));
        };

        Establish context = () =>
        {
            ShoppingCart = new ShoppingCart
            {
                Items = new[]
                {
                    new ShoppingCartItem { Name = "Name 1", Description = "Desc 1", MerchantItemId = "Id 1", Quantity = 1, UnitPrice = 1.99m, Tax = 0.99m },
                    new ShoppingCartItem { Name = "Name 2", Description = "Desc 2", MerchantItemId = "Id 2", Quantity = 2, UnitPrice = 2.99m, Tax = 1.3m }
                },
                Discounts = new[]
                {
                    new ShoppingCartDiscount { Name = "Discount Name 1", Description = "Discount Desc 1", Quantity = 1, Amount = 1.99m, Tax = 0m },
                    new ShoppingCartDiscount { Name = "Discount Name 2", Description = "Discount Desc 2", Quantity = 2, Amount = -2.99m, Tax = 1.3m }
                }
            };
            Configuration = new GoogleCheckoutConfiguration(PaymentEnvironment.Test, "merchantId", "merchantKey");
            SUT = new GoogleCheckoutRequestBuilder(Configuration);
        };

        Because of = () =>
            Result = SUT.CreateRequest(ShoppingCart);

        static GoogleCheckoutRequestBuilder SUT;
        static CheckoutShoppingCartRequestWrapper Result;
        static GoogleCheckoutConfiguration Configuration;
        static ShoppingCart ShoppingCart;
    }

    [Subject(typeof(GoogleCheckoutRequestBuilder))]
    public class When_adding_checkout_options_to_request : WithFakes
    {
        It should_set_edit_cart_url = () =>
            RequestMock.Object.EditCartUrl.ShouldEqual(Options.EditCartUrl);

        It should_set_continue_shopping_url = () =>
            RequestMock.Object.ContinueShoppingUrl.ShouldEqual(Options.ContinueShoppingUrl);

        It should_set_request_buyer_phone_number = () =>
            RequestMock.Object.RequestBuyerPhoneNumber.ShouldEqual(Options.RequestBuyerPhoneNumber);

        It should_set_analytics_data = () =>
            RequestMock.Object.AnalyticsData.ShouldEqual(Options.AnalyticsData);

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
            RequestMock = new Mock<CheckoutShoppingCartRequestWrapper>();
            RequestMock.SetupAllProperties();
            SUT = new GoogleCheckoutRequestBuilder(Configuration);
        };

        Because of = () =>
            SUT.AddOptions(RequestMock.Object, Options);

        static GoogleCheckoutRequestBuilder SUT;
        static GoogleCheckoutConfiguration Configuration;
        static Mock<CheckoutShoppingCartRequestWrapper> RequestMock;
        static CheckoutOptions Options;
    }
}