using System;
using System.Web;
using System.Xml.Linq;

namespace Moolah.DataCash
{
    /// <summary>
    /// Builds the DataCash 3D-Secure payment request XML.
    /// This is the same as MoTo, but with added sections for 3D-Secure and the Browser being used.
    /// </summary>
    public class DataCash3DSecureRequestBuilder : DataCashRequestBuilderBase, IDataCashPaymentRequestBuilder
    {
        private readonly DataCash3DSecureConfiguration _configuration;
        private readonly HttpRequestBase _httpRequest;

        public ITimeProvider SystemTime { get; set; }

        public DataCash3DSecureRequestBuilder(DataCash3DSecureConfiguration configuration)
            : this(configuration, new HttpRequestWrapper(HttpContext.Current.Request))
        {
        }

        public DataCash3DSecureRequestBuilder(DataCash3DSecureConfiguration configuration, HttpRequestBase httpRequest)
            : base(configuration)
        {
            if (httpRequest == null) throw new ArgumentNullException("httpRequest");
            _configuration = configuration;
            _httpRequest = httpRequest;
            SystemTime = new TimeProvider();
        }

        public XDocument Build(string merchantReference, decimal amount, CardDetails card)
        {
            return GetDocument(
                TxnDetailsElement(merchantReference, amount),
                CardTxnElement(card));
        }

        protected override XElement TxnDetailsElement(string merchantReference, decimal amount)
        {
            var element = base.TxnDetailsElement(merchantReference, amount);
            element.Add(threeDSecureElement());
            return element;
        }

        private XElement threeDSecureElement()
        {
            return new XElement("ThreeDSecure",
                                new XElement("verify", "yes"),
                                new XElement("merchant_url", _configuration.MerchantUrl),
                                new XElement("purchase_desc", _configuration.PurchaseDescription),
                                new XElement("purchase_datetime", SystemTime.Now.ToString("yyyyMMdd HH:mm:ss")),
                                browserElement());
        }

        private XElement browserElement()
        {
            return new XElement("Browser",
                                new XElement("device_category", "0"),
                                new XElement("accept_headers", _httpRequest.Headers["Accept"]),
                                new XElement("user_agent", _httpRequest.UserAgent));
        }
    }
}