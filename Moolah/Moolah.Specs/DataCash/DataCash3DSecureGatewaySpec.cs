using System.Xml.Linq;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.DataCash;

namespace Moolah.Specs.DataCash
{
    [Subject(typeof(DataCash3DSecureGateway))]
    public class When_submitting_datacash_3ds_pares_required_payment : WithSubject<DataCash3DSecureGateway>
    {
        It should_build_request_parse_and_return_payment_response = () =>
            Response.ShouldEqual(The<I3DSecureResponse>());

        It should_offer_access_to_the_merchant_id = () => 
            Subject.MerchantId.ShouldEqual("merchantId");

        Establish context = () =>
        {
            Configure(new DataCash3DSecureConfiguration(PaymentEnvironment.Test, "merchantId", "password", "https://www.example.com", "Products"));
            The<I3DSecureResponse>().WhenToldTo(x => x.Status).Return(PaymentStatus.Pending);
            The<I3DSecureResponse>().WhenToldTo(x => x.Requires3DSecurePayerVerification).Return(true);

            var requestDoc = new XDocument();
            const string httpResponse = "<PaymentResponse/>";
            The<IDataCashPaymentRequestBuilder>().WhenToldTo(x => x.Build(MerchantReference, Amount, Currency, Card, null))
                .Return(requestDoc);
            The<IHttpClient>().WhenToldTo(x => x.Post(The<DataCash3DSecureConfiguration>().Host, requestDoc.ToString(SaveOptions.DisableFormatting)))
                .Return(httpResponse);
            The<IDataCash3DSecureResponseParser>().WhenToldTo(x => x.Parse(httpResponse))
                .Return(The<I3DSecureResponse>());
        };

        Because of = () =>
            Response = Subject.Payment(MerchantReference, Amount, Card, null, Currency);

        static I3DSecureResponse Response;
        static CardDetails Card = new CardDetails();
        const string MerchantReference = "987654321";
        const decimal Amount = 12.99m;
        const string Currency = "GBP";
    }

    [Subject(typeof(DataCash3DSecureGateway))]
    public class When_authorizing_a_pending_3ds_transaction : WithSubject<DataCash3DSecureGateway>
    {
        It should_build_request_parse_and_return_authorize_response = () =>
            Response.ShouldEqual(The<I3DSecureResponse>());

        Establish context = () =>
        {
            Configure(new DataCash3DSecureConfiguration(PaymentEnvironment.Test, "merchantId", "password", "https://www.example.com", "Products"));
            var requestDoc = new XDocument();
            const string httpResponse = "<AuthorizeResponse/>";
            The<IDataCashAuthorizeRequestBuilder>().WhenToldTo(x => x.Build(TransactionReference, PARes))
                .Return(requestDoc);
            The<IHttpClient>().WhenToldTo(x => x.Post(The<DataCash3DSecureConfiguration>().Host, requestDoc.ToString(SaveOptions.DisableFormatting)))
                .Return(httpResponse);
            The<IDataCash3DSecureResponseParser>().WhenToldTo(x => x.Parse(httpResponse))
                .Return(The<I3DSecureResponse>());
        };

        Because of = () =>
            Response = Subject.Authorise(TransactionReference, PARes);

        static I3DSecureResponse Response; 
        const string TransactionReference = "987645";
        const string PARes = "blahdeblah";
    }

    [Subject(typeof(DataCash3DSecureGateway))]
    public class When_submitting_datacash_3ds_payment_and_immediate_authorization_is_possible : WithSubject<DataCash3DSecureGateway>
    {
        It should_authorize_the_payment = () =>
            Response.ShouldEqual(The<I3DSecureResponse>());

        Establish context = () =>
        {
            Configure(new DataCash3DSecureConfiguration(PaymentEnvironment.Test, "merchantId", "password", "https://www.example.com", "Products"));
            Card = new CardDetails();

            // Payment
            var paymentRequest = new XDocument();
            const string paymentHttpResponse = "<PaymentResponse/>";
            const string transactionReference = "123456";

            var paymentResponse = An<I3DSecureResponse>();
            paymentResponse.WhenToldTo(x => x.Status).Return(PaymentStatus.Pending);
            paymentResponse.WhenToldTo(x => x.Requires3DSecurePayerVerification).Return(false);
            paymentResponse.WhenToldTo(x => x.TransactionReference).Return(transactionReference);

            The<IDataCashPaymentRequestBuilder>().WhenToldTo(x => x.Build(MerchantReference, Amount, Currency, Card, null)).Return(paymentRequest);
            The<IHttpClient>().WhenToldTo(x => x.Post(The<DataCash3DSecureConfiguration>().Host, paymentRequest.ToString(SaveOptions.DisableFormatting))).Return(paymentHttpResponse);
            The<IDataCash3DSecureResponseParser>().WhenToldTo(x => x.Parse(paymentHttpResponse)).Return(paymentResponse);

            // Authorize
            var authorizeRequest = new XDocument(new XDeclaration("1.0", "utf-8", null), new XElement("Authorize"));
            const string authorizeHttpResponse = "<Authorized/>";

            The<IDataCashAuthorizeRequestBuilder>().WhenToldTo(x => x.Build(transactionReference, null)).Return(authorizeRequest);
            The<IHttpClient>().WhenToldTo(x => x.Post(The<DataCash3DSecureConfiguration>().Host, authorizeRequest.ToString(SaveOptions.DisableFormatting))).Return(authorizeHttpResponse);
            The<IDataCash3DSecureResponseParser>().WhenToldTo(x => x.Parse(authorizeHttpResponse)).Return(The<I3DSecureResponse>());
        };

        Because of = () =>
            Response = Subject.Payment(MerchantReference, Amount, Card, null, Currency);

        static I3DSecureResponse Response; 
        const string MerchantReference = "987654321";
        const decimal Amount = 12.99m;
        static CardDetails Card;
        const string Currency = "GBP";
    }

    [Subject(typeof(DataCash3DSecureGateway))]
    public class When_submitting_3d_secure_gateway_transaction_refund : WithSubject<DataCash3DSecureGateway>
    {
        It should_return_response_from_refund_gateway = () =>
            RefundResponse.ShouldEqual(ExpectedRefundResponse);

        Establish context = () =>
        {
            Configure(new DataCash3DSecureConfiguration(PaymentEnvironment.Test, "merchantId", "password", "https://www.example.com", "Products"));
            ExpectedRefundResponse = An<IRefundTransactionResponse>();
            The<IRefundGateway>().WhenToldTo(x => x.Refund(OriginalTransactionReference, Amount))
                .Return(ExpectedRefundResponse);
        };

        Because of = () =>
            RefundResponse = Subject.RefundTransaction(OriginalTransactionReference, Amount);
        
        static IRefundTransactionResponse RefundResponse;
        static IRefundTransactionResponse ExpectedRefundResponse;
        const string OriginalTransactionReference = "987654321";
        const decimal Amount = 12.99m;
    }
}
