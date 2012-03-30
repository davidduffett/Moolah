using System;
using System.Configuration;

namespace Moolah.PayPal
{
    public class PayPalConfiguration : ConfigurationElement
    {
        const string TestHost = "https://api-3t.sandbox.paypal.com/nvp";
        const string TestCheckoutUrlFormat = "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token={0}";

        // Sandbox Credentials provided in Integration Guide
        // https://cms.paypal.com/us/cgi-bin/?cmd=_render-content&content_ID=developer/e_howto_api_ECGettingStarted
        const string TestUser = "sdk-three_api1.sdk.com";
        const string TestPassword = "QFZCWN5HZM8VBG7Q";
        const string TestSignature = "A-IzJhZZjhg29XQ2qnhapuwxIDzyAZQ92FRP5dqBzVesOkzbdUONzmOU";

        const string LiveHost = "https://api-3t.paypal.com/nvp";
        const string LiveCheckoutUrlFormat = "https://www.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token={0}";

        internal PayPalConfiguration()
            : this(PaymentEnvironment.Test)
        {
        }

        /// <summary>
        /// Used for test environment, where PayPal sandbox credentials are static.
        /// </summary>
        public PayPalConfiguration(PaymentEnvironment environment)
            : this(environment, TestUser, TestPassword, TestSignature)
        {
            if (environment == PaymentEnvironment.Live)
                throw new ArgumentException("PayPal UserId, Password and Signature must be provided for the live environment.");
        }

        public PayPalConfiguration(PaymentEnvironment environment, string userId = null, string password = null, string signature = null)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException("userId");
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("password");
            if (string.IsNullOrEmpty(signature)) throw new ArgumentNullException("signature");

            Environment = environment;
            UserId = userId;
            Password = password;
            Signature = signature;
        }

        static class Attributes
        {
            public const string Environment = "environment";
            public const string UserId = "userId";
            public const string Password = "password";
            public const string Signature = "signature";
            public const string LocaleCode = "localeCode";
            public const string UseLocaleFromCurrentCulture = "useLocaleFromCurrentCulture";
        }

        [ConfigurationProperty(Attributes.Environment)]
        public PaymentEnvironment Environment
        {
            get { return (PaymentEnvironment) this[Attributes.Environment]; }
            set { this[Attributes.Environment] = value; }
        }

        [ConfigurationProperty(Attributes.UserId)]
        public string UserId
        {
            get { return (string) this[Attributes.UserId]; }
            set { this[Attributes.UserId] = value; }
        }

        [ConfigurationProperty(Attributes.Password)]
        public string Password
        {
            get { return (string) this[Attributes.Password]; }
            set { this[Attributes.Password] = value; }
        }

        [ConfigurationProperty(Attributes.Signature)]
        public string Signature
        {
            get { return (string) this[Attributes.Signature]; }
            set { this[Attributes.Signature] = value; }
        }

        /// <summary>
        /// PayPal Locale Code to use (optional).
        /// If <seealso cref="UseLocaleFromCurrentCulture"/> is specified, then this is ignored.
        /// </summary>
        [ConfigurationProperty(Attributes.LocaleCode)]
        public string LocaleCode
        {
            get { return (string) this[Attributes.LocaleCode]; }
            set { this[Attributes.LocaleCode] = value; }
        }

        /// <summary>
        /// If PayPal supports it, the current culture is used for the PayPal locale.
        /// If it isn't supported, no locale code is specified (defaults to US).
        /// </summary>
        [ConfigurationProperty(Attributes.UseLocaleFromCurrentCulture)]
        public bool UseLocaleFromCurrentCulture
        {
            get { return (bool) this[Attributes.UseLocaleFromCurrentCulture]; }
            set { this[Attributes.UseLocaleFromCurrentCulture] = value; }
        }

        public string Host
        {
            get { return Environment == PaymentEnvironment.Live ? LiveHost : TestHost; }
        }

        public string CheckoutUrlFormat
        {
            get { return Environment == PaymentEnvironment.Live ? LiveCheckoutUrlFormat : TestCheckoutUrlFormat; }
        }
    }
}