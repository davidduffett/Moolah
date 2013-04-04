using System.Web;
using Machine.Specifications;
using Moolah.PayPal;

namespace Moolah.Specs.PayPal
{
    [Subject(typeof(PayPalResponseParser))]
    public class When_parsing_mass_payment_transaction_when_it_succeeded : PayPalResponseParserContext
    {
        It should_have_successful_payment_status = () =>
            Response.Status.ShouldEqual(PaymentStatus.Successful);

        Because of = () =>
        {
            var payPalResponse = HttpUtility.ParseQueryString("ACK=Success");
            Response = SUT.MassPayment(payPalResponse);
        };

        static IPaymentResponse Response;
    }

    [Subject(typeof(PayPalResponseParser))]
    public class When_parsing_mass_payment_transaction_when_it_failed : PayPalResponseParserContext
    {
        It should_have_failed_payment_status = () =>
            Response.Status.ShouldEqual(PaymentStatus.Failed);

        It should_be_a_system_failure = () =>
            Response.IsSystemFailure.ShouldBeTrue();

        It should_provide_a_failure_message = () =>
            Response.FailureMessage.ShouldNotBeEmpty();

        Because of = () =>
        {
            var payPalResponse = HttpUtility.ParseQueryString("ACK=Failure&L_SHORTMESSAGE0=Security%20error&L_LONGMESSAGE0=Security%20header%20is%20not%20valid&L_SEVERITYCODE0=Error");
            Response = SUT.MassPayment(payPalResponse);
        };

        static IPaymentResponse Response;
    }
}