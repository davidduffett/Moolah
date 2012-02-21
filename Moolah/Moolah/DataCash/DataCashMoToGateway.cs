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
        
        public DataCashMoToGateway(DataCashConfiguration configuration)
            : this(configuration, new HttpClient(), new DataCashMoToRequestBuilder(configuration), new DataCashResponseParser())
        {
        }

        /// <summary>
        /// TODO: Make internal and visible to Moolah.Specs
        /// </summary>
        public DataCashMoToGateway(
            DataCashConfiguration configuration, 
            IHttpClient httpClient, 
            IDataCashPaymentRequestBuilder requestBuilder,
            IDataCashResponseParser responseParser)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (httpClient == null) throw new ArgumentNullException("httpClient");
            if (requestBuilder == null) throw new ArgumentNullException("requestBuilder");
            if (responseParser == null) throw new ArgumentNullException("responseParser");
            _configuration = configuration;
            _httpClient = httpClient;
            _paymentRequestBuilder = requestBuilder;
            _responseParser = responseParser;
        }

        public IPaymentResponse Payment(string merchantReference, decimal amount, CardDetails card)
        {
            var requestDocument = _paymentRequestBuilder.Build(merchantReference, amount, card);
            var response = _httpClient.Post(_configuration.Host, requestDocument.ToString(SaveOptions.DisableFormatting));
            return _responseParser.Parse(response);
        }
    }
}