using System;

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

    public class DataCash3DSecureConfiguration : DataCashConfiguration
    {
        public DataCash3DSecureConfiguration(PaymentEnvironment environment, string merchantId, string password,
            string merchantUrl, string purchaseDescription)
            : base(environment, merchantId, password)
        {
            if (string.IsNullOrWhiteSpace(merchantUrl)) throw new ArgumentNullException("merchantUrl");
            if (string.IsNullOrWhiteSpace(purchaseDescription)) throw new ArgumentNullException("purchaseDescription");
            MerchantUrl = merchantUrl;
            PurchaseDescription = purchaseDescription;
        }

        /// <summary>
        /// The URL of the merchant website.  
        /// This is displayed to the customer by the bank during the 3D-Secure authentication process.
        /// </summary>
        public string MerchantUrl { get; private set; }

        /// <summary>
        /// A short description of items to be purchased.
        /// This is displayed to the customer by the bank during the 3D-Secure authentication process.
        /// </summary>
        public string PurchaseDescription { get; private set; }
    }
}