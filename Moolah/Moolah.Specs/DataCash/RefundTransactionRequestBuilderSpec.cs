using System.Xml.Linq;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.DataCash;

namespace Moolah.Specs.DataCash
{
    [Subject(typeof(RefundTransactionRequestBuilder))]
    public class When_building_refund_transaction_request : WithSubject<RefundTransactionRequestBuilder>
    {
        It should_contain_correct_authentication_client = () =>
            Result.XPathValue("Request/Authentication/client").ShouldEqual(The<DataCashConfiguration>().MerchantId);

        It should_contain_correct_authentication_password = () =>
            Result.XPathValue("Request/Authentication/password").ShouldEqual(The<DataCashConfiguration>().Password);

        It should_contain_historic_txn_reference = () =>
            Result.XPathValue("Request/Transaction/HistoricTxn/reference").ShouldEqual(OriginalTransactionReference);

        It should_contain_correct_method = () =>
            Result.XPathValue("Request/Transaction/HistoricTxn/method").ShouldEqual("txn_refund");

        It should_contain_the_amount = () =>
            Result.XPathValue("Request/Transaction/TxnDetails/amount").ShouldEqual(Amount.ToString("0.00"));

        Establish context = () =>
            Configure(new DataCashConfiguration(PaymentEnvironment.Test, "merchantId", "password"));

        Because of = () =>
            Result = Subject.Build(OriginalTransactionReference, Amount);

        static XDocument Result;
        const string OriginalTransactionReference = "originalTxn";
        const decimal Amount = 12.99m;
    }
}