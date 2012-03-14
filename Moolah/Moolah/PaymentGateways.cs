using Moolah.GoogleCheckout;
using Moolah.PayPal;

namespace Moolah
{
    public interface IPaymentGateway
    {
        /// <summary>
        /// Attempts to transact the specified amount using the card details provided.
        /// </summary>
        /// <param name="merchantReference">The merchant's reference for the transaction.</param>
        /// <param name="amount">The amount to transact.</param>
        /// <param name="card">Credit or debit card details.</param>
        ICardPaymentResponse Payment(string merchantReference, decimal amount, CardDetails card);
    }

    public interface I3DSecurePaymentGateway
    {
        /// <summary>
        /// Attempts to make a card payment.  If the card is enrolled in 3D-Secure, then the Access Control Server URL (ACSUrl)
        /// and Payer Authentication Request (PARes) are returned for the calling site to redirect or show in an iframe.
        /// This should then be followed by a call to <see cref="Authorise"/> with the resulting PARes returned by the ACS.
        /// </summary>
        /// <param name="merchantReference">The merchant's reference for the transaction.</param>
        /// <param name="amount">The amount to transact.</param>
        /// <param name="card">Card details.</param>
        I3DSecureResponse Payment(string merchantReference, decimal amount, CardDetails card);

        /// <summary>
        /// Attempts to authorise a 3D-Secure transaction previously submitted to the <see cref="Payment"/> method.
        /// </summary>
        /// <param name="transactionReference">Transaction reference returned by the Gateway for the original 3D-Secure payment request.</param>
        /// <param name="PARes">Payer Authentication Response (PARes) returned by the bank in response to the user entering their 3D-Secure credentials.</param>
        I3DSecureResponse Authorise(string transactionReference, string PARes);
    }

    public interface IPayPalExpressCheckout
    {
        /// <summary>
        /// Starts a PayPal express checkout session by providing a token and URL
        /// for the user to be redirected to.
        /// </summary>
        /// <param name="amount">Transaction amount.</param>
        /// <param name="cancelUrl">URL to return to if the customer cancels the checkout process.</param>
        /// <param name="confirmationUrl">URL to return to for the customer to confirm payment and place the order.</param>
        /// <returns>
        /// A PayPal token for the express checkout and URL to redirect the customer to.
        /// When the customer navigates to the confirmationUrl, you should then call
        /// <see cref="GetExpressCheckoutDetails"/> to retrieve details about the express checkout.
        /// </returns>
        PayPalExpressCheckoutToken SetExpressCheckout(decimal amount, string cancelUrl, string confirmationUrl);

        /// <summary>
        /// Starts a PayPal express checkout session by providing a token and URL
        /// for the user to be redirected to.
        /// </summary>
        /// <param name="orderDetails">Detailed information on the order this checkout is for.</param>
        /// <param name="cancelUrl">URL to return to if the customer cancels the checkout process.</param>
        /// <param name="confirmationUrl">URL to return to for the customer to confirm payment and place the order.</param>
        /// <returns>
        /// A PayPal token for the express checkout and URL to redirect the customer to.
        /// When the customer navigates to the confirmationUrl, you should then call
        /// <see cref="GetExpressCheckoutDetails"/> to retrieve details about the express checkout.
        /// </returns>
        PayPalExpressCheckoutToken SetExpressCheckout(OrderDetails orderDetails, string cancelUrl, string confirmationUrl);

        /// <summary>
        /// Retrieves information about the express checkout required to carry out authorisation of payment.
        /// </summary>
        /// <param name="payPalToken">The PayPal token returned in the initial <see cref="SetExpressCheckout"/> call.</param>
        /// <returns>
        /// Details about the express checkout, such as the customer details, and PayPal PayerId, which 
        /// must be passed to <see cref="DoExpressCheckoutPayment"/> to perform the payment.
        /// </returns>
        PayPalExpressCheckoutDetails GetExpressCheckoutDetails(string payPalToken);

        /// <summary>
        /// Performs the payment.
        /// </summary>
        /// <param name="amount">Transaction amount.  You may have adjusted the amount depending on the delivery options the customer specified in PayPal.</param>
        /// <param name="payPalToken">The PayPal token returned in the initial <see cref="SetExpressCheckout"/> call.</param>
        /// <param name="payPalPayerId">The PayPal PayerID returned in the <see cref="GetExpressCheckoutDetails"/> call.</param>
        PayPalPaymentResponse DoExpressCheckoutPayment(decimal amount, string payPalToken, string payPalPayerId);
    }

    public interface IGoogleCheckout
    {
        /// <summary>
        /// Returns the image for the Google Checkout button.  This URL needs to include
        /// configuration related information, such as whether we are in the Sandbox or Live, 
        /// and the merchant Id.
        /// See http://code.google.com/apis/checkout/developer/Google_Checkout_XML_API.html#google_checkout_buttons
        /// </summary>
        string GoogleCheckoutButtonImage(ButtonSize size, ButtonStyle style);

        /// <summary>
        /// Requests to start a Google Checkout.
        /// </summary>
        /// <param name="shoppingCart">Details of items and discounts included in the shopping cart.</param>
        /// <param name="options">Options for the checkout request, such as the edit cart and continue shopping URLs.</param>
        /// <returns>
        /// A Google Checkout URL to redirect the customer to.
        /// </returns>
        GoogleCheckoutRedirect RequestCheckout(ShoppingCart shoppingCart, CheckoutOptions options);
    }
}
