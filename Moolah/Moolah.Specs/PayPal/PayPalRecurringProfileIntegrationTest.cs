using System;
using System.Collections.Specialized;
using System.Web;
using Machine.Fakes;
using Machine.Specifications;
using Machine.Specifications.Model;
using Moolah.PayPal;

namespace Moolah.Specs.PayPal
{
    [Behaviors]
    public class SuccesfulRecurringProfileCreationBehavior
    {
        It should_get_a_successful_response = () =>
            Response.Status.ShouldEqual(PaymentStatus.Pending);

        It should_not_specify_a_failure_message = () =>
            Response.FailureMessage.ShouldBeNull();

        It should_get_a_checkout_token = () =>
            Response.PayPalToken.ShouldNotBeEmpty();

        It should_provide_a_redirect_url = () =>
        {
            Response.RedirectUrl.ShouldNotBeEmpty();
            System.Diagnostics.Debug.WriteLine("Redirect to: " + Response.RedirectUrl);
        };

        protected static PayPalExpressCheckoutToken Response;
    }

    [Subject(typeof(PayPalExpressCheckout))]
    public class When_starting_express_checkout_with_single_subscription : ExpressCheckoutContext
    {
        Behaves_like<SuccessfulExpressCheckoutBehavior> a_successful_express_checkout;
        Establish context = () =>
                OrderDetails = new OrderDetails
                                   {
                                       OrderDescription = "Some order",
                                       Items = new[]
                                                   {
                                                       new OrderDetailsItem
                                                           {
                                                               Description = "Subscription",
                                                               Name = "SBC",
                                                               Number = 1,
                                                               Quantity = 1,
                                                               UnitPrice = 0.0m, 
                                                               ItemUrl = "http://localhost/subscription?1",
                                                               IsRecurrentPayment = true
                                                           }
                                                   },
                                       ShippingTotal = 0m,
                                       OrderTotal = 0m
                                   };
    }

    [Subject(typeof(PayPalExpressCheckout))]
    public class When_starting_express_checkout_with_items_and_subscription : ExpressCheckoutContext
    {
        Behaves_like<SuccessfulExpressCheckoutBehavior> a_successful_express_checkout;
        Establish context = () =>
                OrderDetails = new OrderDetails
                {
                    OrderDescription = "Some order",
                    Items = new[]
                                                   {
                                                       new OrderDetailsItem
                                                           {
                                                               Description = "First Item",
                                                               Name = "FIRST",
                                                               Number = 1,
                                                               Quantity = 2,
                                                               UnitPrice = 1.99m,
                                                               ItemUrl = "http://localhost/product?1"
                                                           },
                                                       new OrderDetailsItem
                                                           {
                                                               Description = "Second Item",
                                                               Name = "2ND",
                                                               Number = 2,
                                                               Quantity = 1,
                                                               UnitPrice = 11m, 
                                                               ItemUrl = "http://localhost/product?2"
                                                           },
                                                   
                                                       new OrderDetailsItem
                                                           {
                                                               Description = "Subscription",
                                                               Name = "SBC",
                                                               Number = 1,
                                                               Quantity = 1,
                                                               UnitPrice =0.0m, 
                                                               ItemUrl = "http://localhost/subscription?1",
                                                               IsRecurrentPayment = true
                                                           }
                                                   },
                    ShippingTotal = 2m,
                    OrderTotal = 16.98m 
                };
    }

    [Subject(typeof(PayPalExpressCheckout))]
    public class When_starting_express_checkout_with_multiple_subscriptions : ExpressCheckoutContext
    {
        Behaves_like<SuccessfulExpressCheckoutBehavior> a_successful_express_checkout;
        Establish context = () =>
                OrderDetails = new OrderDetails
                {
                    OrderDescription = "Some order",
                    Items = new[]
                                                   {
                                                       new OrderDetailsItem
                                                           {
                                                               Description = "Subscription",
                                                               Name = "SBC",
                                                               Number = 1,
                                                               Quantity = 1,
                                                               UnitPrice = 0.0m, 
                                                               ItemUrl = "http://localhost/subscription?1",
                                                               IsRecurrentPayment = true
                                                           },
                                                           new OrderDetailsItem
                                                           {
                                                               Description = "Subscription2",
                                                               Name = "SBC2",
                                                               Number = 1,
                                                               Quantity = 1,
                                                               UnitPrice = 10.0m, 
                                                               ItemUrl = "http://localhost/subscription?2",
                                                               IsRecurrentPayment = true
                                                           }
                                                   },
                    ShippingTotal = 0m,
                    OrderTotal = 10m
                };
    }
}