using System.Collections.Specialized;
using System.Web;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.DataCash;

namespace Moolah.Specs.DataCash
{
    public abstract class DataCash3DSecureContext : DataCashIntegrationContext
    {
        Establish context = () =>
        {
            Configuration = new DataCash3DSecureConfiguration(PaymentEnvironment.Test, MerchantId, Password, "https://www.example.com", "Products");
            ExpiryDate = ThreeDSecureExpiryDate;
            HttpRequest = An<HttpRequestBase>();
            HttpRequest.WhenToldTo(x => x.UserAgent).Return(UserAgent);
            HttpRequest.WhenToldTo(x => x.Headers).Return(
                new NameValueCollection { { "Accept", AcceptHeaders } });
        };

        Because of = () =>
        {
            Gateway = new DataCash3DSecureGateway(Configuration, new HttpClient(), 
                new DataCash3DSecureRequestBuilder(Configuration, HttpRequest), 
                new DataCash3DSecureAuthorizeRequestBuilder(Configuration), 
                new DataCash3DSecureResponseParser());
            Response = Gateway.Payment(MerchantReference(), 12.99m, new CardDetails { Number = CardNumber, Cv2 = "123", ExpiryDate = ExpiryDate });
        };

        protected static DataCash3DSecureConfiguration Configuration;
        protected static I3DSecurePaymentGateway Gateway;
        protected static I3DSecureResponse Response;
        // Still need to mock HttpRequest
        static HttpRequestBase HttpRequest;
        const string AcceptHeaders = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
        const string UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:10.0.2) Gecko/20100101 Firefox/10.0.2";
    }

    [Subject(typeof(DataCash3DSecureGateway), "Integration")]
    [Ignore("Integration requires DataCash MerchantId and Password to be provided")]
    public class DataCash_3DSecure_not_enrolled_payment_authorised : DataCash3DSecureContext
    {
        It should_be_successful = () =>
            Response.Status.ShouldEqual(PaymentStatus.Successful);

        It should_not_be_a_system_failure = () =>
            Response.IsSystemFailure.ShouldBeFalse();

        It should_provide_a_datacash_transaction_reference = () =>
            Response.TransactionReference.ShouldNotBeEmpty();

        It should_not_have_a_failure_message = () =>
            Response.FailureMessage.ShouldBeNull();

        It should_not_require_3ds_payer_verification = () =>
            Response.Requires3DSecurePayerVerification.ShouldBeFalse();

        Establish context = () =>
        {
            CardNumber = AuthorisedCardNumber;
            // Use MoTo expiry date to specify not 3DS enrolled
            ExpiryDate = MoToExpiryDate;
        };
    }

    [Subject(typeof(DataCash3DSecureGateway), "Integration")]
    [Ignore("Integration requires DataCash MerchantId and Password to be provided")]
    public class DataCash_3DSecure_enrolled_requires_pares : DataCash3DSecureContext
    {
        It should_be_pending = () =>
            Response.Status.ShouldEqual(PaymentStatus.Pending);

        It should_not_be_a_system_failure = () =>
            Response.IsSystemFailure.ShouldBeFalse();

        It should_provide_a_datacash_transaction_reference = () =>
            Response.TransactionReference.ShouldNotBeEmpty();

        It should_not_have_a_failure_message = () =>
            Response.FailureMessage.ShouldBeNull();

        It should_require_3ds_payer_verification = () =>
            Response.Requires3DSecurePayerVerification.ShouldBeTrue();

        It should_provide_acs_url = () =>
            Response.ACSUrl.ShouldNotBeEmpty();

        It should_provide_pareq = () =>
            Response.PAReq.ShouldNotBeEmpty();

        Establish context = () =>
        {
            CardNumber = AuthorisedCardNumber;
            ExpiryDate = ThreeDSecureExpiryDate;
        };
    }

    [Subject(typeof(DataCash3DSecureGateway), "Integration")]
    [Ignore("Integration requires DataCash MerchantId and Password to be provided")]
    public class DataCash_3DSecure_payment_not_authorised : DataCash3DSecureContext
    {
        It should_have_failed = () =>
            Response.Status.ShouldEqual(PaymentStatus.Failed);

        It should_not_be_a_system_failure = () =>
            Response.IsSystemFailure.ShouldBeFalse();

        It should_provide_a_datacash_transaction_reference = () =>
            Response.TransactionReference.ShouldNotBeEmpty();

        It should_have_the_expected_failure_message = () =>
            Response.FailureMessage.ShouldEqual("Transaction was declined by your bank. Please check your details or contact your bank and try again.");

        It should_not_require_3ds_payer_verification = () =>
            Response.Requires3DSecurePayerVerification.ShouldBeFalse();

        Establish context = () =>
        {
            CardNumber = DeclinedCardNumber;
            ExpiryDate = MoToExpiryDate;
        };
    }

    [Subject(typeof(DataCash3DSecureGateway), "Integration")]
    [Ignore("Integration requires DataCash MerchantId and Password to be provided")]
    public class DataCash_3DSecure_payment_system_failure : DataCash3DSecureContext
    {
        It should_have_failed = () =>
            Response.Status.ShouldEqual(PaymentStatus.Failed);

        It should_be_a_system_failure = () =>
            Response.IsSystemFailure.ShouldBeTrue();

        It should_not_provide_a_datacash_transaction_reference = () =>
            Response.TransactionReference.ShouldBeNull();

        It should_have_a_failure_message = () =>
            Response.FailureMessage.ShouldEqual("The vTID or password were incorrect");

        It should_not_require_3ds_payer_verification = () =>
            Response.Requires3DSecurePayerVerification.ShouldBeFalse();

        Establish context = () =>
        {
            // Use incorrect vTID username/password
            Configuration = new DataCash3DSecureConfiguration(PaymentEnvironment.Test, "bad_id", "bad_password", "https://www.example.com", "Products");
            CardNumber = AuthorisedCardNumber;
            ExpiryDate = MoToExpiryDate;
        };
    }
}
