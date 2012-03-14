using Machine.Specifications;
using Moolah.GoogleCheckout;

namespace Moolah.Specs.GoogleCheckout
{
    public abstract class GoogleCheckoutIntegrationContext
    {
        Establish context = () =>
        {
            ShoppingCart = new ShoppingCart
            {
                Items = new[]
                {
                    new ShoppingCartItem { Name = "Name 1", Description = "Desc 1", MerchantItemId = "Id 1", Quantity = 1, UnitPrice = 1.99m },
                    new ShoppingCartItem { Name = "Name 2", Description = "Desc 2", MerchantItemId = "Id 2", Quantity = 2, UnitPrice = 2.99m }
                },
                Discounts = new[]
                {
                    new ShoppingCartDiscount { Name = "Discount Name 1", Description = "Discount Desc 1", Quantity = 1, Amount = 1.99m },
                    new ShoppingCartDiscount { Name = "Discount Name 2", Description = "Discount Desc 2", Quantity = 2, Amount = -2.99m }
                }
            };
        };

        Because of = () =>
            Response = SUT.RequestCheckout(ShoppingCart, Options);

        protected static GoogleCheckoutGateway SUT;
        protected static GoogleCheckoutRedirect Response;
        protected static GoogleCheckoutConfiguration Configuration;
        protected static ShoppingCart ShoppingCart;
        protected static CheckoutOptions Options;
    }

    [Subject(typeof(GoogleCheckoutGateway), "Integration")]
    [Ignore("Integration requires Google Checkout Sandbox MerchantId and MerchantKey to be provided")]
    public class When_requesting_a_google_checkout : GoogleCheckoutIntegrationContext
    {
        // Change these to your Sandbox credentials
        const string MerchantId = "";
        const string MerchantKey = "";

        It should_get_a_successful_response = () =>
            Response.Status.ShouldEqual(PaymentStatus.Pending);

        It should_not_specify_a_failure_message = () =>
            Response.FailureMessage.ShouldBeNull();

        It should_provide_a_redirect_url = () =>
        {
            Response.RedirectUrl.ShouldNotBeEmpty();
            System.Diagnostics.Debug.WriteLine("Redirect to: " + Response.RedirectUrl);
        };

        Establish context = () =>
        {
            Configuration = new GoogleCheckoutConfiguration(PaymentEnvironment.Test, MerchantId, MerchantKey);
            SUT = new GoogleCheckoutGateway(Configuration);
        };
    }

    [Subject(typeof(GoogleCheckoutGateway), "Integration")]
    public class When_requesting_a_google_checkout_with_invalid_credentials : GoogleCheckoutIntegrationContext
    {
        It should_get_a_failed_response = () =>
            Response.Status.ShouldEqual(PaymentStatus.Failed);

        It should_be_a_system_failure = () =>
            Response.IsSystemFailure.ShouldBeTrue();

        It should_specify_a_failure_message = () =>
            Response.FailureMessage.ShouldNotBeNull();

        Establish context = () =>
        {
            Configuration = new GoogleCheckoutConfiguration(PaymentEnvironment.Test, "blah", "bleh");
            SUT = new GoogleCheckoutGateway(Configuration);
        };
    }
}