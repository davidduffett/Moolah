using System.Xml.Linq;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.DataCash;

namespace Moolah.Specs.DataCash
{
    [Subject(typeof(DataCashMoToGateway))]
    public class When_submitting_datacash_payment : WithSubject<DataCashMoToGateway>
    {
        It should_build_request_parse_and_return_response = () =>
            Response.ShouldEqual(ExpectedResponse);

        Establish context = () =>
        {
            Configure(new DataCashConfiguration(PaymentEnvironment.Test, "merchantId", "password"));
            ExpectedResponse = An<ICardPaymentResponse>();

            var requestDoc = new XDocument();
            const string httpResponse = "<PaymentResponse/>";
            The<IDataCashPaymentRequestBuilder>().WhenToldTo(x => x.Build(MerchantReference, Amount, Card))
                .Return(requestDoc);
            The<IHttpClient>().WhenToldTo(x => x.Post(The<DataCashConfiguration>().Host, requestDoc.ToString(SaveOptions.DisableFormatting)))
                .Return(httpResponse);
            The<IDataCashResponseParser>().WhenToldTo(x => x.Parse(httpResponse))
                .Return(ExpectedResponse);
        };

        Because of = () =>
            Response = Subject.Payment(MerchantReference, Amount, Card);

        static ICardPaymentResponse ExpectedResponse;
        static ICardPaymentResponse Response;
        const string MerchantReference = "987654321";
        const decimal Amount = 12.99m;
        static readonly CardDetails Card;
    }

    [Subject(typeof(DataCashMoToGateway))]
    public class When_submitting_transaction_refund : WithSubject<DataCashMoToGateway>
    {
        It should_build_request_parse_and_return_response = () =>
            Response.ShouldEqual(ExpectedResponse);

        Establish context = () =>
        {
            Configure(new DataCashConfiguration(PaymentEnvironment.Test, "merchantId", "password"));
            ExpectedResponse = An<IRefundTransactionResponse>();

            var requestDoc = new XDocument();
            const string httpResponse = "<RefundResponse/>";
            The<IDataCashRefundTransactionRequestBuilder>().WhenToldTo(x => x.Build(OriginalTransactionReference, Amount))
                .Return(requestDoc);
            The<IHttpClient>().WhenToldTo(x => x.Post(The<DataCashConfiguration>().Host, requestDoc.ToString(SaveOptions.DisableFormatting)))
                .Return(httpResponse);
            The<IRefundTransactionResponseParser>().WhenToldTo(x => x.Parse(httpResponse))
                .Return(ExpectedResponse);
        };

        Because of = () =>
            Response = Subject.RefundTransaction(OriginalTransactionReference, Amount);

        static IRefundTransactionResponse ExpectedResponse;
        static IRefundTransactionResponse Response;
        const string OriginalTransactionReference = "originalTxn";
        const decimal Amount = 12.99m;
    }
}