using System;

namespace Moolah.PayPal
{
    public class PayPalConfiguration
    {
        const string TestHost = "https://api-3t.sandbox.paypal.com/nvp";
        const string LiveHost = "https://api-3t.paypal.com/nvp";

        public PayPalConfiguration(PaymentEnvironment environment, string userId, string password, string signature)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException("userId");
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("password");
            if (string.IsNullOrEmpty(signature)) throw new ArgumentNullException("signature");

            Environment = environment;
            UserId = userId;
            Password = password;
            Signature = signature;

            Host = environment == PaymentEnvironment.Live ? LiveHost : TestHost;
        }

        public PaymentEnvironment Environment { get; private set; }
        public string Host { get; private set; }
        public string UserId { get; private set; }
        public string Password { get; private set; }
        public string Signature { get; private set; }
    }
}