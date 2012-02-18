using System;

namespace Moolah.DataCash
{
    public class DataCashPaymentGateway : IPaymentGateway
    {
        private readonly DataCashConfiguration _configuration;
        private readonly IHttpClient _httpClient;
        private readonly IDataCashRequestBuilder _requestBuilder;
        
        public DataCashPaymentGateway(DataCashConfiguration configuration)
            : this(configuration, new HttpClient(), new DataCashRequestBuilder(configuration))
        {
        }

        /// <summary>
        /// TODO: Make internal and visible to Moolah.Specs
        /// </summary>
        public DataCashPaymentGateway(DataCashConfiguration configuration, IHttpClient httpClient, IDataCashRequestBuilder requestBuilder)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (httpClient == null) throw new ArgumentNullException("httpClient");
            if (requestBuilder == null) throw new ArgumentNullException("requestBuilder");
            _configuration = configuration;
            _httpClient = httpClient;
            _requestBuilder = requestBuilder;
        }

        public IPaymentResponse Payment(string merchantReference, decimal amount, CardDetails card)
        {
            throw new NotImplementedException();
        }
    }
}