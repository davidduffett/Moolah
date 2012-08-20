using System.Xml.Linq;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.DataCash;

namespace Moolah.Specs.DataCash
{
    public abstract class DataCash3DSecureGatewayContext : WithFakes
    {
        Establish context = () =>
        {
            Configuration = new DataCash3DSecureConfiguration(PaymentEnvironment.Test, "merchantId", "password",
                "https://www.example.com", "Products");
            PaymentRequestBuilder = An<IDataCashPaymentRequestBuilder>();
            HttpClient = An<IHttpClient>();
            AuthorizeRequestBuilder = An<IDataCashAuthorizeRequestBuilder>();
            ResponseParser = An<IDataCash3DSecureResponseParser>();
            ExpectedResponse = An<I3DSecureResponse>();
        };

        protected static DataCash3DSecureGateway SUT;
        protected static DataCash3DSecureConfiguration Configuration;
        protected static IDataCashPaymentRequestBuilder PaymentRequestBuilder;
        protected static IDataCashAuthorizeRequestBuilder AuthorizeRequestBuilder;
        protected static IHttpClient HttpClient;
        protected static IDataCash3DSecureResponseParser ResponseParser;
        protected static I3DSecureResponse ExpectedResponse;
        protected static I3DSecureResponse Response;
    }

    [Subject(typeof(DataCash3DSecureGateway))]
    public class When_submitting_datacash_3ds_pares_required_payment : DataCash3DSecureGatewayContext
    {
        It should_build_request_parse_and_return_payment_response = () =>
            Response.ShouldEqual(ExpectedResponse);

        It should_offer_access_to_the_merchant_id = () => 
           SUT.MerchantId.ShouldEqual("merchantId");

        Establish context = () =>
        {
            Card = new CardDetails();
            var requestDoc = new XDocument();
            const string httpResponse = "<PaymentResponse/>";
            ExpectedResponse.WhenToldTo(x => x.Status).Return(PaymentStatus.Pending);
            ExpectedResponse.WhenToldTo(x => x.Requires3DSecurePayerVerification).Return(true);

            PaymentRequestBuilder.WhenToldTo(x => x.Build(MerchantReference, Amount, Card))
                .Return(requestDoc);
            HttpClient.WhenToldTo(x => x.Post(Configuration.Host, requestDoc.ToString(SaveOptions.DisableFormatting)))
                .Return(httpResponse);
            ResponseParser.WhenToldTo(x => x.Parse(httpResponse))
                .Return(ExpectedResponse);
        };

        Because of = () =>
        {
            SUT = new DataCash3DSecureGateway(Configuration, HttpClient, PaymentRequestBuilder, AuthorizeRequestBuilder, ResponseParser);
            Response = SUT.Payment(MerchantReference, Amount, Card);
        };

        const string MerchantReference = "987654321";
        const decimal Amount = 12.99m;
        static CardDetails Card;
    }

    [Subject(typeof(DataCash3DSecureGateway))]
    public class When_authorizing_a_pending_3ds_transaction : DataCash3DSecureGatewayContext
    {
        It should_build_request_parse_and_return_authorize_response = () =>
            Response.ShouldEqual(ExpectedResponse);

        Establish context = () =>
        {
            var requestDoc = new XDocument();
            const string httpResponse = "<AuthorizeResponse/>";
            AuthorizeRequestBuilder.WhenToldTo(x => x.Build(TransactionReference, PARes))
                .Return(requestDoc);
            HttpClient.WhenToldTo(x => x.Post(Configuration.Host, requestDoc.ToString(SaveOptions.DisableFormatting)))
                .Return(httpResponse);
            ResponseParser.WhenToldTo(x => x.Parse(httpResponse))
                .Return(ExpectedResponse);
        };

        Because of = () =>
        {
            SUT = new DataCash3DSecureGateway(Configuration, HttpClient, PaymentRequestBuilder, AuthorizeRequestBuilder, ResponseParser);
            Response = SUT.Authorise(TransactionReference, PARes);
        };

        const string TransactionReference = "987645";
        const string PARes = "blahdeblah";
    }

    [Subject(typeof(DataCash3DSecureGateway))]
    public class When_submitting_datacash_3ds_payment_and_immediate_authorization_is_possible : DataCash3DSecureGatewayContext
    {
        It should_authorize_the_payment = () =>
            Response.ShouldEqual(ExpectedResponse);

        Establish context = () =>
        {
            Card = new CardDetails();

            // Payment
            var paymentRequest = new XDocument();
            const string paymentHttpResponse = "<PaymentResponse/>";
            const string transactionReference = "123456";

            var paymentResponse = An<I3DSecureResponse>();
            paymentResponse.WhenToldTo(x => x.Status).Return(PaymentStatus.Pending);
            paymentResponse.WhenToldTo(x => x.Requires3DSecurePayerVerification).Return(false);
            paymentResponse.WhenToldTo(x => x.TransactionReference).Return(transactionReference);

            PaymentRequestBuilder.WhenToldTo(x => x.Build(MerchantReference, Amount, Card)).Return(paymentRequest);
            HttpClient.WhenToldTo(x => x.Post(Configuration.Host, paymentRequest.ToString(SaveOptions.DisableFormatting))).Return(paymentHttpResponse);
            ResponseParser.WhenToldTo(x => x.Parse(paymentHttpResponse)).Return(paymentResponse);

            // Authorize
            var authorizeRequest = new XDocument(new XDeclaration("1.0", "utf-8", null), new XElement("Authorize"));
            const string authorizeHttpResponse = "<Authorized/>";

            AuthorizeRequestBuilder.WhenToldTo(x => x.Build(transactionReference, null)).Return(authorizeRequest);
            HttpClient.WhenToldTo(x => x.Post(Configuration.Host, authorizeRequest.ToString(SaveOptions.DisableFormatting))).Return(authorizeHttpResponse);
            ResponseParser.WhenToldTo(x => x.Parse(authorizeHttpResponse)).Return(ExpectedResponse);
        };

        Because of = () =>
        {
            SUT = new DataCash3DSecureGateway(Configuration, HttpClient, PaymentRequestBuilder, AuthorizeRequestBuilder, ResponseParser);
            Response = SUT.Payment(MerchantReference, Amount, Card);
        };

        const string MerchantReference = "987654321";
        const decimal Amount = 12.99m;
        static CardDetails Card;
    }
}
