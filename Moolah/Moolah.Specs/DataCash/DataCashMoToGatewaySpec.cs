using System.Xml.Linq;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.DataCash;

namespace Moolah.Specs.DataCash
{
    [Subject(typeof(DataCashMoToGateway))]
    public class When_submitting_datacash_payment : WithFakes
    {
        It should_build_request_parse_and_return_response = () =>
            Response.ShouldEqual(ExpectedResponse);

        Establish context = () =>
        {
            Configuration = new DataCashConfiguration(PaymentEnvironment.Test, "merchantId", "password");
            PaymentRequestBuilder = An<IDataCashPaymentRequestBuilder>();
            HttpClient = An<IHttpClient>();
            ResponseParser = An<IDataCashResponseParser>();
            ExpectedResponse = An<ICardPaymentResponse>();

            var requestDoc = new XDocument();
            const string httpResponse = "<PaymentResponse/>";
            PaymentRequestBuilder.WhenToldTo(x => x.Build(MerchantReference, Amount, Card))
                .Return(requestDoc);
            HttpClient.WhenToldTo(x => x.Post(Configuration.Host, requestDoc.ToString(SaveOptions.DisableFormatting)))
                .Return(httpResponse);
            ResponseParser.WhenToldTo(x => x.Parse(httpResponse))
                .Return(ExpectedResponse);
        };

        Because of = () =>
        {
            SUT = new DataCashMoToGateway(Configuration, HttpClient, PaymentRequestBuilder, ResponseParser);
            Response = SUT.Payment(MerchantReference, Amount, Card);
        };

        protected static DataCashMoToGateway SUT;
        protected static DataCashConfiguration Configuration;
        protected static IDataCashPaymentRequestBuilder PaymentRequestBuilder;
        protected static IHttpClient HttpClient;
        protected static IDataCashResponseParser ResponseParser;
        protected static ICardPaymentResponse ExpectedResponse;
        protected static IPaymentResponse Response;
        const string MerchantReference = "987654321";
        const decimal Amount = 12.99m;
        static CardDetails Card;
    }
}