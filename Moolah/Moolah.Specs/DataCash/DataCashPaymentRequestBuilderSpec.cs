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
            Result.XPathValue("Request/Transaction/TxnDetails/amount/@currency").ShouldEqual(Currency);

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
        protected static string Currency;
    }

    [Subject(typeof(DataCashMoToRequestBuilder))]
    public class When_building_auth_request_xml_without_billing_address : WithFakes
    {
        Behaves_like<DataCashPaymentRequestBehavior> a_datacash_payment_request;

        It should_not_contain_street_address_1 = () =>
            Result.TryGetXPathValue("Request/Transaction/CardTxn/Card/Cv2Avs/street_address1", out outValue).ShouldBeFalse();

        It should_not_contain_postcode = () =>
            Result.TryGetXPathValue("Request/Transaction/CardTxn/Card/Cv2Avs/postcode", out outValue).ShouldBeFalse();

        Because of = () =>
        {
            var builder = new DataCashMoToRequestBuilder(Configuration);
            Result = builder.Build(MerchantReference, Amount, Currency, CardDetails, null);
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
        protected static string Currency = "EUR";
        static string outValue;
    }

    [Subject(typeof(DataCashMoToRequestBuilder))]
    public class When_building_auth_request_xml_with_billing_address : WithFakes
    {
        Behaves_like<DataCashPaymentRequestBehavior> a_datacash_payment_request;

        It should_contain_street_address_1_with_numeric_parts_of_address_only = () =>
            Result.XPathValue("Request/Transaction/CardTxn/Card/Cv2Avs/street_address1").ShouldEqual("123456");

        It should_contain_postcode = () =>
            Result.XPathValue("Request/Transaction/CardTxn/Card/Cv2Avs/postcode").ShouldEqual(BillingAddress.Postcode);

        Because of = () =>
        {
            var builder = new DataCashMoToRequestBuilder(Configuration);
            Result = builder.Build(MerchantReference, Amount, Currency, CardDetails, BillingAddress);
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
        protected static string Currency = "EUR";
        static BillingAddress BillingAddress = new BillingAddress
            {
                StreetAddress1 = "Some Company 1",
                StreetAddress2 = "On Some Street 2",
                StreetAddress3 = "In Some Place 3",
                StreetAddress4 = "In Some Town 4",
                City = "In Some City 5",
                State = "In Some State 6",
                Postcode = "SW100QJ"
            };
    }

    [Subject(typeof(DataCashMoToRequestBuilder))]
    public class When_building_auth_request_xml_and_postcode_contains_non_alphanumeric_characters : WithFakes
    {
        It should_strip_those_characters_from_the_postcode = () =>
            Result.XPathValue("Request/Transaction/CardTxn/Card/Cv2Avs/postcode").ShouldEqual("postcode");

        Because of = () =>
        {
            var builder = new DataCashMoToRequestBuilder(Configuration);
            Result = builder.Build("123456", 12.99m, "GBP", CardDetails, BillingAddress);
        };

        static XDocument Result;
        static DataCashConfiguration Configuration = new DataCashConfiguration(PaymentEnvironment.Test, "merchant", "password123");
        static CardDetails CardDetails = new CardDetails
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
            Postcode = "p-o! s%t?c:o*dÃe"
        };
    }

    [Subject(typeof(DataCashMoToRequestBuilder))]
    public class When_building_auth_request_xml_and_postcode_is_longer_than_9_characters : WithFakes
    {
        It should_limit_the_postcode_to_9_characters = () =>
            Result.XPathValue("Request/Transaction/CardTxn/Card/Cv2Avs/postcode").ShouldEqual("postcode9");

        Because of = () =>
        {
            var builder = new DataCashMoToRequestBuilder(Configuration);
            Result = builder.Build("123456", 12.99m, "GBP", CardDetails, BillingAddress);
        };

        static XDocument Result;
        static DataCashConfiguration Configuration = new DataCashConfiguration(PaymentEnvironment.Test, "merchant", "password123");
        static CardDetails CardDetails = new CardDetails
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
            Postcode = "postcode90123456"
        };
    }
}