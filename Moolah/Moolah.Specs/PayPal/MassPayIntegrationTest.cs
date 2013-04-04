using Machine.Fakes;
using Machine.Specifications;
using Moolah.PayPal;

namespace Moolah.Specs.PayPal
{
    [Subject(typeof(PayPalMassPay))]
    [Ignore("Payer account needs to have enough funds first.")]
    public class When_doing_mass_payment_by_email : WithSubject<PayPalMassPay>
    {
        It should_get_a_successful_response = () =>
            Response.Status.ShouldEqual(PaymentStatus.Successful);

        It should_not_specify_a_failure_message = () =>
            Response.FailureMessage.ShouldBeNull();

        Because of = () =>
        {
            Response = Subject.DoMassPayment(
                new[] 
                {
                    new PayReceiver("buyer1@gmail.com", 10, "12345", "This is a test payment"),
                    new PayReceiver("buyer2@gmail.com", 5, "12346", "This is a test payment")
                },
                CurrencyCodeType.GBP,
                emailSubject: "Test payment"
            );
        };

        Establish context = () =>            
            Subject = new PayPalMassPay(new PayPalConfiguration(PaymentEnvironment.Test));

        protected static IPaymentResponse Response;
    }
}