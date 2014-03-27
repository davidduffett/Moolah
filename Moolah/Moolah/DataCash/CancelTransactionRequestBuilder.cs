using System.Xml.Linq;

namespace Moolah.DataCash
{
    public class CancelTransactionRequestBuilder : DataCashRequestBuilderBase, IDataCashCancelTransactionRequestBuilder
    {
        public CancelTransactionRequestBuilder(DataCashConfiguration configuration)
            : base(configuration)
        {
        }

        public XDocument Build(string originalTransactionReference)
        {
            return GetDocument(
                HistoricTxnElement(originalTransactionReference));
        }

        private XElement HistoricTxnElement(string originalTransactionReference)
        {
            return new XElement("HistoricTxn",
                                new XElement("reference", originalTransactionReference),
                                new XElement("method", "cancel"));
        }
    }
}