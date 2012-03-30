using System.Collections.Generic;
using GCheckout.Util;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.GoogleCheckout;

namespace Moolah.Specs.GoogleCheckout
{
    public abstract class GoogleCheckoutButtonImageContext : WithFakes
    {
        Establish context = () =>
        {
            SizeParameters = null;
            Style = ButtonStyle.White;
            Configuration = new GoogleCheckoutConfiguration(PaymentEnvironment.Test, "123456", "key");
            SUT = new GoogleCheckoutGateway(Configuration);
        };

        protected static GoogleCheckoutGateway SUT;
        protected static string Result;
        protected static GoogleCheckoutConfiguration Configuration;
        protected static string SizeParameters;
        protected static ButtonStyle Style;
    }

    [Behaviors]
    public class GoogleCheckoutButtonUrlBehavior
    {
        It should_use_the_correct_base_url = () =>
            Result.ShouldStartWith(Configuration.ButtonSrc);

        It should_include_the_merchant_id = () =>
            Result.ShouldContain("merchant_id=" + Configuration.MerchantId);

        It should_specify_the_correct_size = () =>
            Result.ShouldContain(SizeParameters);

        It should_specify_the_correct_style = () =>
            Result.ShouldContain("style=" + Style.ToString().ToLower());

        // TODO: Introduce different locales
        It should_specify_en_GB_loc = () =>
            Result.ShouldContain("loc=en_GB");

        It should_show_as_enabled = () =>
            Result.ShouldContain("variant=text");

        protected static string Result;
        protected static GoogleCheckoutConfiguration Configuration;
        protected static string SizeParameters;
        protected static ButtonStyle Style;
    }
    
    [Subject(typeof(GoogleCheckoutGateway))]
    public class When_requesting_checkout_button_image : GoogleCheckoutButtonImageContext
    {
        Behaves_like<GoogleCheckoutButtonUrlBehavior> a_google_checkout_button;

        Because of = () =>
        {
            // Small is default
            SizeParameters = "w=160&h=43";
            Result = SUT.GoogleCheckoutButtonImage();
        };
    }

    [Subject(typeof(GoogleCheckoutGateway))]
    public class When_requesting_checkout_button_small_image : GoogleCheckoutButtonImageContext
    {
        Behaves_like<GoogleCheckoutButtonUrlBehavior> a_google_checkout_button;

        Because of = () =>
        {
            SizeParameters = "w=160&h=43";
            Result = SUT.GoogleCheckoutButtonImage(ButtonSize.Small);
        };
    }

    [Subject(typeof(GoogleCheckoutGateway))]
    public class When_requesting_checkout_button_medium_image : GoogleCheckoutButtonImageContext
    {
        Behaves_like<GoogleCheckoutButtonUrlBehavior> a_google_checkout_button;

        Because of = () =>
        {
            SizeParameters = "w=168&h=44";
            Result = SUT.GoogleCheckoutButtonImage(ButtonSize.Medium);
        };
    }

    [Subject(typeof(GoogleCheckoutGateway))]
    public class When_requesting_checkout_button_large_image : GoogleCheckoutButtonImageContext
    {
        Behaves_like<GoogleCheckoutButtonUrlBehavior> a_google_checkout_button;

        Because of = () =>
        {
            SizeParameters = "w=180&h=46";
            Result = SUT.GoogleCheckoutButtonImage(ButtonSize.Large);
        };
    }

    [Subject(typeof(GoogleCheckoutGateway))]
    public class When_requesting_checkout_button_as_transparent : GoogleCheckoutButtonImageContext
    {
        Behaves_like<GoogleCheckoutButtonUrlBehavior> a_google_checkout_button;

        Because of = () =>
        {
            SizeParameters = "w=180&h=46";
            Style = ButtonStyle.Trans;
            Result = SUT.GoogleCheckoutButtonImage(ButtonSize.Large, ButtonStyle.Trans);
        };
    }

    public abstract class GoogleCheckoutRequestContext : WithFakes
    {
        Establish context = () =>
        {
            Configuration = new GoogleCheckoutConfiguration(PaymentEnvironment.Test, "merchantId", "merchantKey");
            ShoppingCart = An<ShoppingCart>();
            Request = An<CheckoutShoppingCartRequestWrapper>();
            RequestBuilder = An<IGoogleCheckoutRequestBuilder>();
            RequestBuilder.WhenToldTo(x => x.CreateRequest(ShoppingCart))
                .Return(Request);
            SUT = new GoogleCheckoutGateway(Configuration, RequestBuilder);
        };

        Because of = () =>
            Result = SUT.RequestCheckout(ShoppingCart, Options, ShippingMethods);

        protected static GoogleCheckoutGateway SUT;
        protected static GoogleCheckoutRedirect Result;
        protected static ShoppingCart ShoppingCart;
        protected static CheckoutOptions Options;
        protected static IEnumerable<ShippingMethod> ShippingMethods;
        protected static GoogleCheckoutConfiguration Configuration;
        protected static IGoogleCheckoutRequestBuilder RequestBuilder;
        protected static CheckoutShoppingCartRequestWrapper Request;
        protected static GCheckoutResponse Response;
        protected const string RedirectUrl = "http://google.checkout";
        protected const string GoogleResponse = "<google-response>";
    }

    [Behaviors]
    public class GoogleCheckoutRequestBehavior
    {
        It should_generate_and_send_the_request = () =>
            Request.WasToldTo(x => x.Send());

        It should_provide_the_google_response_xml = () =>
            Result.GoogleResponse.ShouldEqual(GoogleResponse);

        protected static GoogleCheckoutRedirect Result;
        protected static CheckoutShoppingCartRequestWrapper Request;
        protected static string GoogleResponse;
    }

    [Subject(typeof (GoogleCheckoutGateway))]
    public class When_requesting_google_checkout_and_response_is_good : GoogleCheckoutRequestContext
    {
        Behaves_like<GoogleCheckoutRequestBehavior> a_google_checkout_request;

        It should_return_the_redirect_url = () =>
            Result.RedirectUrl.ShouldEqual(RedirectUrl);

        It should_be_a_success = () =>
            Result.Status.ShouldEqual(PaymentStatus.Pending);

        Establish context = () =>
        {
            Response = new FakeGCheckoutResponse(isGood: true, responseXml: GoogleResponse, redirectUrl: RedirectUrl);
            Request.WhenToldTo(x => x.Send()).Return(Response);
        };
    }

    [Subject(typeof(GoogleCheckoutGateway))]
    public class When_requesting_google_checkout_with_options_and_shipping_methods_and_response_is_good : GoogleCheckoutRequestContext
    {
        Behaves_like<GoogleCheckoutRequestBehavior> a_google_checkout_request;

        It should_add_options_to_the_request = () =>
            RequestBuilder.WasToldTo(x => x.AddOptions(Request, Options));

        It should_add_shipping_methods_to_the_request = () =>
            RequestBuilder.WasToldTo(x => x.AddShippingMethods(Request, ShippingMethods));

        It should_return_the_redirect_url = () =>
            Result.RedirectUrl.ShouldEqual(RedirectUrl);

        It should_be_a_success = () =>
            Result.Status.ShouldEqual(PaymentStatus.Pending);

        Establish context = () =>
        {
            Options = new CheckoutOptions();
            ShippingMethods = new List<ShippingMethod> { new ShippingMethod() };
            Response = new FakeGCheckoutResponse(isGood: true, responseXml: GoogleResponse, redirectUrl: RedirectUrl);
            Request.WhenToldTo(x => x.Send()).Return(Response);
        };
    }

    [Subject(typeof(GoogleCheckoutGateway))]
    public class When_requesting_google_checkout_and_response_is_not_good : GoogleCheckoutRequestContext
    {
        Behaves_like<GoogleCheckoutRequestBehavior> a_google_checkout_request;

        It should_be_a_failure = () =>
            Result.Status.ShouldEqual(PaymentStatus.Failed);

        It should_be_a_system_failure = () =>
            Result.IsSystemFailure.ShouldBeTrue();

        It should_include_error_message = () =>
            Result.FailureMessage.ShouldEqual(GoogleErrorMessage);

        Establish context = () =>
        {
            Response = new FakeGCheckoutResponse(isGood: false, responseXml: GoogleResponse, errorMessage: GoogleErrorMessage);
            Request.WhenToldTo(x => x.Send()).Return(Response);
        };

        const string GoogleErrorMessage = "google error";
    }
}