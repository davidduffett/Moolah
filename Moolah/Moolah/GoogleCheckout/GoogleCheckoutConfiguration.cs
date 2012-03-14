using System;
using System.Configuration;
using GCheckout;

namespace Moolah.GoogleCheckout
{
    public class GoogleCheckoutConfiguration : ConfigurationElement
    {
        const string TestButton = "http://sandbox.google.com/checkout/buttons/checkout.gif";
        const string LiveButton = "http://checkout.google.com/buttons/checkout.gif";

        const string TestHost = "https://sandbox.google.com/checkout/api/checkout/v2/merchantCheckout/Merchant/";
        const string LiveHost = "https://checkout.google.com/api/checkout/v2/merchantCheckout/Merchant/";

        internal GoogleCheckoutConfiguration()
        {
        }

        public GoogleCheckoutConfiguration(PaymentEnvironment environment, string merchantId, string merchantKey)
        {
            if (string.IsNullOrWhiteSpace(merchantId)) throw new ArgumentNullException("merchantId");
            if (string.IsNullOrWhiteSpace(merchantKey)) throw new ArgumentNullException("merchantKey");
            Environment = environment;
            MerchantId = merchantId;
            MerchantKey = merchantKey;
        }

        static class Attributes
        {
            public const string Environment = "environment";
            public const string MerchantId = "merchantId";
            public const string MerchantKey = "merchantKey";
        }

        [ConfigurationProperty(Attributes.Environment)]
        public PaymentEnvironment Environment
        {
            get { return (PaymentEnvironment)this[Attributes.Environment]; }
            set { this[Attributes.Environment] = value; }
        }

        [ConfigurationProperty(Attributes.MerchantId)]
        public string MerchantId
        {
            get { return (string)this[Attributes.MerchantId]; }
            set { this[Attributes.MerchantId] = value; }
        }

        [ConfigurationProperty(Attributes.MerchantKey)]
        public string MerchantKey
        {
            get { return (string)this[Attributes.MerchantKey]; }
            set { this[Attributes.MerchantKey] = value; }
        }

        public string ButtonSrc
        {
            get { return Environment == PaymentEnvironment.Live ? LiveButton : TestButton; }
        }

        public string Host
        {
            get { return (Environment == PaymentEnvironment.Live ? LiveHost : TestHost) + MerchantId; }
        }

        public EnvironmentType EnvironmentType
        {
            get { return Environment == PaymentEnvironment.Live ? EnvironmentType.Production : EnvironmentType.Sandbox; }
        }
    }
}