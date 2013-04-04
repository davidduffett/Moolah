using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.PayPal;

namespace Moolah.Specs.PayPal
{
    public abstract class PayPalMassPayContext : WithSubject<PayPalMassPay>
    {
        Establish context = () =>
        {
            Configuration = new PayPalConfiguration(PaymentEnvironment.Test);            
            The<IHttpClient>().WhenToldTo(x => x.Post(Param<string>.IsNotNull, Request))
                .Return(Response);
            The<IHttpClient>().WhenToldTo(x => x.Get(Configuration.Host + "?" + Request))
                .Return(Response);
            Subject = new PayPalMassPay(Configuration, The<IHttpClient>(), The<IPayPalRequestBuilder>(), The<IPayPalResponseParser>());
        };

        protected static PayPalConfiguration Configuration;
        protected const string Request = "Test=Request";
        protected const string Response = "Test=Response";
    }

    [Subject(typeof(PayPalMassPay))]
    public class When_do_mass_payment_is_called : PayPalMassPayContext
    {
        It should_return_payment_response = () =>
            Result.ShouldEqual(The<IPaymentResponse>());

        Establish context = () =>
        {
            The<IPayPalRequestBuilder>().WhenToldTo(x => x.MassPayment(Items, CurrencyCodeType.GBP, ReceiverType.EmailAddress, EmailSubject))
                .Return(HttpUtility.ParseQueryString(Request));
            The<IPayPalResponseParser>().WhenToldTo(x => x.MassPayment(Param<NameValueCollection>.Matches(r => r.ToString() == Response)))
                .Return(The<IPaymentResponse>());
        };

        Because of = () =>
            Result = Subject.DoMassPayment(Items, CurrencyCodeType.GBP, ReceiverType.EmailAddress, EmailSubject);

        static readonly IEnumerable<PayReceiver> Items = new List<PayReceiver>();
        static IPaymentResponse Result;
        const string EmailSubject = "Email subject";
    }
}