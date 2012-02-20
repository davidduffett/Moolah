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
            result.ShouldEqual(expectedResponse);

        Establish context = () =>
        {
            var requestBuilder = An<IDataCashRequestBuilder>();
            requestBuilder.WhenToldTo(x => x.Build(MerchantReference, Amount, Card))
                .Return(requestDocument);

            httpClient = An<IHttpClient>();
            const string httpResponse = "A response from DataCash";
            httpClient.WhenToldTo(x => x.Post(config.Host, requestXml))
                .Return(httpResponse);

            var responseParser = An<IDataCashResponseParser>();
            expectedResponse = An<IPaymentResponse>();
            responseParser.WhenToldTo(x => x.Parse(httpResponse))
                .Return(expectedResponse);

            SUT = new DataCashMoToGateway(config, httpClient, requestBuilder, responseParser);
        };

        Because of = () =>
            result = SUT.Payment(MerchantReference, Amount, Card);

        static DataCashMoToGateway SUT;
        static IPaymentResponse result;
        static IHttpClient httpClient;
        static readonly DataCashConfiguration config = new DataCashConfiguration(PaymentEnvironment.Test, "merchant", "password");
        static readonly XDocument requestDocument = new XDocument(new XDeclaration("1.0", "utf-8", null), new XElement("Request"));
        static readonly string requestXml = requestDocument.ToString(SaveOptions.DisableFormatting);
        static IPaymentResponse expectedResponse;
        const string MerchantReference = "1234";
        const decimal Amount = 12.00m;
        static readonly CardDetails Card;
    }
}