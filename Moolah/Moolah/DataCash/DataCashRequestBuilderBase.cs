using System;
using System.Xml.Linq;

namespace Moolah.DataCash
{
    public interface IDataCashPaymentRequestBuilder
    {
        XDocument Build(string merchantReference, decimal amount, CardDetails card);
    }

    public interface IDataCashAuthorizeRequestBuilder
    {
        XDocument Build(string transactionReference, string PARes);
    }

    public abstract class DataCashRequestBuilderBase
    {
        private readonly DataCashConfiguration _configuration;

        public DataCashRequestBuilderBase(DataCashConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            _configuration = configuration;
        }

        public XDocument GetDocument(params object[] transactionElements)
        {
            return new XDocument(new XDeclaration("1.0", "utf-8", null), requestElement(transactionElements));
        }

        private XElement requestElement(params object[] transactionElements)
        {
            return new XElement("Request",
                authenticationElement(),
                transactionElement(transactionElements));
        }

        private XElement authenticationElement()
        {
            return new XElement("Authentication",
                                new XElement("client", _configuration.MerchantId),
                                new XElement("password", _configuration.Password));
        }

        private XElement transactionElement(params object[] elements)
        {
            return new XElement("Transaction", elements);
        }

        protected virtual XElement TxnDetailsElement(string merchantReference, decimal amount)
        {
            return new XElement("TxnDetails",
                new XElement("merchantreference", merchantReference),
                new XElement("amount", new XAttribute("currency", "GBP"), amount.ToString("0.00")));
        }

        protected virtual XElement CardTxnElement(CardDetails card)
        {
            return new XElement("CardTxn",
                                new XElement("method", "auth"),
                                CardElement(card));
        }

        protected virtual XElement CardElement(CardDetails card)
        {
            return new XElement("Card",
                                new XElement("pan", card.Number),
                                new XElement("expirydate", card.ExpiryDate),
                                new XElement("startdate", card.StartDate),
                                new XElement("issuenumber", card.IssueNumber),
                                Cv2AvsElement(card));
        }

        protected virtual XElement Cv2AvsElement(CardDetails card)
        {
            return new XElement("Cv2Avs",
                                new XElement("cv2", card.Cv2));
        }
    }
}