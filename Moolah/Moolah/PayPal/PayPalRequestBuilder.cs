using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;

namespace Moolah.PayPal
{
    public interface IPayPalRequestBuilder
    {
        NameValueCollection SetExpressCheckout(decimal amount, CurrencyCodeType currencyCodeType, string cancelUrl, string confirmationUrl);
        NameValueCollection SetExpressCheckout(OrderDetails orderDetails, string cancelUrl, string confirmationUrl);
        NameValueCollection SetExpressCheckout(OrderDetails orderDetails, string cancelUrl, string confirmationUrl, NameValueCollection optionalFields);
        NameValueCollection GetExpressCheckoutDetails(string payPalToken);
        NameValueCollection DoExpressCheckoutPayment(decimal amount, CurrencyCodeType currencyCodeType, string payPalToken, string payPalPayerId);
        NameValueCollection DoExpressCheckoutPayment(OrderDetails orderDetails, string payPalToken, string payPalPayerId);
        NameValueCollection RefundFullTransaction(string transactionId);
        NameValueCollection RefundPartialTransaction(string transactionId, decimal amount, CurrencyCodeType currencyCodeType, string description);
        NameValueCollection MassPayment(IEnumerable<PayReceiver> receivers, CurrencyCodeType currencyCodeType, ReceiverType receiverType, string emailSubject);
        NameValueCollection CreateRecurringPaymentsProfile(RecurringProfile profile, string payPalToken);
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

        public NameValueCollection SetExpressCheckout(decimal amount, CurrencyCodeType currencyCodeType, string cancelUrl, string confirmationUrl)
        {
            var request = getBaseSetExpressCheckoutRequest(amount, currencyCodeType, cancelUrl, confirmationUrl);
            return request;
        }

        NameValueCollection getBaseSetExpressCheckoutRequest(decimal amount, CurrencyCodeType currencyCodeType, string cancelUrl, string confirmationUrl)
        {
            var request = getQueryWithCredentials();
            request["METHOD"] = "SetExpressCheckout";
            request["PAYMENTREQUEST_0_CURRENCYCODE"] = currencyCodeType.ToString();
            request["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale";
            request["PAYMENTREQUEST_0_AMT"] = amount.AsPayPalFormatString();
            request["cancelUrl"] = cancelUrl;
            request["returnUrl"] = confirmationUrl;

            if (_configuration.UseLocaleFromCurrentCulture)
                request["LOCALECODE"] = Culture.Current.ToPayPalLocaleCode();
            else if (LocaleCodes.LocaleCodeSupported(_configuration.LocaleCode))
                request["LOCALECODE"] = _configuration.LocaleCode;

            return request;
        }

        public NameValueCollection SetExpressCheckout(OrderDetails orderDetails, string cancelUrl, string confirmationUrl)
        {
            var request = getBaseSetExpressCheckoutRequest(orderDetails.OrderTotal, orderDetails.CurrencyCodeType, cancelUrl, confirmationUrl);
            addOrderDetailsValues(orderDetails, request);

            // SetExpressCheckout specific
            addOptionalValueToRequest("ALLOWNOTE", orderDetails.AllowNote, request);
            addOptionalValueToRequest("BUYEREMAILOPTINENABLE", orderDetails.EnableCustomerMarketingEmailOptIn, request);

            return request;
        }

        public NameValueCollection SetExpressCheckout(OrderDetails orderDetails, string cancelUrl, string confirmationUrl, NameValueCollection optionalFields)
        {
            var request = SetExpressCheckout(orderDetails, cancelUrl, confirmationUrl);

            foreach(string key in optionalFields)
            {
                //Make sure we're not overwriting a field that's already been set
                if(request[key] == null)
                    addOptionalValueToRequest(key, optionalFields[key], request);
            }

            return request;
        }

        void addOrderDetailsValues(OrderDetails orderDetails, NameValueCollection request)
        {
            addOptionalValueToRequest("NOSHIPPING", (int)orderDetails.DisplayShippingAddress, request);
            addOptionalValueToRequest("PAYMENTREQUEST_0_TAXAMT", orderDetails.TaxTotal, request);
            addOptionalValueToRequest("PAYMENTREQUEST_0_SHIPPINGAMT", orderDetails.ShippingTotal, request);
            addOptionalValueToRequest("PAYMENTREQUEST_0_SHIPDISCAMT", orderDetails.ShippingDiscount, request);
            addOptionalValueToRequest("PAYMENTREQUEST_0_CUSTOM", orderDetails.CustomField, request);
            addOptionalValueToRequest("PAYMENTREQUEST_0_DESC", orderDetails.OrderDescription, request);

            var lineNumber = 0;
            var itemTotal = 0m;
            var recurringNumber = 0;
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

                    if (line.IsRecurringPayment)
                    {
                        addOptionalValueToRequest("L_BILLINGTYPE" + recurringNumber, "RecurringPayments", request);
                        addOptionalValueToRequest("L_BILLINGAGREEMENTDESCRIPTION" + recurringNumber, line.Description, request);
                        recurringNumber++;
                    }

                    itemTotal += (line.UnitPrice ?? 0) * (line.Quantity ?? 1);
                    lineNumber++;
                }
            }
            if (orderDetails.Discounts != null)
            {
                foreach (var line in orderDetails.Discounts)
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

        public NameValueCollection DoExpressCheckoutPayment(decimal amount, CurrencyCodeType currencyCodeType, string payPalToken, string payPalPayerId)
        {
            var request = getQueryWithCredentials();
            request["METHOD"] = "DoExpressCheckoutPayment";
            request["TOKEN"] = payPalToken;
            request["PAYERID"] = payPalPayerId;
            request["PAYMENTREQUEST_0_AMT"] = amount.AsPayPalFormatString();
            request["PAYMENTREQUEST_0_CURRENCYCODE"] = currencyCodeType.ToString();
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

        public NameValueCollection DoExpressCheckoutPayment(OrderDetails orderDetails, string payPalToken, string payPalPayerId)
        {
            var request = getQueryWithCredentials();

            request["METHOD"] = "DoExpressCheckoutPayment";
            request["TOKEN"] = payPalToken;
            request["PAYERID"] = payPalPayerId;
            request["PAYMENTREQUEST_0_AMT"] = orderDetails.OrderTotal.AsPayPalFormatString();
            request["PAYMENTREQUEST_0_CURRENCYCODE"] = orderDetails.CurrencyCodeType.ToString();
            request["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale";

            addOrderDetailsValues(orderDetails, request);

            return request;
        }

        public NameValueCollection RefundFullTransaction(string transactionId)
        {
            var request = getQueryWithCredentials();
            request["METHOD"] = "RefundTransaction";
            request["TRANSACTIONID"] = transactionId;
            request["REFUNDTYPE"] = "FULL";
            return request;
        }

        public NameValueCollection RefundPartialTransaction(string transactionId, decimal amount, CurrencyCodeType currencyCodeType, string description)
        {
            var request = getQueryWithCredentials();
            request["METHOD"] = "RefundTransaction";
            request["TRANSACTIONID"] = transactionId;
            request["REFUNDTYPE"] = "Partial";
            request["AMT"] = amount.AsPayPalFormatString();
            request["CURRENCYCODE"] = currencyCodeType.ToString();
            request["NOTE"] = description;
            return request;
        }

        public NameValueCollection MassPayment(IEnumerable<PayReceiver> receivers, CurrencyCodeType currencyCodeType, ReceiverType receiverType, string emailSubject)
        {
            var request = getQueryWithCredentials();
            request["METHOD"] = "MassPay";
            request["CURRENCYCODE"] = currencyCodeType.ToString();
            request["RECEIVERTYPE"] = receiverType.ToString();
            addOptionalValueToRequest("EMAILSUBJECT", emailSubject, request);
            addItems(receiverType, receivers, request);
            return request;
        }

        void addItems(ReceiverType receiverType, IEnumerable<PayReceiver> receivers, NameValueCollection request)
        {
            int index = 0;
            foreach (var receiver in receivers)
            {
                if (receiverType == ReceiverType.EmailAddress)
                    request["L_EMAIL" + index] = receiver.ReceiverId;
                else if (receiverType == ReceiverType.UserID)
                    request["L_RECEIVERID" + index] = receiver.ReceiverId;
                else
                    throw new NotImplementedException(string.Format("ReceiverType {0} is not implemented", receiverType));

                request["L_AMT" + index] = receiver.Amount.ToString("0.00");

                addOptionalValueToRequest("L_UNIQUEID" + index, receiver.UniqueId, request);
                addOptionalValueToRequest("L_NOTE" + index, receiver.Note, request);

                index++;
            }
        }

        public NameValueCollection CreateRecurringPaymentsProfile(RecurringProfile profile, string payPalToken)
        {
            var request = getQueryWithCredentials();

            request["METHOD"] = "CreateRecurringPaymentsProfile";
            request["TOKEN"] = payPalToken;
            request["PROFILESTARTDATE"] = profile.StartDate.ToString("yyyy-MM-ddTHH:mm:ss");
            request["DESC"] = profile.Description;
            request["BILLINGPERIOD"] = profile.BillingPeriod.ToString();
            request["BILLINGFREQUENCY"] = profile.BillingFrequency.ToString();
            request["AMT"] = profile.Amount.AsPayPalFormatString();
            request["CURRENCYCODE"] = profile.CurrencyCodeType.ToString();
            request["EMAIL"] = profile.Email;
            request["L_PAYMENTREQUEST_0_ITEMCATEGORY0"] = "Digital";
            request["L_PAYMENTREQUEST_0_NAME0"] = profile.ItemName;
            request["L_PAYMENTREQUEST_0_AMT0"] = profile.Amount.AsPayPalFormatString();
            request["L_PAYMENTREQUEST_0_QTY0"] = "1";

            return request;
        }
    }
}