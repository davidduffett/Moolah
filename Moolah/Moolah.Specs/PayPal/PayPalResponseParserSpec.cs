using System;
using System.Linq;
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
            var payPalResponse = HttpUtility.ParseQueryString("ACK=Success&PAYMENTREQUEST_0_AMT=5.00&PHONENUM=phone-number&BUYERMARKETINGEMAIL=marketing-email&" +
                "EMAIL=paypal-email&PAYERID=payer-id&SALUTATION=title&FIRSTNAME=first-name&LASTNAME=last-name&" +
                "PAYMENTREQUEST_0_SHIPTONAME=delivery-name&PAYMENTREQUEST_0_SHIPTOSTREET=street-1&PAYMENTREQUEST_0_SHIPTOSTREET2=street-2&" +
                "PAYMENTREQUEST_0_SHIPTOCITY=city&PAYMENTREQUEST_0_SHIPTOSTATE=state&PAYMENTREQUEST_0_SHIPTOZIP=postcode&" +
                "PAYMENTREQUEST_0_SHIPTOCOUNTRYCODE=uk&PAYMENTREQUEST_0_SHIPTOPHONENUM=delivery-phone");
            Response = SUT.GetExpressCheckoutDetails(payPalResponse);
        };

        static PayPalExpressCheckoutDetails Response;
    }

    [Subject(typeof(PayPalResponseParser))]
    public class When_parsing_failed_get_express_checkout_details_response : PayPalResponseParserContext
    {
        It should_throw_an_exception = () =>
            exception.ShouldNotBeNull();

        Because of = () =>
        {
            var payPalResponse = HttpUtility.ParseQueryString(
                "TIMESTAMP=2012%2d02%2d26T16%3a13%3a12Z&CORRELATIONID=ff25e376319b6&ACK=Failure&VERSION=78&BUILD=2571254&L_ERRORCODE0=10002&L_SHORTMESSAGE0=Security%20error&L_LONGMESSAGE0=Security%20header%20is%20not%20valid&L_SEVERITYCODE0=Error");
            exception = Catch.Exception(() => SUT.GetExpressCheckoutDetails(payPalResponse));
        };

        static Exception exception;
    }

    [Subject(typeof(PayPalResponseParser))]
    public class When_parsing_successful_get_express_checkout_details_response_with_order_details : PayPalResponseParserContext
    {
        It should_provide_the_tax_value_for_the_order = () =>
            Response.OrderDetails.TaxTotal.ShouldEqual(5m); //PAYMENTREQUEST_0_TAXAMT

        It should_provide_the_shipping_total = () =>
            Response.OrderDetails.ShippingTotal.ShouldEqual(0.54m); //PAYMENTREQUEST_0_SHIPPINGAMT

        It should_provide_the_shipping_discount = () =>
            Response.OrderDetails.ShippingDiscount.ShouldEqual(-7.9m); //PAYMENTREQUEST_0_SHIPDISCAMT

        It should_provide_the_order_total = () =>
            Response.OrderDetails.OrderTotal.ShouldEqual(100m); //PAYMENTREQUEST_0_AMT

        It should_provide_the_order_description = () =>
            Response.OrderDetails.OrderDescription.ShouldEqual("Some order"); //PAYMENTREQUEST_0_DESC

        It should_provide_custom_field_value = () =>
            Response.OrderDetails.CustomField.ShouldEqual("Custom field"); //PAYMENTREQUEST_0_CUSTOM

        It should_provide_each_line_description = () =>
        {
            Response.OrderDetails.Items.First().Description.ShouldEqual("First Item"); //L_PAYMENTREQUEST_0_DESC0
            Response.OrderDetails.Items.Last().Description.ShouldEqual("Second Item"); //L_PAYMENTREQUEST_0_DESC1
        };

        It should_provide_each_line_name = () =>
        {
            Response.OrderDetails.Items.First().Name.ShouldEqual("FIRST"); //L_PAYMENTREQUEST_0_NAME0
            Response.OrderDetails.Items.Last().Name.ShouldEqual("2ND"); //L_PAYMENTREQUEST_0_NAME1
        };

        It should_provide_each_line_number = () =>
        {
            Response.OrderDetails.Items.First().Number.ShouldEqual(1); //L_PAYMENTREQUEST_0_NUMBER0
            Response.OrderDetails.Items.Last().Number.ShouldEqual(2); //L_PAYMENTREQUEST_0_NUMBER1
        };

        It should_provide_item_url_for_lines_where_specified = () =>
            Response.OrderDetails.Items.First().ItemUrl.ShouldEqual("http://localhost/product?123&navigationid=3"); //L_PAYMENTREQUEST_0_ITEMURL0

        It should_not_provide_item_url_for_lines_where_not_specified = () =>
            Response.OrderDetails.Items.Last().ItemUrl.ShouldBeNull(); //L_PAYMENTREQUEST_0_ITEMURL1

        It should_provide_each_line_tax_amount = () =>
        {
            Response.OrderDetails.Items.First().Tax.ShouldEqual(1.19m); //L_PAYMENTREQUEST_0_TAXAMT0
            Response.OrderDetails.Items.Last().Tax.ShouldEqual(2m); //L_PAYMENTREQUEST_0_TAXAMT1
        };

        It should_provide_each_line_unit_price = () =>
        {
            Response.OrderDetails.Items.First().UnitPrice.ShouldEqual(3.19m); //L_PAYMENTREQUEST_0_AMT0
            Response.OrderDetails.Items.Last().UnitPrice.ShouldEqual(5m); //L_PAYMENTREQUEST_0_AMT1
        };

        It should_provide_each_line_quantity = () =>
        {
            Response.OrderDetails.Items.First().Quantity.ShouldEqual(2); //L_PAYMENTREQUEST_0_QTY0
            Response.OrderDetails.Items.Last().Quantity.ShouldEqual(1); //L_PAYMENTREQUEST_0_QTY1
        };

        It should_provide_each_discount_line_description = () =>
        {
            Response.OrderDetails.Discounts.First().Description.ShouldEqual("Multi-buy discount, -1 per item."); //L_PAYMENTREQUEST_0_NAME2
            Response.OrderDetails.Discounts.Last().Description.ShouldEqual("Loyalty discount"); //L_PAYMENTREQUEST_0_NAME3
        };

        It should_provide_each_discount_line_quantity = () =>
        {
            Response.OrderDetails.Discounts.First().Quantity.ShouldEqual(2); //L_PAYMENTREQUEST_0_QTY2
            Response.OrderDetails.Discounts.Last().Quantity.ShouldEqual(1); //L_PAYMENTREQUEST_0_QTY3
        };

        It should_provide_each_discount_line_amount = () =>
        {
            Response.OrderDetails.Discounts.First().Amount.ShouldEqual(-1.75m); //L_PAYMENTREQUEST_0_AMT2
            Response.OrderDetails.Discounts.Last().Amount.ShouldEqual(-3.25m); //L_PAYMENTREQUEST_0_AMT3
        };

        It should_provide_each_discount_line_tax_amount = () =>
        {
            Response.OrderDetails.Discounts.First().Tax.ShouldEqual(-0.29m); //L_PAYMENTREQUEST_0_TAXAMT2
            Response.OrderDetails.Discounts.Last().Tax.ShouldEqual(-0.63m); //L_PAYMENTREQUEST_0_TAXAMT3
        };

        It should_provide_currency_code = () =>
            Response.OrderDetails.CurrencyCodeType.ShouldEqual(CurrencyCodeType.GBP);

        Because of = () =>
        {
            var payPalResponse = HttpUtility.ParseQueryString(string.Empty);
            payPalResponse.Add("ACK", "Success");
            payPalResponse.Add("PAYMENTREQUEST_0_TAXAMT", "5.00");
            payPalResponse.Add("PAYMENTREQUEST_0_SHIPPINGAMT", "0.54");
            payPalResponse.Add("PAYMENTREQUEST_0_SHIPDISCAMT", "-7.90");
            payPalResponse.Add("PAYMENTREQUEST_0_AMT", "100.00");
            payPalResponse.Add("PAYMENTREQUEST_0_DESC", "Some order");
            payPalResponse.Add("PAYMENTREQUEST_0_CUSTOM", "Custom field");
            payPalResponse.Add("L_PAYMENTREQUEST_0_DESC0", "First Item");
            payPalResponse.Add("L_PAYMENTREQUEST_0_DESC1", "Second Item");
            payPalResponse.Add("L_PAYMENTREQUEST_0_NAME0", "FIRST");
            payPalResponse.Add("L_PAYMENTREQUEST_0_NAME1", "2ND");
            payPalResponse.Add("L_PAYMENTREQUEST_0_NUMBER0", "1");
            payPalResponse.Add("L_PAYMENTREQUEST_0_NUMBER1", "2");
            payPalResponse.Add("L_PAYMENTREQUEST_0_ITEMURL0", "http://localhost/product?123&navigationid=3");
            payPalResponse.Add("L_PAYMENTREQUEST_0_TAXAMT0", "1.19");
            payPalResponse.Add("L_PAYMENTREQUEST_0_TAXAMT1", "2.00");
            payPalResponse.Add("L_PAYMENTREQUEST_0_AMT0", "3.19");
            payPalResponse.Add("L_PAYMENTREQUEST_0_AMT1", "5.00");
            payPalResponse.Add("L_PAYMENTREQUEST_0_QTY0", "2");
            payPalResponse.Add("L_PAYMENTREQUEST_0_QTY1", "1");
            payPalResponse.Add("L_PAYMENTREQUEST_0_NAME2", "Multi-buy discount, -1 per item.");
            payPalResponse.Add("L_PAYMENTREQUEST_0_NAME3", "Loyalty discount");
            payPalResponse.Add("L_PAYMENTREQUEST_0_QTY2", "2");
            payPalResponse.Add("L_PAYMENTREQUEST_0_QTY3", "1");
            payPalResponse.Add("L_PAYMENTREQUEST_0_AMT2", "-1.75");
            payPalResponse.Add("L_PAYMENTREQUEST_0_AMT3", "-3.25");
            payPalResponse.Add("L_PAYMENTREQUEST_0_TAXAMT2", "-0.29");
            payPalResponse.Add("L_PAYMENTREQUEST_0_TAXAMT3", "-0.63");
            payPalResponse.Add("PAYMENTREQUEST_0_CURRENCYCODE", "GBP");
            Response = SUT.GetExpressCheckoutDetails(payPalResponse);
        };

        static PayPalExpressCheckoutDetails Response;
    }

    [Subject(typeof(PayPalResponseParser))]
    public class When_parsing_successful_get_express_checkout_details_response_with_partial_order_details : PayPalResponseParserContext
    {
        It should_provide_the_specified_values = () =>
            Response.OrderDetails.OrderTotal.ShouldEqual(12.45m);

        It should_not_include_unspecified_fields = () =>
        {
            Response.OrderDetails.TaxTotal.ShouldBeNull();
            Response.OrderDetails.ShippingTotal.ShouldBeNull();
            Response.OrderDetails.ShippingDiscount.ShouldBeNull();
            Response.OrderDetails.AllowNote.ShouldBeNull();
            Response.OrderDetails.OrderDescription.ShouldBeNull();
        };

        It should_not_include_item_details_for_unspecified_lines = () =>
            Response.OrderDetails.Items.ShouldBeEmpty();

        It should_not_include_discount_details_for_unspecified_lines = () =>
            Response.OrderDetails.Discounts.ShouldBeEmpty();

        Because of = () =>
        {
            var payPalResponse = HttpUtility.ParseQueryString("ACK=Success&PAYMENTREQUEST_0_AMT=12.45");
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

        static IPaymentResponse Response;
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

        static IPaymentResponse Response;
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

        static IPaymentResponse Response;
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

        static IPaymentResponse Response;
    }
}