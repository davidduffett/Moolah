using System;
using System.Collections.Specialized;
using System.Web;
using NLog;

namespace Moolah.PayPal
{
    public class PayPalExpressCheckout : IPayPalExpressCheckout
    {
        static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly PayPalConfiguration _configuration;
        private readonly IHttpClient _httpClient;
        private readonly IPayPalRequestBuilder _requestBuilder;
        private readonly IPayPalResponseParser _responseParser;

        public PayPalExpressCheckout() 
            : this(MoolahConfiguration.Current.PayPal)
        {
        }

        public PayPalExpressCheckout(PayPalConfiguration configuration)
            : this(configuration, new HttpClient(), new PayPalRequestBuilder(configuration), new PayPalResponseParser(configuration))
        {
        }

        /// <summary>
        /// For testing.
        /// </summary>
        public PayPalExpressCheckout(PayPalConfiguration configuration, IHttpClient httpClient,
            IPayPalRequestBuilder requestBuilder, IPayPalResponseParser responseParser)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (httpClient == null) throw new ArgumentNullException("httpClient");
            if (requestBuilder == null) throw new ArgumentNullException("requestBuilder");
            if (responseParser == null) throw new ArgumentNullException("responseParser");
            _configuration = configuration;
            _httpClient = httpClient;
            _requestBuilder = requestBuilder;
            _responseParser = responseParser;
        }

        public PayPalExpressCheckoutToken SetExpressCheckout(decimal amount, string cancelUrl, string confirmationUrl)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException("amount", "Amount must be greater than zero.");
            if (string.IsNullOrWhiteSpace(cancelUrl)) throw new ArgumentNullException("cancelUrl");
            if (string.IsNullOrWhiteSpace(confirmationUrl)) throw new ArgumentNullException("confirmationUrl");

            _logger.Log("SetExpressCheckout.Request", new { Amount = amount, CancelUrl = cancelUrl, ConfirmationUrl = confirmationUrl });

            var request = _requestBuilder.SetExpressCheckout(amount, cancelUrl, confirmationUrl);
            return setExpressCheckoutRequestFor(request);
        }

        public PayPalExpressCheckoutToken SetExpressCheckout(OrderDetails orderDetails, string cancelUrl, string confirmationUrl)
        {
            if (orderDetails == null) throw new ArgumentNullException("orderDetails");
            if (string.IsNullOrWhiteSpace(cancelUrl)) throw new ArgumentNullException("cancelUrl");
            if (string.IsNullOrWhiteSpace(confirmationUrl)) throw new ArgumentNullException("confirmationUrl");

            _logger.Log("SetExpressCheckout.Request", orderDetails);

            return setExpressCheckoutRequestFor(_requestBuilder.SetExpressCheckout(orderDetails, cancelUrl, confirmationUrl));
        }

        PayPalExpressCheckoutToken setExpressCheckoutRequestFor(NameValueCollection request)
        {
            var response = sendToPayPal(request);

            _logger.Log("SetExpressCheckout.Response", response);

            return _responseParser.SetExpressCheckout(response);
        }

        public PayPalExpressCheckoutDetails GetExpressCheckoutDetails(string payPalToken)
        {
            if (string.IsNullOrWhiteSpace(payPalToken)) throw new ArgumentNullException("payPalToken");
            
            var request = _requestBuilder.GetExpressCheckoutDetails(payPalToken);
            var response = sendToPayPal(request);
            return _responseParser.GetExpressCheckoutDetails(response);
        }

        public PayPalPaymentResponse DoExpressCheckoutPayment(decimal amount, string payPalToken, string payPalPayerId)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException("amount", "Amount must be greater than zero.");
            if (string.IsNullOrWhiteSpace(payPalToken)) throw new ArgumentNullException("payPalToken");
            if (string.IsNullOrWhiteSpace(payPalPayerId)) throw new ArgumentNullException("payPalPayerId");

            var request = _requestBuilder.DoExpressCheckoutPayment(amount, payPalToken, payPalPayerId);
            var response = sendToPayPal(request);
            return _responseParser.DoExpressCheckoutPayment(response);
        }

        private NameValueCollection sendToPayPal(NameValueCollection queryString)
        {
            return HttpUtility.ParseQueryString(_httpClient.Get(_configuration.Host + "?" + queryString));
        }
    }
}