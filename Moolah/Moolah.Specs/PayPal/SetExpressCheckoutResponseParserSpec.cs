using Machine.Specifications;
using Moolah.PayPal;

namespace Moolah.Specs.PayPal
{
    [Subject(typeof(SetExpressCheckoutResponseParser))]
    public class When_set_express_checkout_response_is_successful
    {
        It should_have_payment_status_of_pending = () =>
            Response.Status.ShouldEqual(PaymentStatus.Pending);

        It should_have_the_correct_token = () =>
            Response.PayPalToken.ShouldEqual("EC-5X481106MU8743929");

        It should_have_the_correct_redirect_url = () =>
            Response.RedirectUrl.ShouldEqual("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=EC-5X481106MU8743929");

        Establish context = () =>
            SUT = new SetExpressCheckoutResponseParser(new PayPalConfiguration(PaymentEnvironment.Test));

        Because of = () =>
            Response = SUT.Parse("TOKEN=EC%2d5X481106MU8743929&TIMESTAMP=2012%2d02%2d26T15%3a03%3a36Z&CORRELATIONID=efc2d0ff5a121&ACK=Success&VERSION=78&BUILD=2571254");

        static SetExpressCheckoutResponseParser SUT;
        static PayPalExpressCheckoutToken Response;
    }

    [Subject(typeof(SetExpressCheckoutResponseParser))]
    public class When_set_express_checkout_response_failure
    {
        It should_have_payment_status_of_failed = () =>
            Response.Status.ShouldEqual(PaymentStatus.Failed);

        It should_be_a_system_failure = () =>
            Response.IsSystemFailure.ShouldBeTrue();

        It should_provide_a_failure_message = () =>
            Response.FailureMessage.ShouldNotBeEmpty();

        It should_not_provide_a_redirect_url = () =>
            Response.RedirectUrl.ShouldBeNull();

        Establish context = () =>
            SUT = new SetExpressCheckoutResponseParser(new PayPalConfiguration(PaymentEnvironment.Test));

        Because of = () =>
            Response = SUT.Parse("TIMESTAMP=2012%2d02%2d26T16%3a13%3a12Z&CORRELATIONID=ff25e376319b6&ACK=Failure&VERSION=78&BUILD=2571254&L_ERRORCODE0=10002&L_SHORTMESSAGE0=Security%20error&L_LONGMESSAGE0=Security%20header%20is%20not%20valid&L_SEVERITYCODE0=Error");

        static SetExpressCheckoutResponseParser SUT;
        static PayPalExpressCheckoutToken Response;
    }
}