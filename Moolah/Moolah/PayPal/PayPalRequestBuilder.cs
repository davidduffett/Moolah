using System;
using System.Collections.Specialized;
using System.Web;

namespace Moolah.PayPal
{
    public interface IPayPalRequestBuilder
    {
        NameValueCollection SetExpressCheckout(decimal amount, string cancelUrl, string confirmationUrl);
        NameValueCollection GetExpressCheckoutDetails(string payPalToken);
        NameValueCollection DoExpressCheckoutPayment(decimal amount, string payPalToken, string payPalPayerId);
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
            var request = getQueryWithCredentials();
            request["METHOD"] = "SetExpressCheckout";
            request["PAYMENTREQUEST_0_AMT"] = amount.ToString("0.00");
            request["PAYMENTREQUEST_0_CURRENCYCODE"] = "GBP";
            request["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale";
            request["cancelUrl"] = cancelUrl;
            request["returnUrl"] = confirmationUrl;
            return request;
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
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["VERSION"] = "78";
            queryString["USER"] = _configuration.UserId;
            queryString["PWD"] = _configuration.Password;
            queryString["SIGNATURE"] = _configuration.Signature;
            return queryString;
        }
    }
}