using System.Xml.Linq;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.DataCash;

namespace Moolah.Specs.DataCash
{
    [Behaviors]
    public class DataCashPaymentRequestBehavior
    {
        It should_contain_correct_authentication_client = () =>
            Result.XPathValue("Request/Authentication/client").ShouldEqual(Configuration.MerchantId);

        It should_contain_correct_authentication_password = () =>
            Result.XPathValue("Request/Authentication/password").ShouldEqual(Configuration.Password);

        It should_contain_correct_transaction_merchant_reference = () =>
            Result.XPathValue("Request/Transaction/TxnDetails/merchantreference").ShouldEqual(MerchantReference);

        It should_contain_correct_transaction_currency = () =>
            Result.XPathValue("Request/Transaction/TxnDetails/amount/@currency").ShouldEqual("GBP");

        It should_contain_correct_transaction_amount = () =>
            Result.XPathValue("Request/Transaction/TxnDetails/amount").ShouldEqual(string.Format("{0:0.00}", Amount));

        It should_contain_correct_card_pan = () =>
            Result.XPathValue("Request/Transaction/CardTxn/Card/pan").ShouldEqual(CardDetails.Number);

        It should_contain_correct_card_expiry_date = () =>
            Result.XPathValue("Request/Transaction/CardTxn/Card/expirydate").ShouldEqual(CardDetails.ExpiryDate);

        It should_contain_correct_card_start_date = () =>
            Result.XPathValue("Request/Transaction/CardTxn/Card/startdate").ShouldEqual(CardDetails.StartDate);

        It should_contain_correct_card_issue_number = () =>
            Result.XPathValue("Request/Transaction/CardTxn/Card/issuenumber").ShouldEqual(CardDetails.IssueNumber);

        It should_contain_correct_card_cv2 = () =>
            Result.XPathValue("Request/Transaction/CardTxn/Card/Cv2Avs/cv2").ShouldEqual(CardDetails.Cv2);

        It should_contain_correct_method = () =>
            Result.XPathValue("Request/Transaction/CardTxn/method").ShouldEqual("auth");

        protected static XDocument Result;
        protected static DataCashConfiguration Configuration;
        protected static string MerchantReference;
        protected static decimal Amount;
        protected static CardDetails CardDetails;
    }

    [Subject(typeof(DataCashMoToRequestBuilder))]
    public class When_building_auth_request_xml : WithFakes
    {
        Behaves_like<DataCashPaymentRequestBehavior> a_datacash_payment_request;

        Because of = () =>
        {
            var builder = new DataCashMoToRequestBuilder(Configuration);
            Result = builder.Build(MerchantReference, Amount, CardDetails);
        };

        protected static XDocument Result;
        protected static DataCashConfiguration Configuration = new DataCashConfiguration(PaymentEnvironment.Test, "merchant", "password123");
        protected static string MerchantReference = "123456";
        protected static decimal Amount = 12.99m;
        protected static CardDetails CardDetails = new CardDetails
                                                       {
                                                           Number = "1234567890123456",
                                                           ExpiryDate = "10/12",
                                                           Cv2 = "123",
                                                           StartDate = "10/10",
                                                           IssueNumber = "123"
                                                       };
    }
}