using System.Collections.Specialized;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.PayPal;

namespace Moolah.Specs.PayPal
{
    public abstract class PayPalRequestBuilderContext : WithFakes
    {
        Establish context = () =>
        {
            Configuration = new PayPalConfiguration(PaymentEnvironment.Test, "testUser", "testpassword", "testsignature");
            SUT = new PayPalRequestBuilder(Configuration);
        };

        protected static PayPalRequestBuilder SUT;
        protected static NameValueCollection Request;
        protected static PayPalConfiguration Configuration;
        protected const string CancelUrl = "http://yoursite.com/paypalconfirm";
        protected const string ConfirmationUrl = "http://yoursite.com/basket";
    }

    [Behaviors]
    public class PayPalCommonRequestBehavior
    {
        It should_specify_api_version_78 = () =>
            Request["VERSION"].ShouldEqual("78");

        It should_specify_user_from_configuration = () =>
            Request["USER"].ShouldEqual(Configuration.UserId);

        It should_specify_password_from_configuration = () =>
            Request["PWD"].ShouldEqual(Configuration.Password);

        It should_specify_signature_from_configuration = () =>
            Request["SIGNATURE"].ShouldEqual(Configuration.Signature);

        protected static NameValueCollection Request;
        protected static PayPalConfiguration Configuration;
    }

    [Subject(typeof(PayPalRequestBuilder))]
    public class When_building_a_request_with_values_that_must_be_url_encoded
    {
        It should_url_encode_the_values_when_converted_to_a_string = () =>
            request.ToString().ShouldNotContain('?');

        Because of = () =>
            request = SUT.SetExpressCheckout(1, "http://localhost/?something=2&somethingelse=1",
                                    "http://localhost/?something=2&somethingelse=1");
        Establish context = () =>
            SUT = new PayPalRequestBuilder(new PayPalConfiguration(PaymentEnvironment.Test));

        static PayPalRequestBuilder SUT;
        static NameValueCollection request;
    }

    [Subject(typeof(PayPalRequestBuilder))]
    public class When_building_set_express_checkout_request_for_an_amount : PayPalRequestBuilderContext
    {
        Behaves_like<PayPalCommonRequestBehavior> a_paypal_nvp_request;

        It should_specify_correct_method = () =>
            Request["METHOD"].ShouldEqual("SetExpressCheckout");

        It should_specify_formatted_amount = () =>
            Request["PAYMENTREQUEST_0_AMT"].ShouldEqual(Amount.ToString("0.00"));

        It should_specify_currency_code = () =>
            Request["PAYMENTREQUEST_0_CURRENCYCODE"].ShouldEqual("GBP");

        It should_specify_sale_payment_action = () =>
            Request["PAYMENTREQUEST_0_PAYMENTACTION"].ShouldEqual("Sale");

        It should_specify_cancel_url = () =>
            Request["cancelUrl"].ShouldEqual(CancelUrl);

        It should_specify_return_url = () =>
            Request["returnUrl"].ShouldEqual(ConfirmationUrl);

        Because of = () =>
            Request = SUT.SetExpressCheckout(Amount, CancelUrl, ConfirmationUrl);

        const decimal Amount = 12.99m;
    }

    [Subject(typeof(PayPalRequestBuilder))]
    public class When_building_set_express_checkout_request_with_order_details : PayPalRequestBuilderContext
    {
        Behaves_like<PayPalCommonRequestBehavior> a_paypal_nvp_request;

        It should_specify_the_tax_value_for_the_order = () =>
            Request["PAYMENTREQUEST_0_TAXAMT"].ShouldEqual(OrderDetails.TaxTotal.AsPayPalFormatString());

        It should_specify_the_shipping_total = () =>
            Request["PAYMENTREQUEST_0_SHIPPINGAMT"].ShouldEqual(OrderDetails.ShippingTotal.AsPayPalFormatString());

        It should_specify_the_shipping_discount = () =>
            Request["PAYMENTREQUEST_0_SHIPDISCAMT"].ShouldEqual(OrderDetails.ShippingDiscount.AsPayPalFormatString());

        It should_specify_the_order_total = () =>
            Request["PAYMENTREQUEST_0_AMT"].ShouldEqual(OrderDetails.OrderTotal.AsPayPalFormatString());

        It should_specify_the_order_description = () =>
            Request["PAYMENTREQUEST_0_DESC"].ShouldEqual(OrderDetails.OrderDescription);

        It should_include_each_line_description = () =>
            {
                Request["L_PAYMENTREQUEST_0_DESC0"].ShouldEqual(OrderDetails.Items.First().Description);
                Request["L_PAYMENTREQUEST_0_DESC1"].ShouldEqual(OrderDetails.Items.Last().Description);
            };

        It should_include_each_line_name = () =>
        {
            Request["L_PAYMENTREQUEST_0_NAME0"].ShouldEqual(OrderDetails.Items.First().Name);
            Request["L_PAYMENTREQUEST_0_NAME1"].ShouldEqual(OrderDetails.Items.Last().Name);
        };

        It should_include_each_line_number = () =>
        {
            Request["L_PAYMENTREQUEST_0_NUMBER0"].ShouldEqual(OrderDetails.Items.First().Number.ToString());
            Request["L_PAYMENTREQUEST_0_NUMBER1"].ShouldEqual(OrderDetails.Items.Last().Number.ToString());
        };

        It should_include_item_url_for_lines_where_specified = () =>
            Request["L_PAYMENTREQUEST_0_ITEMURL0"].ShouldEqual(OrderDetails.Items.First().ItemUrl);

        It should_not_include_item_url_for_lines_where_not_specified = () =>
            Request["L_PAYMENTREQUEST_n_ITEMURL1"].ShouldBeNull();

        It should_include_tax_for_each_line = () =>
            {
                Request["L_PAYMENTREQUEST_0_TAXAMT0"].ShouldEqual(OrderDetails.Items.First().Tax.AsPayPalFormatString());
                Request["L_PAYMENTREQUEST_0_TAXAMT1"].ShouldEqual(OrderDetails.Items.Last().Tax.AsPayPalFormatString());
            };

        Because of = () =>
            Request = SUT.SetExpressCheckout(OrderDetails, CancelUrl, ConfirmationUrl);

        static readonly OrderDetails OrderDetails = new OrderDetails
                                                        {
                                                            OrderDescription = "Some order",
                                                            OrderTotal = 100m,
                                                            ShippingDiscount = -7.9m,
                                                            ShippingTotal = 0.54m,
                                                            TaxTotal = 5m,
                                                            Items = new[]
                                                                        {
                                                                            new OrderDetailsItem
                                                                                {
                                                                                    Description = "First Item",
                                                                                    Name = "FIRST",
                                                                                    Number = 1,
                                                                                    Quantity = 3,
                                                                                    UnitPrice = 5.99m,
                                                                                    ItemUrl = "http://localhost/product?123"
                                                                                },
                                                                            new OrderDetailsItem
                                                                                {
                                                                                    Description = "Second Item",
                                                                                    Name = "2ND",
                                                                                    Number = 2,
                                                                                    Quantity = 1,
                                                                                    UnitPrice = 11m
                                                                                }
                                                                        }
                                                        };
    }

    [Subject(typeof(PayPalRequestBuilder))]
    public class When_specifying_line_unit_prices : PayPalRequestBuilderContext
    {
        It should_include_item_total_in_the_request = () =>
            Request["PAYMENTREQUEST_0_ITEMAMT"].ShouldNotBeNull();

        It should_sum_the_unit_price_and_quantity_to_get_the_item_total = () =>
            Request["PAYMENTREQUEST_0_ITEMAMT"].ShouldEqual("28.97");

        It should_include_each_line_quantity = () =>
        {
            Request["L_PAYMENTREQUEST_0_QTY0"].ShouldEqual(OrderDetails.Items.First().Quantity.ToString());
            Request["L_PAYMENTREQUEST_0_QTY1"].ShouldEqual(OrderDetails.Items.Last().Quantity.ToString());
        };

        It should_include_each_line_unit_price = () =>
        {
            Request["L_PAYMENTREQUEST_0_AMT0"].ShouldEqual(OrderDetails.Items.First().UnitPrice.AsPayPalFormatString());
            Request["L_PAYMENTREQUEST_0_AMT1"].ShouldEqual(OrderDetails.Items.Last().UnitPrice.AsPayPalFormatString());
        };

        Because of = () =>
            Request = SUT.SetExpressCheckout(OrderDetails, CancelUrl, ConfirmationUrl);

        Establish context = () =>
            OrderDetails = new OrderDetails
                               {
                                   Items = new[]
                                               {
                                                   new OrderDetailsItem
                                                       {
                                                           Quantity = 3,
                                                           UnitPrice = 5.99m
                                                       },
                                                   new OrderDetailsItem
                                                       {
                                                           Quantity = 1,
                                                           UnitPrice = 11m
                                                       }
                                               }
                               };

        static OrderDetails OrderDetails;
    }

    [Subject(typeof(PayPalRequestBuilder))]
    public class When_specifying_allow_note_to_set_express_checkout : PayPalRequestBuilderContext
    {
        It should_include_it_in_the_request = () =>
            Request["ALLOWNOTE"].ShouldEqual("1");

        Because of = () =>
            Request = SUT.SetExpressCheckout(new OrderDetails { AllowNote = true }, CancelUrl, ConfirmationUrl);
    }

    [Subject(typeof(PayPalRequestBuilder))]
    public class When_not_specifying_allow_note_to_set_express_checkout : PayPalRequestBuilderContext
    {
        It should_include_it_in_the_request = () =>
            Request["ALLOWNOTE"].ShouldEqual("0");

        Because of = () =>
            Request = SUT.SetExpressCheckout(new OrderDetails { AllowNote = false }, CancelUrl, ConfirmationUrl);
    }

    [Subject(typeof(PayPalRequestBuilder))]
    public class When_building_set_express_checkout_request_with_partial_order_details : PayPalRequestBuilderContext
    {
        Behaves_like<PayPalCommonRequestBehavior> a_paypal_nvp_request;

        It should_include_the_specified_values = () =>
            Request["PAYMENTREQUEST_0_AMT"].ShouldEqual(PartialOrderDetails.OrderTotal.AsPayPalFormatString());

        It should_not_include_unspecified_fields = () =>
            Request.AllKeys.ShouldNotContain(new[]
                                                 {
                                                     "PAYMENTREQUEST_0_ITEMAMT",
                                                     "PAYMENTREQUEST_0_TAXAMT",
                                                     "PAYMENTREQUEST_0_SHIPPINGAMT",
                                                     "PAYMENTREQUEST_0_SHIPDISCAMT",
                                                     "ALLOWNOTE",
                                                     "PAYMENTREQUEST_0_DESC"
                                                 });

        It should_not_include_details_for_unspecified_lines = () =>
            Request.AllKeys.Any(x => x.StartsWith("L_PAYMENTREQUEST_0_")).ShouldBeFalse();

        Because of = () =>
            Request = SUT.SetExpressCheckout(PartialOrderDetails, CancelUrl, ConfirmationUrl);

        static readonly OrderDetails PartialOrderDetails = new OrderDetails
        {
            OrderTotal = 100m
        };
        const string CancelUrl = "http://yoursite.com/paypalconfirm";
        const string ConfirmationUrl = "http://yoursite.com/basket";
    }

    [Subject(typeof(PayPalRequestBuilder))]
    public class When_building_get_express_checkout_details_request : PayPalRequestBuilderContext
    {
        Behaves_like<PayPalCommonRequestBehavior> a_paypal_nvp_request;

        It should_specify_correct_method = () =>
            Request["METHOD"].ShouldEqual("GetExpressCheckoutDetails");

        It should_specify_the_paypal_token = () =>
            Request["TOKEN"].ShouldEqual(PayPalToken);

        Because of = () =>
            Request = SUT.GetExpressCheckoutDetails(PayPalToken);

        const string PayPalToken = "tokenValue";
    }

    [Subject(typeof(PayPalRequestBuilder))]
    public class When_building_do_express_checkout_payment_request : PayPalRequestBuilderContext
    {
        Behaves_like<PayPalCommonRequestBehavior> a_paypal_nvp_request;

        It should_specify_correct_method = () =>
            Request["METHOD"].ShouldEqual("DoExpressCheckoutPayment");

        It should_specify_the_paypal_token = () =>
            Request["TOKEN"].ShouldEqual(PayPalToken);

        It should_specify_the_paypal_payer_id = () =>
            Request["PAYERID"].ShouldEqual(PayPalPayerId);

        It should_specify_formatted_amount = () =>
            Request["PAYMENTREQUEST_0_AMT"].ShouldEqual(Amount.ToString("0.00"));

        It should_specify_currency_code = () =>
            Request["PAYMENTREQUEST_0_CURRENCYCODE"].ShouldEqual("GBP");

        It should_specify_sale_payment_action = () =>
           Request["PAYMENTREQUEST_0_PAYMENTACTION"].ShouldEqual("Sale");

        Because of = () =>
            Request = SUT.DoExpressCheckoutPayment(Amount, PayPalToken, PayPalPayerId);

        const string PayPalToken = "tokenValue";
        const string PayPalPayerId = "payerId";
        const decimal Amount = 12.99m;
    }
}