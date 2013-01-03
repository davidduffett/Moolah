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
        private readonly IDataCashRefundTransactionRequestBuilder _refundBuilder;
        private readonly IRefundTransactionResponseParser _refundResponseParser;
        
        public DataCashMoToGateway()
            : this(MoolahConfiguration.Current.DataCashMoTo)
        {
        }

        public DataCashMoToGateway(DataCashConfiguration configuration)
            : this(configuration, new HttpClient(), new DataCashMoToRequestBuilder(configuration), new RefundTransactionRequestBuilder(configuration), new DataCashResponseParser(), new RefundTransactionResponseParser())
        {
        }

        /// <summary>
        /// TODO: Make internal and visible to Moolah.Specs
        /// </summary>
        public DataCashMoToGateway(
            DataCashConfiguration configuration, 
            IHttpClient httpClient, 
            IDataCashPaymentRequestBuilder requestBuilder,
            IDataCashRefundTransactionRequestBuilder refundBuilder,
            IDataCashResponseParser responseParser,
            IRefundTransactionResponseParser refundResponseParser)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (httpClient == null) throw new ArgumentNullException("httpClient");
            if (requestBuilder == null) throw new ArgumentNullException("requestBuilder");
            if (refundBuilder == null) throw new ArgumentNullException("refundBuilder");
            if (responseParser == null) throw new ArgumentNullException("responseParser");
            if (refundResponseParser == null) throw new ArgumentNullException("refundResponseParser");
            _configuration = configuration;
            _httpClient = httpClient;
            _paymentRequestBuilder = requestBuilder;
            _refundBuilder = refundBuilder;
            _responseParser = responseParser;
            _refundResponseParser = refundResponseParser;
        }

        public ICardPaymentResponse Payment(string merchantReference, decimal amount, CardDetails card)
        {
            var requestDocument = _paymentRequestBuilder.Build(merchantReference, amount, card);
            var response = _httpClient.Post(_configuration.Host, requestDocument.ToString(SaveOptions.DisableFormatting));
            return _responseParser.Parse(response);
        }

        public IRefundTransactionResponse RefundTransaction(string originalTransactionReference, decimal amount)
        {
            var requestDocument = _refundBuilder.Build(originalTransactionReference, amount);
            var response = _httpClient.Post(_configuration.Host, requestDocument.ToString(SaveOptions.DisableFormatting));
            return _refundResponseParser.Parse(response);
        }
    }
}