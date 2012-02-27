using System.Collections.Specialized;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.PayPal;

namespace Moolah.Specs.PayPal
{
    public abstract class PayPalRequestBuilderContext : WithFakes
    {
        Establish context = () =>
        {
            Configuration = new PayPalConfiguration(PaymentEnvironment.Test, "testUser", "testpassword", "testsignature");
            SUT = new PayPalRequestBuilder(Configuration);
        };

        protected static PayPalRequestBuilder SUT;
        protected static NameValueCollection Request;
        protected static PayPalConfiguration Configuration;
    }

    [Behaviors]
    public class PayPalCommonRequestBehavior
    {
        It should_specify_api_version_78 = () =>
            Request["VERSION"].ShouldEqual("78");

        It should_specify_user_from_configuration = () =>
            Request["USER"].ShouldEqual(Configuration.UserId);

        It should_specify_password_from_configuration = () =>
            Request["PWD"].ShouldEqual(Configuration.Password);

        It should_specify_signature_from_configuration = () =>
            Request["SIGNATURE"].ShouldEqual(Configuration.Signature);

        protected static NameValueCollection Request;
        protected static PayPalConfiguration Configuration;
    }

    [Subject(typeof(PayPalRequestBuilder))]
    public class When_building_set_express_checkout_request : PayPalRequestBuilderContext
    {
        Behaves_like<PayPalCommonRequestBehavior> a_paypal_nvp_request;

        It should_specify_correct_method = () =>
            Request["METHOD"].ShouldEqual("SetExpressCheckout");

        It should_specify_formatted_amount = () =>
            Request["PAYMENTREQUEST_0_AMT"].ShouldEqual(Amount.ToString("0.00"));

        It should_specify_currency_code = () =>
            Request["PAYMENTREQUEST_0_CURRENCYCODE"].ShouldEqual("GBP");

        It should_specify_sale_payment_action = () =>
            Request["PAYMENTREQUEST_0_PAYMENTACTION"].ShouldEqual("Sale");

        It should_specify_cancel_url = () =>
            Request["cancelUrl"].ShouldEqual(CancelUrl);

        It should_specify_return_url = () =>
            Request["returnUrl"].ShouldEqual(ConfirmationUrl);

        Because of = () =>
            Request = SUT.SetExpressCheckout(Amount, CancelUrl, ConfirmationUrl);

        const decimal Amount = 12.99m;
        const string CancelUrl = "http://yoursite.com/paypalconfirm";
        const string ConfirmationUrl = "http://yoursite.com/basket";
    }

    [Subject(typeof(PayPalRequestBuilder))]
    public class When_building_get_express_checkout_details_request : PayPalRequestBuilderContext
    {
        Behaves_like<PayPalCommonRequestBehavior> a_paypal_nvp_request;

        It should_specify_correct_method = () =>
            Request["METHOD"].ShouldEqual("GetExpressCheckoutDetails");

        It should_specify_the_paypal_token = () =>
            Request["TOKEN"].ShouldEqual(PayPalToken);

        Because of = () =>
            Request = SUT.GetExpressCheckoutDetails(PayPalToken);

        const string PayPalToken = "tokenValue";
    }

    [Subject(typeof(PayPalRequestBuilder))]
    public class When_building_do_express_checkout_payment_request : PayPalRequestBuilderContext
    {
        Behaves_like<PayPalCommonRequestBehavior> a_paypal_nvp_request;

        It should_specify_correct_method = () =>
            Request["METHOD"].ShouldEqual("DoExpressCheckoutPayment");

        It should_specify_the_paypal_token = () =>
            Request["TOKEN"].ShouldEqual(PayPalToken);

        It should_specify_the_paypal_payer_id = () =>
            Request["PAYERID"].ShouldEqual(PayPalPayerId);

        It should_specify_formatted_amount = () =>
            Request["PAYMENTREQUEST_0_AMT"].ShouldEqual(Amount.ToString("0.00"));

        It should_specify_currency_code = () =>
            Request["PAYMENTREQUEST_0_CURRENCYCODE"].ShouldEqual("GBP");

        It should_specify_sale_payment_action = () =>
           Request["PAYMENTREQUEST_0_PAYMENTACTION"].ShouldEqual("Sale");

        Because of = () =>
            Request = SUT.DoExpressCheckoutPayment(Amount, PayPalToken, PayPalPayerId);

        const string PayPalToken = "tokenValue";
        const string PayPalPayerId = "payerId";
        const decimal Amount = 12.99m;
    }
}