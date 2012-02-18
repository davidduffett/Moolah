namespace Moolah.DataCash
{
    public class DataCashConfiguration
    {
        const string TestHost = "https://testserver.datacash.com/Transaction";
        const string LiveHost = "https://mars.transaction.datacash.com/Transaction";

        public DataCashConfiguration(PaymentEnvironment environment, string merchantId, string password)
        {
            Environment = environment;
            Host = environment == PaymentEnvironment.Live ? LiveHost : TestHost;
            MerchantId = merchantId;
            Password = password;
        }

        public PaymentEnvironment Environment { get; private set; }
        public string Host { get; private set; }
        public string MerchantId { get; private set; }
        public string Password { get; private set; }
    }
}