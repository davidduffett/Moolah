using System;
using System.Collections.Specialized;
using System.Web;
using System.Xml.Linq;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.DataCash;

namespace Moolah.Specs.DataCash
{
    [Subject(typeof(DataCash3DSecureRequestBuilder))]
    public class When_building_3d_secure_auth_request_xml : WithFakes
    {
        Behaves_like<DataCashPaymentRequestBehavior> a_datacash_payment_request;

        It should_specify_3d_secure_verify_yes = () =>
            Result.XPathValue("Request/Transaction/TxnDetails/ThreeDSecure/verify").ShouldEqual("yes");

        It should_contain_3d_secure_purchase_description = () =>
            Result.XPathValue("Request/Transaction/TxnDetails/ThreeDSecure/purchase_desc").ShouldEqual(Configuration.PurchaseDescription);

        It should_contain_3d_secure_purchase_date_time = () =>
            Result.XPathValue("Request/Transaction/TxnDetails/ThreeDSecure/purchase_datetime").ShouldEqual(FakeSystemTime.Now.ToString("yyyyMMdd HH:mm:ss"));

        It should_contain_browser_device_category = () =>
            Result.XPathValue("Request/Transaction/TxnDetails/ThreeDSecure/Browser/device_category").ShouldEqual("0");

        It should_contain_accept_headers_from_request = () =>
            Result.XPathValue("Request/Transaction/TxnDetails/ThreeDSecure/Browser/accept_headers").ShouldEqual(AcceptHeaders);

        It should_contain_user_agent_from_request = () =>
            Result.XPathValue("Request/Transaction/TxnDetails/ThreeDSecure/Browser/user_agent").ShouldEqual(UserAgent);

        Because of = () =>
        {
            HttpRequest = An<HttpRequestBase>();
            HttpRequest.WhenToldTo(x => x.UserAgent).Return(UserAgent);
            HttpRequest.WhenToldTo(x => x.Headers).Return(
                new NameValueCollection { { "Accept", AcceptHeaders } });

            var builder = new DataCash3DSecureRequestBuilder(Configuration, HttpRequest) { SystemTime = FakeSystemTime };
            Result = builder.Build(MerchantReference, Amount, CardDetails);
        };

        Establish context = () =>
            {
                FakeSystemTime = An<ITimeProvider>();
                FakeSystemTime.WhenToldTo(x => x.Now).Return(new DateTime(2012, 05, 30, 15, 39, 25));
            };

        static ITimeProvider FakeSystemTime;
        protected static XDocument Result;
        protected static DataCash3DSecureConfiguration Configuration = new DataCash3DSecureConfiguration(
            PaymentEnvironment.Test, "merchant", "password123",
            "http://www.example.com", "Example purchase description");
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

        static HttpRequestBase HttpRequest;
        const string AcceptHeaders = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
        const string UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:10.0.2) Gecko/20100101 Firefox/10.0.2";
    }
}