using System;
using System.Collections.Specialized;
using System.Web;

namespace Moolah.PayPal
{
    public class PayPalExpressCheckout : IPayPalExpressCheckout
    {
        private readonly PayPalConfiguration _configuration;
        private readonly IHttpClient _httpClient;
        private readonly ISetExpressCheckoutResponseParser _setExpressCheckoutResponseParser;

        public PayPalExpressCheckout(PayPalConfiguration configuration)
            : this(configuration, new HttpClient(), new SetExpressCheckoutResponseParser(configuration))
        {
        }

        /// <summary>
        /// For testing.
        /// </summary>
        public PayPalExpressCheckout(PayPalConfiguration configuration, IHttpClient httpClient,
            ISetExpressCheckoutResponseParser setExpressCheckoutResponseParser)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (httpClient == null) throw new ArgumentNullException("httpClient");
            if (setExpressCheckoutResponseParser == null) throw new ArgumentNullException("setExpressCheckoutResponseParser");
            _configuration = configuration;
            _httpClient = httpClient;
            _setExpressCheckoutResponseParser = setExpressCheckoutResponseParser;
        }

        public PayPalExpressCheckoutToken SetExpressCheckout(decimal amount, string cancelUrl, string confirmationUrl)
        {
            var queryString = getQueryWithCredentials();
            queryString["METHOD"] = "SetExpressCheckout";
            queryString["PAYMENTREQUEST_0_AMT"] = amount.ToString("0.00");
            queryString["PAYMENTREQUEST_0_CURRENCYCODE"] = "GBP";
            queryString["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale";
            queryString["cancelUrl"] = cancelUrl;
            queryString["returnUrl"] = confirmationUrl;

            var httpResponse = _httpClient.Get(_configuration.Host + "?" + queryString);

            return _setExpressCheckoutResponseParser.Parse(httpResponse);
        }

        private NameValueCollection getQueryWithCredentials()
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["VERSION"] = "78";
            queryString["USER"] = _configuration.UserId;
            queryString["PWD"] = _configuration.Password;
            queryString["SIGNATURE"] = _configuration.Signature;
            return queryString;
        }

        public PayPalExpressCheckoutDetails GetExpressCheckoutDetails(string payPalToken)
        {
            throw new System.NotImplementedException();
        }

        public IPaymentResponse DoExpressCheckoutPayment(decimal amount, string payPalToken, string payPalPayerId)
        {
            throw new System.NotImplementedException();
        }
    }
}