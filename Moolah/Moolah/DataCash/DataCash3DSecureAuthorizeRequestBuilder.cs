using System.Xml.Linq;

namespace Moolah.DataCash
{
    /// <summary>
    /// Builds DataCash request XML to authorize a pending 3D-Secure payment transaction.
    /// </summary>
    public class DataCash3DSecureAuthorizeRequestBuilder : DataCashRequestBuilderBase, IDataCashAuthorizeRequestBuilder
    {
        public DataCash3DSecureAuthorizeRequestBuilder(DataCashConfiguration configuration)
            : base(configuration)
        {
        }

        public XDocument Build(string transactionReference, string PARes)
        {
            return GetDocument(
                HistoricTxnElement(transactionReference, PARes));
        }

        private XElement HistoricTxnElement(string transactionReference, string PARes)
        {
            return new XElement("HistoricTxn",
                                new XElement("reference", transactionReference),
                                new XElement("method", "threedsecure_authorization_request"),
                                new XElement("pares_message", PARes));
        }
    }
}