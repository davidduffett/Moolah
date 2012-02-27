using System.Web;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.PayPal;

namespace Moolah.Specs.PayPal
{
    public abstract class PayPalResponseParserContext : WithFakes
    {
        Establish context = () =>
            SUT = new PayPalResponseParser(new PayPalConfiguration(PaymentEnvironment.Test));

        protected static PayPalResponseParser SUT;
    }

    [Subject(typeof(PayPalResponseParser))]
    public class When_parsing_successful_set_express_checkout_response : PayPalResponseParserContext
    {
        It should_have_payment_status_of_pending = () =>
            Response.Status.ShouldEqual(PaymentStatus.Pending);

        It should_have_the_correct_token = () =>
            Response.PayPalToken.ShouldEqual("EC-5X481106MU8743929");

        It should_have_the_correct_redirect_url = () =>
            Response.RedirectUrl.ShouldEqual("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=EC-5X481106MU8743929");

        Because of = () =>
        {
            var payPalResponse = HttpUtility.ParseQueryString(
                "TOKEN=EC%2d5X481106MU8743929&TIMESTAMP=2012%2d02%2d26T15%3a03%3a36Z&CORRELATIONID=efc2d0ff5a121&ACK=Success&VERSION=78&BUILD=2571254");
            Response = SUT.SetExpressCheckout(payPalResponse);
        };

        static PayPalExpressCheckoutToken Response;
    }

    [Subject(typeof(PayPalResponseParser))]
    public class When_parsing_failed_set_express_checkout_response : PayPalResponseParserContext
    {
        It should_have_payment_status_of_failed = () =>
            Response.Status.ShouldEqual(PaymentStatus.Failed);

        It should_be_a_system_failure = () =>
            Response.IsSystemFailure.ShouldBeTrue();

        It should_provide_a_failure_message = () =>
            Response.FailureMessage.ShouldNotBeEmpty();

        It should_not_provide_a_redirect_url = () =>
            Response.RedirectUrl.ShouldBeNull();

        Because of = () =>
        {
            var payPalResponse = HttpUtility.ParseQueryString(
                "TIMESTAMP=2012%2d02%2d26T16%3a13%3a12Z&CORRELATIONID=ff25e376319b6&ACK=Failure&VERSION=78&BUILD=2571254&L_ERRORCODE0=10002&L_SHORTMESSAGE0=Security%20error&L_LONGMESSAGE0=Security%20header%20is%20not%20valid&L_SEVERITYCODE0=Error");
            Response = SUT.SetExpressCheckout(payPalResponse);
        };

        static PayPalExpressCheckoutToken Response;
    }

    [Subject(typeof(PayPalResponseParser))]
    public class When_parsing_successful_get_express_checkout_details_response : PayPalResponseParserContext
    {
        It should_provide_customer_phone_number = () =>
            Response.CustomerPhoneNumber.ShouldEqual("phone-number"); //PHONENUM

        It should_provide_customer_marketing_email_address = () =>
            Response.CustomerMarketingEmail.ShouldEqual("marketing-email"); //BUYERMARKETINGEMAIL

        It should_provide_customer_paypal_email_address = () =>
            Response.PayPalEmail.ShouldEqual("paypal-email"); //EMAIL

        It should_provide_paypal_payer_id = () =>
            Response.PayPalPayerId.ShouldEqual("payer-id"); //PAYERID

        It should_provide_customer_title = () =>
            Response.CustomerTitle.ShouldEqual("title"); //SALUTATION

        It should_provide_customer_first_name = () =>
            Response.CustomerFirstName.ShouldEqual("first-name"); //FIRSTNAME

        It should_provide_customer_last_name = () =>
            Response.CustomerLastName.ShouldEqual("last-name"); //LASTNAME

        It should_provide_delivery_address_name = () =>
            Response.DeliveryName.ShouldEqual("delivery-name"); //PAYMENTREQUEST_0_SHIPTONAME

        It should_provide_delivery_address_line1 = () =>
            Response.DeliveryStreet1.ShouldEqual("street-1"); //PAYMENTREQUEST_0_SHIPTOSTREET

        It should_provide_delivery_address_line2 = () =>
            Response.DeliveryStreet2.ShouldEqual("street-2"); //PAYMENTREQUEST_0_SHIPTOSTREET2

        It should_provide_delivery_city = () =>
            Response.DeliveryCity.ShouldEqual("city"); //PAYMENTREQUEST_0_SHIPTOCITY

        It should_provide_delivery_state = () =>
            Response.DeliveryState.ShouldEqual("state"); //PAYMENTREQUEST_0_SHIPTOSTATE

        It should_provide_delivery_postcode = () =>
            Response.DeliveryPostcode.ShouldEqual("postcode"); //PAYMENTREQUEST_0_SHIPTOZIP

        It should_provide_delivery_country_code = () =>
            Response.DeliveryCountryCode.ShouldEqual("uk"); //PAYMENTREQUEST_0_SHIPTOCOUNTRYCODE

        It should_provide_delivery_phone_number = () =>
            Response.DeliveryPhoneNumber.ShouldEqual("delivery-phone"); //PAYMENTREQUEST_0_SHIPTOPHONENUM

        Because of = () =>
        {
            var payPalResponse = HttpUtility.ParseQueryString("PHONENUM=phone-number&BUYERMARKETINGEMAIL=marketing-email&" +
                "EMAIL=paypal-email&PAYERID=payer-id&SALUTATION=title&FIRSTNAME=first-name&LASTNAME=last-name&" +
                "PAYMENTREQUEST_0_SHIPTONAME=delivery-name&PAYMENTREQUEST_0_SHIPTOSTREET=street-1&PAYMENTREQUEST_0_SHIPTOSTREET2=street-2&" +
                "PAYMENTREQUEST_0_SHIPTOCITY=city&PAYMENTREQUEST_0_SHIPTOSTATE=state&PAYMENTREQUEST_0_SHIPTOZIP=postcode&" +
                "PAYMENTREQUEST_0_SHIPTOCOUNTRYCODE=uk&PAYMENTREQUEST_0_SHIPTOPHONENUM=delivery-phone");
            Response = SUT.GetExpressCheckoutDetails(payPalResponse);
        };

        static PayPalExpressCheckoutDetails Response;
    }

    [Subject(typeof(PayPalResponseParser))]
    public class When_parsing_successful_do_express_checkout_payment_response : PayPalResponseParserContext
    {
        It should_have_payment_status_of_successful = () =>
            Response.Status.ShouldEqual(PaymentStatus.Successful);

        It should_provide_paypal_transaction_reference = () =>
            Response.TransactionReference.ShouldEqual("paypal-reference"); //PAYMENTINFO_0_TRANSACTIONID

        Because of = () =>
        {
            var payPalResponse = HttpUtility.ParseQueryString("ACK=Success&PAYMENTINFO_0_TRANSACTIONID=paypal-reference&PAYMENTINFO_0_PAYMENTSTATUS=Completed");
            Response = SUT.DoExpressCheckoutPayment(payPalResponse);
        };

        static PayPalPaymentResponse Response;
    }

    [Subject(typeof(PayPalResponseParser))]
    public class When_parsing_failed_do_express_checkout_payment_response : PayPalResponseParserContext
    {
        It should_have_payment_status_of_failed = () =>
            Response.Status.ShouldEqual(PaymentStatus.Failed);

        It should_be_a_system_failure = () =>
            Response.IsSystemFailure.ShouldBeTrue();

        It should_provide_a_failure_message = () =>
            Response.FailureMessage.ShouldNotBeEmpty();

        It should_not_provide_paypal_transaction_reference = () =>
            Response.TransactionReference.ShouldBeNull();

        Because of = () =>
        {
            var payPalResponse = HttpUtility.ParseQueryString("ACK=Failure&L_ERRORCODE0=10002&L_SHORTMESSAGE0=Security%20error&L_LONGMESSAGE0=Security%20header%20is%20not%20valid&L_SEVERITYCODE0=Error");
            Response = SUT.DoExpressCheckoutPayment(payPalResponse);
        };

        static PayPalPaymentResponse Response;
    }

    [Subject(typeof(PayPalResponseParser))]
    public class When_parsing_do_express_checkout_payment_response_where_payment_is_pending : PayPalResponseParserContext
    {
        It should_have_payment_status_of_pending = () =>
            Response.Status.ShouldEqual(PaymentStatus.Pending);

        It should_provide_paypal_transaction_reference = () =>
            Response.TransactionReference.ShouldEqual("paypal-reference");

        Because of = () =>
        {
            var payPalResponse = HttpUtility.ParseQueryString("ACK=Success&PAYMENTINFO_0_TRANSACTIONID=paypal-reference&PAYMENTINFO_0_PAYMENTSTATUS=Pending&PAYMENTINFO_0_PENDINGREASON=echeck");
            Response = SUT.DoExpressCheckoutPayment(payPalResponse);
        };

        static PayPalPaymentResponse Response;
    }

    [Subject(typeof(PayPalResponseParser))]
    public class When_parsing_do_express_checkout_payment_response_where_payment_failed : PayPalResponseParserContext
    {
        It should_have_payment_status_of_failed = () =>
            Response.Status.ShouldEqual(PaymentStatus.Failed);

        It should_provide_paypal_transaction_reference = () =>
            Response.TransactionReference.ShouldEqual("paypal-reference");

        Because of = () =>
        {
            var payPalResponse = HttpUtility.ParseQueryString("ACK=Success&PAYMENTINFO_0_TRANSACTIONID=paypal-reference&PAYMENTINFO_0_PAYMENTSTATUS=Failed");
            Response = SUT.DoExpressCheckoutPayment(payPalResponse);
        };

        static PayPalPaymentResponse Response;
    }
}