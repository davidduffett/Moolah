using System.Xml.Linq;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.DataCash;

namespace Moolah.Specs.DataCash
{
    [Subject(typeof(DataCash3DSecureAuthorizeRequestBuilder))]
    public class When_building_authorize_request_xml : WithSubject<DataCash3DSecureAuthorizeRequestBuilder>
    {
        It should_contain_correct_authentication_client = () =>
            Result.XPathValue("Request/Authentication/client").ShouldEqual(The<DataCashConfiguration>().MerchantId);

        It should_contain_correct_authentication_password = () =>
            Result.XPathValue("Request/Authentication/password").ShouldEqual(The<DataCashConfiguration>().Password);

        It should_contain_historic_txn_reference = () =>
            Result.XPathValue("Request/Transaction/HistoricTxn/reference").ShouldEqual(TransactionReference);

        It should_contain_correct_method = () =>
            Result.XPathValue("Request/Transaction/HistoricTxn/method").ShouldEqual("threedsecure_authorization_request");

        It should_contain_3ds_pares = () =>
            Result.XPathValue("Request/Transaction/HistoricTxn/pares_message").ShouldEqual(PARes);

        Establish context = () =>
            Configure(new DataCashConfiguration(PaymentEnvironment.Test, "merchantId", "password"));

        Because of = () =>
            Result = Subject.Build(TransactionReference, PARes);

        static XDocument Result;
        const string TransactionReference = "4300200042810537";
        const string PARes = "uyt45t89cnwu3rhc98a4hterjklth4o8ctsrjzth4";
    }
}