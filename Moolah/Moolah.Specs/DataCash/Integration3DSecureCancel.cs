using System.Collections.Specialized;
using System.Web;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.DataCash;

namespace Moolah.Specs.DataCash
{
    [Subject(typeof(DataCash3DSecureGateway), "Integration")]
    [Ignore("Integration requires DataCash MerchantId and Password to be provided")]
    public class When_cancelling_a_3d_secure_transaction : DataCashIntegrationContext
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
            Configure(new DataCash3DSecureConfiguration(PaymentEnvironment.Test, MerchantId, Password, "https://www.example.com", "Products"));
            The<HttpRequestBase>().WhenToldTo(x => x.UserAgent).Return(UserAgent);
            The<HttpRequestBase>().WhenToldTo(x => x.Headers).Return(
                new NameValueCollection { { "Accept", AcceptHeaders } });

            Gateway = new DataCash3DSecureGateway(The<DataCash3DSecureConfiguration>(), new HttpClient(),
                new DataCash3DSecureRequestBuilder(The<DataCash3DSecureConfiguration>(), The<HttpRequestBase>()),
                new DataCash3DSecureAuthorizeRequestBuilder(The<DataCash3DSecureConfiguration>()),
                new DataCash3DSecureResponseParser(),
                new RefundGateway(The<DataCash3DSecureConfiguration>()),
                new CancelGateway(The<DataCash3DSecureConfiguration>()));
            PaymentResponse = Gateway.Payment(MerchantReference(), 12.99m, new CardDetails { Number = AuthorisedCardNumber, Cv2 = "123", ExpiryDate = MoToExpiryDate });
        };

        Because of = () =>
            CancelResponse = Gateway.CancelTransaction(PaymentResponse.TransactionReference);

        static I3DSecureResponse PaymentResponse;
        static ICancelTransactionResponse CancelResponse;
        static I3DSecurePaymentGateway Gateway;
    }
}