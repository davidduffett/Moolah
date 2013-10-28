using System.Xml.Linq;

namespace Moolah.DataCash
{
    /// <summary>
    /// Builds the DataCash MoTo request XML.
    /// </summary>
    public class DataCashMoToRequestBuilder : DataCashRequestBuilderBase, IDataCashPaymentRequestBuilder
    {
        public DataCashMoToRequestBuilder(DataCashConfiguration configuration)
            : base(configuration)
        {
        }

        public XDocument Build(string merchantReference, decimal amount, string currencyCode, CardDetails card, BillingAddress billingAddress)
        {
            return GetDocument(
                TxnDetailsElement(merchantReference, amount, currencyCode),
                CardTxnElement(card, billingAddress));
        }
    }
}