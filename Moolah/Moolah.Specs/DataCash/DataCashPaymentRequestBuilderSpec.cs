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
            Result = builder.Build(MerchantReference, Amount, CardDetails, null);
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

    [Subject(typeof(DataCashMoToRequestBuilder))]
    public class When_building_auth_request_xml_with_billing_address : WithFakes
    {
        Behaves_like<DataCashPaymentRequestBehavior> a_datacash_payment_request;

        It should_contain_street_address_1 = () =>
            Result.XPathValue("Request/Transaction/CardTxn/Card/Cv2Avs/street_address1").ShouldEqual(BillingAddress.StreetAddress1);

        It should_contain_street_address_2 = () =>
            Result.XPathValue("Request/Transaction/CardTxn/Card/Cv2Avs/street_address2").ShouldEqual(BillingAddress.StreetAddress2);

        It should_contain_street_address_3 = () =>
            Result.XPathValue("Request/Transaction/CardTxn/Card/Cv2Avs/street_address3").ShouldEqual(BillingAddress.StreetAddress3);

        It should_contain_street_address_4 = () =>
            Result.XPathValue("Request/Transaction/CardTxn/Card/Cv2Avs/street_address4").ShouldEqual(BillingAddress.StreetAddress4);

        It should_contain_city = () =>
            Result.XPathValue("Request/Transaction/CardTxn/Card/Cv2Avs/city").ShouldEqual(BillingAddress.City);

        It should_contain_state = () =>
            Result.XPathValue("Request/Transaction/CardTxn/Card/Cv2Avs/state_province").ShouldEqual(BillingAddress.State);

        It should_contain_postcode = () =>
            Result.XPathValue("Request/Transaction/CardTxn/Card/Cv2Avs/postcode").ShouldEqual(BillingAddress.Postcode);

        Because of = () =>
        {
            var builder = new DataCashMoToRequestBuilder(Configuration);
            Result = builder.Build(MerchantReference, Amount, CardDetails, BillingAddress);
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
        static BillingAddress BillingAddress = new BillingAddress
            {
                StreetAddress1 = "Some Company",
                StreetAddress2 = "On Some Street",
                StreetAddress3 = "In Some Place",
                StreetAddress4 = "In Some Town",
                City = "In Some City",
                State = "In Some State",
                Postcode = "With Some Postcode"
            };
    }
}