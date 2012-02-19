using System;
using System.Xml.Linq;

namespace Moolah.DataCash
{
    public class DataCashPaymentGateway : IPaymentGateway
    {
        private readonly DataCashConfiguration _configuration;
        private readonly IHttpClient _httpClient;
        private readonly IDataCashRequestBuilder _requestBuilder;
        private readonly IDataCashResponseParser _responseParser;
        
        public DataCashPaymentGateway(DataCashConfiguration configuration)
            : this(configuration, new HttpClient(), new DataCashRequestBuilder(configuration), new DataCashResponseParser())
        {
        }

        /// <summary>
        /// TODO: Make internal and visible to Moolah.Specs
        /// </summary>
        public DataCashPaymentGateway(
            DataCashConfiguration configuration, 
            IHttpClient httpClient, 
            IDataCashRequestBuilder requestBuilder,
            IDataCashResponseParser responseParser)
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

        public IPaymentResponse Payment(string merchantReference, decimal amount, CardDetails card)
        {
            var requestDocument = _requestBuilder.Build(merchantReference, amount, card);
            var response = _httpClient.Post(_configuration.Host, requestDocument.ToString(SaveOptions.DisableFormatting));
            return _responseParser.Parse(response);
        }
    }
}