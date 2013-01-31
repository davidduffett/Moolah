using System.Collections.Specialized;
using System.Web;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.DataCash;

namespace Moolah.Specs.DataCash
{
    public abstract class DataCash3DSecureRefundContext : DataCashIntegrationContext
    {
        Establish context = () =>
        {
            Configuration = new DataCash3DSecureConfiguration(PaymentEnvironment.Test, MerchantId, Password, "https://www.example.com", "Products");
            ExpiryDate = ThreeDSecureExpiryDate;
            HttpRequest = An<HttpRequestBase>();
            HttpRequest.WhenToldTo(x => x.UserAgent).Return(UserAgent);
            HttpRequest.WhenToldTo(x => x.Headers).Return(
                new NameValueCollection { { "Accept", AcceptHeaders } });

            CardNumber = AuthorisedCardNumber;
            // Use MoTo expiry date to specify not 3DS enrolled
            ExpiryDate = MoToExpiryDate;

            Gateway = new DataCash3DSecureGateway(Configuration, new HttpClient(),
                new DataCash3DSecureRequestBuilder(Configuration, HttpRequest),
                new DataCash3DSecureAuthorizeRequestBuilder(Configuration),
                new DataCash3DSecureResponseParser(),
                new RefundGateway(Configuration));
            Response = Gateway.Payment(MerchantReference(), 12.99m, new CardDetails { Number = CardNumber, Cv2 = "123", ExpiryDate = ExpiryDate });
        };

        Because of = () =>
            RefundResponse = Gateway.RefundTransaction(Response.TransactionReference, RefundAmount);

        protected static string OriginalTransactionReference;
        protected static decimal RefundAmount;
        protected static IRefundTransactionResponse RefundResponse;
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
    public class DataCash_3DSecure_refund_was_successful : DataCash3DSecureRefundContext
    {
        It should_be_successful = () =>
            RefundResponse.Status.ShouldEqual(PaymentStatus.Successful);

        It should_not_be_a_system_failure = () =>
            RefundResponse.IsSystemFailure.ShouldBeFalse();

        It should_provide_a_datacash_transaction_reference = () =>
            RefundResponse.TransactionReference.ShouldNotBeEmpty();

        It should_not_have_a_failure_message = () =>
            RefundResponse.FailureMessage.ShouldBeNull();

        Establish context = () =>
            RefundAmount = 12.99m;
    }

    [Subject(typeof(DataCash3DSecureGateway), "Integration")]
    [Ignore("Integration requires DataCash MerchantId and Password to be provided")]
    public class DataCash_3DSecure_refund_was_unsuccessful : DataCashMoToRefundContext
    {
        It should_have_failed = () =>
            RefundResponse.Status.ShouldEqual(PaymentStatus.Failed);

        It should_be_a_system_failure = () =>
            RefundResponse.IsSystemFailure.ShouldBeTrue();

        It should_provide_a_datacash_transaction_reference = () =>
            RefundResponse.TransactionReference.ShouldNotBeEmpty();

        It should_have_a_failure_message = () =>
            RefundResponse.FailureMessage.ShouldNotBeEmpty();

        Establish context = () =>
            RefundAmount = 555.55m;
    }
}