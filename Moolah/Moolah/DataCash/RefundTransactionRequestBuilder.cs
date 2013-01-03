using System.Xml.Linq;

namespace Moolah.DataCash
{
    public class RefundTransactionRequestBuilder : DataCashRequestBuilderBase, IDataCashRefundTransactionRequestBuilder
    {
        public RefundTransactionRequestBuilder(DataCashConfiguration configuration)
            : base(configuration)
        {
        }

        public XDocument Build(string originalTransactionReference, decimal amount)
        {
            return GetDocument(
                TxnDetailsElement(null, amount),
                HistoricTxnElement(originalTransactionReference));
        }

        protected override XElement TxnDetailsElement(string merchantReference, decimal amount)
        {
            return new XElement("TxnDetails",
                new XElement("amount", new XAttribute("currency", "GBP"), amount.ToString("0.00")));
        }

        private XElement HistoricTxnElement(string originalTransactionReference)
        {
            return new XElement("HistoricTxn",
                                new XElement("reference", originalTransactionReference),
                                new XElement("method", "txn_refund"));
        }
    }
}