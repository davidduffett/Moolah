using System.Xml.Linq;

namespace Moolah.DataCash
{
    public class RefundTransactionRequestBuilder : DataCashRequestBuilderBase, IDataCashRefundTransactionRequestBuilder
    {
        public RefundTransactionRequestBuilder(DataCashConfiguration configuration)
            : base(configuration)
        {
        }

        public XDocument Build(string originalTransactionReference, decimal amount, string currencyCode)
        {
            return GetDocument(
                TxnDetailsElement(null, amount, currencyCode),
                HistoricTxnElement(originalTransactionReference));
        }

        protected override XElement TxnDetailsElement(string merchantReference, decimal amount, string currencyCode)
        {
            var amountElement = new XElement("amount", amount.ToString("0.00"));
            if (!string.IsNullOrWhiteSpace(currencyCode))
                amountElement.Add(new XAttribute("currency", currencyCode));
            return new XElement("TxnDetails", amountElement);
        }

        private XElement HistoricTxnElement(string originalTransactionReference)
        {
            return new XElement("HistoricTxn",
                                new XElement("reference", originalTransactionReference),
                                new XElement("method", "txn_refund"));
        }
    }
}