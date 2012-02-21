using System;
using Machine.Fakes;
using Machine.Specifications;

namespace Moolah.Specs.DataCash
{
    [Ignore("Integration requires DataCash MerchantId and Password to be provided")]
    public abstract class DataCashIntegrationContext : WithFakes
    {
        // NOTE: Provide your own DataCash Test Server credentials!
        protected const string MerchantId = "merchantId";
        protected const string Password = "password";
        // DataCash requires unique merchant references (even on their TEST servers!), so use clock ticks for now.
        protected static Func<string> MerchantReference = () => DateTime.UtcNow.Ticks.ToString();
        // DataCash magic card numbers
        protected const string AuthorisedCardNumber = "1000010000000007";
        protected const string DeclinedCardNumber = "1000350000000106";
        // Specifying expiry in January forces 3D-Secure
        protected const string MoToExpiryDate = "02/18";
        protected const string ThreeDSecureExpiryDate = "01/18";
        // Card number to use in test case
        protected static string CardNumber;
        // Expiry date to use in test case
        protected static string ExpiryDate;
    }
}
