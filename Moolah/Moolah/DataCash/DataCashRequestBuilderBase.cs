using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Moolah.DataCash
{
    public interface IDataCashPaymentRequestBuilder
    {
        XDocument Build(string merchantReference, decimal amount, CardDetails card, BillingAddress billingAddress);
    }

    public interface IDataCashAuthorizeRequestBuilder
    {
        XDocument Build(string transactionReference, string PARes);
    }

    public interface IDataCashRefundTransactionRequestBuilder
    {
        XDocument Build(string originalTransactionReference, decimal amount);
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

        protected virtual XElement CardTxnElement(CardDetails card, BillingAddress billingAddress)
        {
            return new XElement("CardTxn",
                                new XElement("method", "auth"),
                                CardElement(card, billingAddress));
        }

        protected virtual XElement CardElement(CardDetails card, BillingAddress billingAddress)
        {
            return new XElement("Card",
                                new XElement("pan", card.Number),
                                new XElement("expirydate", card.ExpiryDate),
                                new XElement("startdate", card.StartDate),
                                new XElement("issuenumber", card.IssueNumber),
                                Cv2AvsElement(card, billingAddress));
        }

        protected virtual XElement Cv2AvsElement(CardDetails card, BillingAddress billingAddress)
        {
            var cv2AvsElements = new List<XElement>();
            if (billingAddress != null)
            {
                if (!string.IsNullOrWhiteSpace(billingAddress.StreetAddress1))
                    cv2AvsElements.Add(new XElement("street_address1", billingAddress.StreetAddress1));
                if (!string.IsNullOrWhiteSpace(billingAddress.StreetAddress2))
                    cv2AvsElements.Add(new XElement("street_address2", billingAddress.StreetAddress2));
                if (!string.IsNullOrWhiteSpace(billingAddress.StreetAddress3))
                    cv2AvsElements.Add(new XElement("street_address3", billingAddress.StreetAddress3));
                if (!string.IsNullOrWhiteSpace(billingAddress.StreetAddress4))
                    cv2AvsElements.Add(new XElement("street_address4", billingAddress.StreetAddress4));
                if (!string.IsNullOrWhiteSpace(billingAddress.City))
                    cv2AvsElements.Add(new XElement("city", billingAddress.City));
                if (!string.IsNullOrWhiteSpace(billingAddress.State))
                    cv2AvsElements.Add(new XElement("state_province", billingAddress.State));
                if (!string.IsNullOrWhiteSpace(billingAddress.Postcode))
                    cv2AvsElements.Add(new XElement("postcode", billingAddress.Postcode));
            }
            cv2AvsElements.Add(new XElement("cv2", card.Cv2));
            return new XElement("Cv2Avs", cv2AvsElements.ToArray());
        }
    }
}