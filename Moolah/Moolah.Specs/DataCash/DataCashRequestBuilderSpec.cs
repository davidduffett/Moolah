using System.Xml.Linq;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.DataCash;

namespace Moolah.Specs.DataCash
{
    [Subject(typeof(DataCashRequestBuilder))]
    public class When_building_auth_request_xml : WithFakes
    {
        It should_contain_correct_authentication_client = () =>
            result.XPathValue("Request/Authentication/client").ShouldEqual(config.MerchantId);

        It should_contain_correct_authentication_password = () =>
            result.XPathValue("Request/Authentication/password").ShouldEqual(config.Password);

        It should_contain_correct_transaction_merchant_reference = () =>
            result.XPathValue("Request/Transaction/TxnDetails/merchantreference").ShouldEqual(MerchantReference);

        It should_contain_correct_transaction_currency = () =>
            result.XPathValue("Request/Transaction/TxnDetails/amount/@currency").ShouldEqual("GBP");

        It should_contain_correct_transaction_amount = () =>
            result.XPathValue("Request/Transaction/TxnDetails/amount").ShouldEqual(string.Format("{0:0.00}", Amount));

        It should_contain_correct_card_pan = () =>
            result.XPathValue("Request/Transaction/CardTxn/Card/pan").ShouldEqual(card.Number);

        It should_contain_correct_card_expiry_date = () =>
            result.XPathValue("Request/Transaction/CardTxn/Card/expirydate").ShouldEqual(card.ExpiryDate);

        It should_contain_correct_card_start_date = () =>
            result.XPathValue("Request/Transaction/CardTxn/Card/startdate").ShouldEqual(card.StartDate);

        It should_contain_correct_card_issue_number = () =>
            result.XPathValue("Request/Transaction/CardTxn/Card/issuenumber").ShouldEqual(card.IssueNumber);

        It should_contain_correct_card_cv2 = () =>
            result.XPathValue("Request/Transaction/CardTxn/Card/Cv2Avs/cv2").ShouldEqual(card.Cv2);

        It should_contain_correct_method = () =>
            result.XPathValue("Request/Transaction/CardTxn/method").ShouldEqual("auth");

        Establish context = () =>
            SUT = new DataCashRequestBuilder(config);

        Because of = () =>
            result = SUT.Build(MerchantReference, Amount, card);

        static DataCashRequestBuilder SUT;
        static XDocument result;
        static readonly DataCashConfiguration config = new DataCashConfiguration(PaymentEnvironment.Test, "merchant", "password123");
        const string MerchantReference = "123456";
        const decimal Amount = 12.99m;
        static readonly CardDetails card = new CardDetails
                                                {
                                                    Number = "1234567890123456",
                                                    ExpiryDate = "10/12",
                                                    Cv2 = "123",
                                                    StartDate = "10/10",
                                                    IssueNumber = "123"
                                                };
    }
}