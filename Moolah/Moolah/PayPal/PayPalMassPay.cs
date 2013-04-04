using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using NLog;

namespace Moolah.PayPal
{
    public class PayPalMassPay : IPayPalMassPay
    {
        static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly PayPalConfiguration _configuration;
        private readonly IHttpClient _httpClient;
        private readonly IPayPalRequestBuilder _requestBuilder;
        private readonly IPayPalResponseParser _responseParser;

        public PayPalMassPay()
            : this(MoolahConfiguration.Current.PayPal)
        {
        }

        public PayPalMassPay(PayPalConfiguration configuration)
            : this(configuration, new HttpClient(), new PayPalRequestBuilder(configuration), new PayPalResponseParser(configuration))
        {
        }

        /// <summary>
        /// For testing.
        /// </summary>
        public PayPalMassPay(PayPalConfiguration configuration, IHttpClient httpClient,
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

        public IPaymentResponse DoMassPayment(IEnumerable<PayReceiver> items, CurrencyCodeType currencyCodeType, ReceiverType receiverType = ReceiverType.EmailAddress, string emailSubject = null)
        {
            _logger.Log("PayPalMassPay.DoMassPayment", 
                new
                {
                    CurrencyCode = currencyCodeType, 
                    ReceiverType = receiverType, 
                    EmailSubject = emailSubject, 
                    Items = items.Select(x => new { x.ReceiverId, x.Amount, x.UniqueId, x.Note })
                });

            var request = _requestBuilder.MassPayment(items, currencyCodeType, receiverType, emailSubject);
            var response = sendToPayPal(request);
            return _responseParser.MassPayment(response);
        }

        private NameValueCollection sendToPayPal(NameValueCollection queryString)
        {
            return HttpUtility.ParseQueryString(_httpClient.Get(_configuration.Host + "?" + queryString));
        }
    }
}