using System.Collections.Specialized;
using System.Web;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.DataCash;

namespace Moolah.Specs.DataCash
{
    [Subject(typeof(DataCashMoToGateway), "Integration")]
    [Ignore("Integration requires DataCash MerchantId and Password to be provided")]
    public class When_cancelling_a_moto_transaction : DataCashIntegrationContext
    {
        It should_be_successful = () =>
            CancelResponse.Status.ShouldEqual(PaymentStatus.Successful);

        It should_not_be_a_system_failure = () =>
            CancelResponse.IsSystemFailure.ShouldBeFalse();

        It should_provide_a_datacash_transaction_reference = () =>
            CancelResponse.TransactionReference.ShouldNotBeEmpty();

        It should_not_have_a_failure_message = () =>
            CancelResponse.FailureMessage.ShouldBeNull();

        Establish context = () =>
        {
            Configure(new DataCashConfiguration(PaymentEnvironment.Test, MerchantId, Password));
            The<HttpRequestBase>().WhenToldTo(x => x.UserAgent).Return(UserAgent);
            The<HttpRequestBase>().WhenToldTo(x => x.Headers).Return(
                new NameValueCollection { { "Accept", AcceptHeaders } });

            Gateway = new DataCashMoToGateway(The<DataCashConfiguration>(), new HttpClient(),
                new DataCashMoToRequestBuilder(The<DataCashConfiguration>()),
                new DataCashResponseParser(),
                new RefundGateway(The<DataCashConfiguration>()),
                new CancelGateway(The<DataCashConfiguration>()));
            PaymentResponse = Gateway.Payment(MerchantReference(), 12.99m, new CardDetails { Number = AuthorisedCardNumber, Cv2 = "123", ExpiryDate = MoToExpiryDate });
        };

        Because of = () =>
            CancelResponse = Gateway.CancelTransaction(PaymentResponse.TransactionReference);

        static IPaymentResponse PaymentResponse;
        static ICancelTransactionResponse CancelResponse;
        static IPaymentGateway Gateway;
    }
}