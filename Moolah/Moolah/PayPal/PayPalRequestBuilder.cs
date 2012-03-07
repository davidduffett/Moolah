using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;

namespace Moolah.PayPal
{
    public interface IPayPalRequestBuilder
    {
        NameValueCollection SetExpressCheckout(decimal amount, string cancelUrl, string confirmationUrl);
        NameValueCollection GetExpressCheckoutDetails(string payPalToken);
        NameValueCollection DoExpressCheckoutPayment(decimal amount, string payPalToken, string payPalPayerId);
        NameValueCollection SetExpressCheckout(OrderDetails orderDetails, string cancelUrl, string confirmationUrl);
    }

    /// <summary>
    /// See https://cms.paypal.com/us/cgi-bin/?cmd=_render-content&content_ID=developer/e_howto_api_ECGettingStarted
    /// </summary>
    public class PayPalRequestBuilder : IPayPalRequestBuilder
    {
        private readonly PayPalConfiguration _configuration;

        public PayPalRequestBuilder(PayPalConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            _configuration = configuration;
        }

        public NameValueCollection SetExpressCheckout(decimal amount, string cancelUrl, string confirmationUrl)
        {
            var request = getBaseSetExpressCheckoutRequest(amount, cancelUrl, confirmationUrl);
            return request;
        }

        NameValueCollection getBaseSetExpressCheckoutRequest(decimal amount, string cancelUrl, string confirmationUrl)
        {
            var request = getQueryWithCredentials();
            request["METHOD"] = "SetExpressCheckout";
            request["PAYMENTREQUEST_0_CURRENCYCODE"] = "GBP";
            request["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale";
            request["PAYMENTREQUEST_0_AMT"] = amount.AsPayPalFormatString();
            request["cancelUrl"] = cancelUrl;
            request["returnUrl"] = confirmationUrl;
            return request;
        }

        public NameValueCollection SetExpressCheckout(OrderDetails orderDetails, string cancelUrl, string confirmationUrl)
        {
            var request = getBaseSetExpressCheckoutRequest(orderDetails.OrderTotal, cancelUrl, confirmationUrl);
            addOptionalValueToRequest("PAYMENTREQUEST_0_TAXAMT", orderDetails.TaxTotal, request);
            addOptionalValueToRequest("PAYMENTREQUEST_0_SHIPPINGAMT", orderDetails.ShippingTotal, request);
            addOptionalValueToRequest("PAYMENTREQUEST_0_SHIPDISCAMT", orderDetails.ShippingDiscount, request);
            addOptionalValueToRequest("ALLOWNOTE", orderDetails.AllowNote, request);
            addOptionalValueToRequest("PAYMENTREQUEST_0_DESC", orderDetails.OrderDescription, request);

            var lineNumber = 0;
            var itemTotal = 0m;
            if (orderDetails.Items != null)
            {
                foreach (var line in orderDetails.Items)
                {
                    addOptionalValueToRequest("L_PAYMENTREQUEST_0_NAME" + lineNumber, line.Name, request);
                    addOptionalValueToRequest("L_PAYMENTREQUEST_0_NUMBER" + lineNumber, line.Number, request);
                    addOptionalValueToRequest("L_PAYMENTREQUEST_0_DESC" + lineNumber, line.Description, request);
                    addOptionalValueToRequest("L_PAYMENTREQUEST_0_AMT" + lineNumber, line.UnitPrice, request);
                    addOptionalValueToRequest("L_PAYMENTREQUEST_0_TAXAMT" + lineNumber, line.Tax, request);
                    addOptionalValueToRequest("L_PAYMENTREQUEST_0_QTY" + lineNumber, line.Quantity, request);
                    addOptionalValueToRequest("L_PAYMENTREQUEST_0_ITEMURL" + lineNumber, line.ItemUrl, request);

                    itemTotal += (line.UnitPrice ?? 0) * (line.Quantity ?? 1);
                    lineNumber++;
                }
            }
            if (orderDetails.Discounts != null)
            {
                foreach(var line in orderDetails.Discounts)
                {
                    addOptionalValueToRequest("L_PAYMENTREQUEST_0_NAME" + lineNumber, line.Description, request);
                    addOptionalValueToRequest("L_PAYMENTREQUEST_0_AMT" + lineNumber, line.Amount.AsNegativeValue(), request);
                    addOptionalValueToRequest("L_PAYMENTREQUEST_0_TAXAMT" + lineNumber, line.Tax.AsNegativeValue(), request);
                    addOptionalValueToRequest("L_PAYMENTREQUEST_0_QTY" + lineNumber, line.Quantity, request);

                    itemTotal += line.Amount.AsNegativeValue() * (line.Quantity ?? 1);
                    lineNumber++;
                }
            }

            if (itemTotal > 0)
                addOptionalValueToRequest("PAYMENTREQUEST_0_ITEMAMT", itemTotal, request);
            
            return request;
        }

        void addOptionalValueToRequest(string fieldName, int? value, NameValueCollection request)
        {
            if (value.HasValue) request[fieldName] = value.Value.ToString(CultureInfo.InvariantCulture);
        }

        void addOptionalValueToRequest(string fieldName, decimal? value, NameValueCollection request)
        {
            if (value.HasValue) request[fieldName] = value.Value.AsPayPalFormatString();
        }

        void addOptionalValueToRequest(string fieldName, bool? value, NameValueCollection request)
        {
            if (value.HasValue) request[fieldName] = value.Value ? "1" : "0";
        }

        void addOptionalValueToRequest(string fieldName, string value, NameValueCollection request)
        {
            if (!String.IsNullOrEmpty(value)) request[fieldName] = value;
        }

        public NameValueCollection GetExpressCheckoutDetails(string payPalToken)
        {
            var request = getQueryWithCredentials();
            request["METHOD"] = "GetExpressCheckoutDetails";
            request["TOKEN"] = payPalToken;
            return request;
        }

        public NameValueCollection DoExpressCheckoutPayment(decimal amount, string payPalToken, string payPalPayerId)
        {
            var request = getQueryWithCredentials();
            request["METHOD"] = "DoExpressCheckoutPayment";
            request["TOKEN"] = payPalToken;
            request["PAYERID"] = payPalPayerId;
            request["PAYMENTREQUEST_0_AMT"] = amount.ToString("0.00");
            request["PAYMENTREQUEST_0_CURRENCYCODE"] = "GBP";
            request["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale";
            return request;
        }

        private NameValueCollection getQueryWithCredentials()
        {
            // ParseQueryString returns a HttpValueCollection that handles url encoding of values automatically.
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["VERSION"] = "78";
            queryString["USER"] = _configuration.UserId;
            queryString["PWD"] = _configuration.Password;
            queryString["SIGNATURE"] = _configuration.Signature;
            return queryString;
        }
    }
}