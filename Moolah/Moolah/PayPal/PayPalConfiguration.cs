using System;

namespace Moolah.PayPal
{
    public class PayPalConfiguration
    {
        const string TestHost = "https://api-3t.sandbox.paypal.com/nvp";
        const string TestCheckoutUrlFormat = "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token={0}";

        // Sandbox Credentials provided in Integration Guide
        // https://cms.paypal.com/us/cgi-bin/?cmd=_render-content&content_ID=developer/e_howto_api_ECGettingStarted
        const string TestUser = "sdk-three_api1.sdk.com";
        const string TestPassword = "QFZCWN5HZM8VBG7";
        const string TestSignature = "A-IzJhZZjhg29XQ2qnhapuwxIDzyAZQ92FRP5dqBzVesOkzbdUONzmOU";

        const string LiveHost = "https://api-3t.paypal.com/nvp";
        const string LiveCheckoutUrlFormat = "https://www.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token={0}";

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

            Host = environment == PaymentEnvironment.Live ? LiveHost : TestHost;
            CheckoutUrlFormat = environment == PaymentEnvironment.Live ? LiveCheckoutUrlFormat : TestCheckoutUrlFormat;
        }

        public PaymentEnvironment Environment { get; private set; }
        public string UserId { get; private set; }
        public string Password { get; private set; }
        public string Signature { get; private set; }

        public string Host { get; private set; }
        public string CheckoutUrlFormat { get; private set; }
    }
}