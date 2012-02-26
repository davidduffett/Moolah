using System;
using System.Web;

namespace Moolah.PayPal
{
    public interface ISetExpressCheckoutResponseParser
    {
        PayPalExpressCheckoutToken Parse(string payPalResponse);
    }

    public class SetExpressCheckoutResponseParser : ISetExpressCheckoutResponseParser
    {
        private readonly PayPalConfiguration _configuration;

        public SetExpressCheckoutResponseParser(PayPalConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            _configuration = configuration;
        }

        public PayPalExpressCheckoutToken Parse(string payPalResponse)
        {
            var response = new PayPalExpressCheckoutToken(payPalResponse);

            var decodedResponse = HttpUtility.ParseQueryString(payPalResponse);
            PayPalStatus payPalStatus;
            if (!Enum.TryParse(decodedResponse["ACK"], true, out payPalStatus))
                throw new InvalidOperationException("Invalid PayPal ACK value: " + decodedResponse["ACK"]);

            switch (payPalStatus)
            {
                case PayPalStatus.Success:
                case PayPalStatus.SuccessWithWarning:
                    response.Status = PaymentStatus.Pending;
                    response.PayPalToken = decodedResponse["TOKEN"];
                    response.RedirectUrl = string.Format(_configuration.CheckoutUrlFormat,
                                                         HttpUtility.UrlEncode(response.PayPalToken));
                    break;
                default:
                    response.Status = PaymentStatus.Failed;
                    response.IsSystemFailure = true;
                    response.FailureMessage = string.Format(
                        "PayPal error code: {0}\n" +
                        "Short message: {1}\n" +
                        "Long message: {2}", 
                        decodedResponse["L_ERRORCODE0"], decodedResponse["L_SHORTMESSAGE0"], decodedResponse["L_LONGMESSAGE0"]);
                    break;
            }

            return response;
        }
    }
}