namespace Moolah.PayPal
{
    public class PayPalExpressCheckout : IPayPalExpressCheckout
    {
        public PayPalExpressCheckoutToken SetExpressCheckout(decimal amount, string cancelUrl, string confirmationUrl)
        {
            throw new System.NotImplementedException();
        }

        public PayPalExpressCheckoutDetails GetExpressCheckoutDetails(string payPalToken)
        {
            throw new System.NotImplementedException();
        }

        public IPaymentResponse DoExpressCheckoutPayment(decimal amount, string payPalToken, string payPalPayerId)
        {
            throw new System.NotImplementedException();
        }
    }
}