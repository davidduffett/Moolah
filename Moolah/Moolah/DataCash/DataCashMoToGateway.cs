using System;
using System.Xml.Linq;

namespace Moolah.DataCash
{
    public class DataCashMoToGateway : IPaymentGateway
    {
        private readonly DataCashConfiguration _configuration;
        private readonly IHttpClient _httpClient;
        private readonly IDataCashPaymentRequestBuilder _paymentRequestBuilder;
        private readonly IDataCashResponseParser _responseParser;
        readonly IRefundGateway _refundGateway;

        public DataCashMoToGateway()
            : this(MoolahConfiguration.Current.DataCashMoTo)
        {
        }

        public DataCashMoToGateway(DataCashConfiguration configuration)
            : this(configuration, new HttpClient(), new DataCashMoToRequestBuilder(configuration), new DataCashResponseParser(), new RefundGateway(configuration))
        {
        }

        /// <summary>
        /// TODO: Make internal and visible to Moolah.Specs
        /// </summary>
        public DataCashMoToGateway(
            DataCashConfiguration configuration, 
            IHttpClient httpClient, 
            IDataCashPaymentRequestBuilder requestBuilder,
            IDataCashResponseParser responseParser,
            IRefundGateway refundGateway)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (httpClient == null) throw new ArgumentNullException("httpClient");
            if (requestBuilder == null) throw new ArgumentNullException("requestBuilder");
            if (responseParser == null) throw new ArgumentNullException("responseParser");
            if (refundGateway == null) throw new ArgumentNullException("refundGateway");
            _configuration = configuration;
            _httpClient = httpClient;
            _paymentRequestBuilder = requestBuilder;
            _responseParser = responseParser;
            _refundGateway = refundGateway;
        }

        public ICardPaymentResponse Payment(string merchantReference, decimal amount, CardDetails card, BillingAddress billingAddress = null, string currencyCode = null)
        {
            var requestDocument = _paymentRequestBuilder.Build(merchantReference, amount, currencyCode, card, billingAddress);
            var response = _httpClient.Post(_configuration.Host, requestDocument.ToString(SaveOptions.DisableFormatting));
            return _responseParser.Parse(response);
        }

        public IRefundTransactionResponse RefundTransaction(string originalTransactionReference, decimal amount)
        {
            return _refundGateway.Refund(originalTransactionReference, amount);            
        }
    }
}