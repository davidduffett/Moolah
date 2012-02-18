using System;
using System.Xml.Linq;

namespace Moolah.DataCash
{
    public interface IDataCashRequestBuilder
    {
        XDocument Build(string merchantReference, decimal amount, CardDetails card);
    }

    public class DataCashRequestBuilder : IDataCashRequestBuilder
    {
        private readonly DataCashConfiguration _configuration;

        public DataCashRequestBuilder(DataCashConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            _configuration = configuration;
        }

        public XDocument Build(string merchantReference, decimal amount, CardDetails card)
        {
            var document = new XDocument(new XDeclaration("1.0", "utf-8", null));
            document.Add(new XElement("Request", 
                authenticationElement(),
                transactionElement(merchantReference, amount, card)));
            return document;
        }

        private XElement authenticationElement()
        {
            return new XElement("Authentication",
                                new XElement("client", _configuration.MerchantId),
                                new XElement("password", _configuration.Password));
        }

        private XElement transactionElement(string merchantReference, decimal amount, CardDetails card)
        {
            return new XElement("Transaction",
                                txnDetailsElement(merchantReference, amount),
                                cardTxnElement(card));
        }

        private XElement txnDetailsElement(string merchantReference, decimal amount)
        {
            return new XElement("TxnDetails",
                new XElement("merchantreference", merchantReference),
                new XElement("amount", new XAttribute("currency", "GBP"), amount.ToString("0.00")));
        }

        private XElement cardTxnElement(CardDetails card)
        {
            return new XElement("CardTxn",
                                new XElement("method", "auth"),
                                cardElement(card));
        }

        private XElement cardElement(CardDetails card)
        {
            return new XElement("Card",
                                new XElement("pan", card.Number),
                                new XElement("expirydate", card.ExpiryDate),
                                new XElement("startdate", card.StartDate),
                                new XElement("issuenumber", card.IssueNumber),
                                cv2AvsElement(card));
        }

        private XElement cv2AvsElement(CardDetails card)
        {
            return new XElement("Cv2Avs",
                                new XElement("cv2", card.Cv2));
        }

    }
}