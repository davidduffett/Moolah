using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.PayPal;

namespace Moolah.Specs.PayPal
{
    [Subject(typeof(PayPalExpressCheckout))]
    public class When_registering_a_recurring_profile : PayPalExpressCheckoutContext
    {
        It should_return_the_correct_result = () =>
            Result.ShouldEqual(ExpectedResult);

        Establish context = () =>
        {
            ExpectedResult = new PayPalRecurringProfileResponse();
            RequestBuilder.WhenToldTo(x => x.CreateRecurringPaymentsProfile(Profile, Token))
                .Return(HttpUtility.ParseQueryString(Request));
            ResponseParser.WhenToldTo(x => x.CreateRecurringProfile(Param<NameValueCollection>.Matches(r => r.ToString() == Response)))
                .Return(ExpectedResult);
        };

        Because of = () =>
            Result = SUT.CreateRecurringPaymentsProfile(Profile, Token);

        static IPaymentResponse Result;
        static PayPalRecurringProfileResponse ExpectedResult;
        const string Token = "tokenValue";
        static RecurringProfile Profile = new RecurringProfile();
        const string PayerId = "payerId";
    }
}
