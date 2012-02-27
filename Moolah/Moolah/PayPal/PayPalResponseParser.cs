using System;
using System.Collections.Specialized;
using System.Web;

namespace Moolah.PayPal
{
    public interface IPayPalResponseParser
    {
        PayPalExpressCheckoutToken SetExpressCheckout(NameValueCollection payPalResponse);
        PayPalExpressCheckoutDetails GetExpressCheckoutDetails(NameValueCollection payPalResponse);
        PayPalPaymentResponse DoExpressCheckoutPayment(NameValueCollection payPalResponse);
    }

    public class PayPalResponseParser : IPayPalResponseParser
    {
        private readonly PayPalConfiguration _configuration;

        public PayPalResponseParser(PayPalConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            _configuration = configuration;
        }

        public PayPalExpressCheckoutToken SetExpressCheckout(NameValueCollection payPalResponse)
        {
            var response = new PayPalExpressCheckoutToken(payPalResponse);

            parsePayPalAck(payPalResponse,
                success: () =>
                {
                    response.Status = PaymentStatus.Pending;
                    response.PayPalToken = payPalResponse["TOKEN"];
                    response.RedirectUrl = string.Format(_configuration.CheckoutUrlFormat,
                                                         HttpUtility.UrlEncode(response.PayPalToken));
                },
                fail: message =>
                {
                    response.Status = PaymentStatus.Failed;
                    response.IsSystemFailure = true;
                    response.FailureMessage = message;
                });

            return response;
        }

        public PayPalExpressCheckoutDetails GetExpressCheckoutDetails(NameValueCollection payPalResponse)
        {
            return new PayPalExpressCheckoutDetails(payPalResponse)
                       {
                           CustomerPhoneNumber = payPalResponse["PHONENUM"],
                           CustomerMarketingEmail = payPalResponse["BUYERMARKETINGEMAIL"],
                           PayPalEmail = payPalResponse["EMAIL"],
                           PayPalPayerId = payPalResponse["PAYERID"],
                           CustomerTitle = payPalResponse["SALUTATION"],
                           CustomerFirstName = payPalResponse["FIRSTNAME"],
                           CustomerLastName = payPalResponse["LASTNAME"],
                           DeliveryName = payPalResponse["PAYMENTREQUEST_0_SHIPTONAME"],
                           DeliveryStreet1 = payPalResponse["PAYMENTREQUEST_0_SHIPTOSTREET"],
                           DeliveryStreet2 = payPalResponse["PAYMENTREQUEST_0_SHIPTOSTREET2"],
                           DeliveryCity = payPalResponse["PAYMENTREQUEST_0_SHIPTOCITY"],
                           DeliveryState = payPalResponse["PAYMENTREQUEST_0_SHIPTOSTATE"],
                           DeliveryPostcode = payPalResponse["PAYMENTREQUEST_0_SHIPTOZIP"],
                           DeliveryCountryCode = payPalResponse["PAYMENTREQUEST_0_SHIPTOCOUNTRYCODE"],
                           DeliveryPhoneNumber = payPalResponse["PAYMENTREQUEST_0_SHIPTOPHONENUM"]
                       };
        }

        public PayPalPaymentResponse DoExpressCheckoutPayment(NameValueCollection payPalResponse)
        {
            var response = new PayPalPaymentResponse(payPalResponse);

            parsePayPalAck(payPalResponse, 
                success: () =>
                {
                    response.TransactionReference = payPalResponse["PAYMENTINFO_0_TRANSACTIONID"];

                    var rawPaymentStatus = payPalResponse["PAYMENTINFO_0_PAYMENTSTATUS"];
                    PayPalPaymentStatus paymentStatus;
                    if (Enum.TryParse(rawPaymentStatus, true, out paymentStatus))
                    {
                        response.Status = paymentStatus == PayPalPaymentStatus.Pending
                                              ? PaymentStatus.Pending
                                              : PaymentStatus.Successful;
                    }
                    else
                    {
                        response.Status = PaymentStatus.Failed;
                        response.FailureMessage = "An error occurred processing your PayPal payment.";
                    }
                },
                fail: message =>
                {
                    response.Status = PaymentStatus.Failed;
                    response.IsSystemFailure = true;
                    response.FailureMessage = message;
                });

            return response;
        }

        private static void parsePayPalAck(NameValueCollection payPalResponse, Action success, Action<string> fail)
        {
            PayPalAck payPalStatus;
            if (!Enum.TryParse(payPalResponse["ACK"], true, out payPalStatus))
                throw new InvalidOperationException("Invalid PayPal ACK value: " + payPalResponse["ACK"]);

            switch (payPalStatus)
            {
                case PayPalAck.Success:
                case PayPalAck.SuccessWithWarning:
                    success();
                    break;
                default:
                    var failureMessage = string.Format(
                        "PayPal error code: {0}\n" +
                        "Short message: {1}\n" +
                        "Long message: {2}",
                        payPalResponse["L_ERRORCODE0"], payPalResponse["L_SHORTMESSAGE0"], payPalResponse["L_LONGMESSAGE0"]);
                    fail(failureMessage);
                    break;
            }
        }
    }
}