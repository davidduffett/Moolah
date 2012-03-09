using System;
using System.Configuration;

namespace Moolah.DataCash
{
    public class DataCashConfiguration : ConfigurationElement
    {
        const string TestHost = "https://testserver.datacash.com/Transaction";
        const string LiveHost = "https://mars.transaction.datacash.com/Transaction";

        internal DataCashConfiguration()
        {
        }

        public DataCashConfiguration(PaymentEnvironment environment, string merchantId, string password)
        {
            Environment = environment;
            MerchantId = merchantId;
            Password = password;
        }

        static class Attributes
        {
            public const string Environment = "environment";
            public const string MerchantId = "merchantId";
            public const string Password = "password";
        }

        [ConfigurationProperty(Attributes.Environment)]
        public PaymentEnvironment Environment
        {
            get { return (PaymentEnvironment) this[Attributes.Environment]; }
            set { this[Attributes.Environment] = value; }
        }

        [ConfigurationProperty(Attributes.MerchantId)]
        public string MerchantId
        {
            get { return (string)this[Attributes.MerchantId]; }
            set { this[Attributes.MerchantId] = value; }
        }

        [ConfigurationProperty(Attributes.Password)]
        public string Password
        {
            get { return (string) this[Attributes.Password]; }
            set { this[Attributes.Password] = value; }
        }

        public string Host
        {
            get { return Environment == PaymentEnvironment.Live ? LiveHost : TestHost; }
        }
    }

    public class DataCash3DSecureConfiguration : DataCashConfiguration
    {
        internal DataCash3DSecureConfiguration()
        {
        }

        public DataCash3DSecureConfiguration(PaymentEnvironment environment, string merchantId, string password,
            string merchantUrl, string purchaseDescription)
            : base(environment, merchantId, password)
        {
            if (string.IsNullOrWhiteSpace(merchantUrl)) throw new ArgumentNullException("merchantUrl");
            if (string.IsNullOrWhiteSpace(purchaseDescription)) throw new ArgumentNullException("purchaseDescription");
            MerchantUrl = merchantUrl;
            PurchaseDescription = purchaseDescription;
        }

        static class Attributes
        {
            public const string MerchantUrl = "merchantUrl";
            public const string PurchaseDescription = "purchaseDescription";
        }

        /// <summary>
        /// The URL of the merchant website.  
        /// This is displayed to the customer by the bank during the 3D-Secure authentication process.
        /// </summary>
        [ConfigurationProperty(Attributes.MerchantUrl)]
        public string MerchantUrl
        {
            get { return (string) this[Attributes.MerchantUrl]; }
            set { this[Attributes.MerchantUrl] = value; }
        }

        /// <summary>
        /// A short description of items to be purchased.
        /// This is displayed to the customer by the bank during the 3D-Secure authentication process.
        /// </summary>
        [ConfigurationProperty(Attributes.PurchaseDescription)]
        public string PurchaseDescription
        {
            get { return (string) this[Attributes.PurchaseDescription]; }
            set { this[Attributes.PurchaseDescription] = value; }
        }
    }
}