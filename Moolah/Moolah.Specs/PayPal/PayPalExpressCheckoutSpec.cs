using Machine.Fakes;
using Machine.Specifications;
using Moolah.PayPal;

namespace Moolah.Specs.PayPal
{
    [Subject(typeof(PayPalExpressCheckout))]
    public class When_set_express_checkout_is_successful : WithFakes
    {
        It should_return_the_correct_response = () =>
            Response.ShouldEqual(ExpectedResponse);

        Establish context = () =>
        {
            var httpClient = An<IHttpClient>();
            httpClient.WhenToldTo(x => x.Get(RequestUrl))
                .Return(PayPalResponse);
            ExpectedResponse = new PayPalExpressCheckoutToken(PayPalResponse);
            var parser = An<ISetExpressCheckoutResponseParser>();
            parser.WhenToldTo(x => x.Parse(PayPalResponse))
                .Return(ExpectedResponse);
            SUT = new PayPalExpressCheckout(new PayPalConfiguration(PaymentEnvironment.Test), httpClient, parser);
        };

        Because of = () =>
            Response = SUT.SetExpressCheckout(Amount, CancelUrl, ConfirmationUrl);

        static PayPalExpressCheckout SUT;
        static PayPalExpressCheckoutToken Response;
        static PayPalExpressCheckoutToken ExpectedResponse;
        const decimal Amount = 10m;
        const string CancelUrl = "http://www.yourdomain.com/cancel.html";
        const string ConfirmationUrl = "http://www.yourdomain.com/success.html";
        const string RequestUrl = "https://api-3t.sandbox.paypal.com/nvp?VERSION=78&USER=sdk-three_api1.sdk.com&PWD=QFZCWN5HZM8VBG7&SIGNATURE=A-IzJhZZjhg29XQ2qnhapuwxIDzyAZQ92FRP5dqBzVesOkzbdUONzmOU&METHOD=SetExpressCheckout&PAYMENTREQUEST_0_AMT=10.00&PAYMENTREQUEST_0_CURRENCYCODE=GBP&PAYMENTREQUEST_0_PAYMENTACTION=Sale&cancelUrl=http%3a%2f%2fwww.yourdomain.com%2fcancel.html&returnUrl=http%3a%2f%2fwww.yourdomain.com%2fsuccess.html";
        const string PayPalResponse = "Sample PayPal Response";
    }
}