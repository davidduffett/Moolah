using GCheckout;
using GCheckout.Checkout;

namespace Moolah.GoogleCheckout
{
    /// <summary>
    /// The GCheckout class does not provide access to items, or set tax rates,
    /// and is therefore very difficult to test.
    /// </summary>
    public class CheckoutShoppingCartRequestWrapper : CheckoutShoppingCartRequest
    {
        protected CheckoutShoppingCartRequestWrapper()
            : base("test", "test", EnvironmentType.Sandbox, "test", 0)
        {
        }

        public CheckoutShoppingCartRequestWrapper(string MerchantID, string MerchantKey, EnvironmentType Env, string Currency, int CartExpirationMinutes) 
            : base(MerchantID, MerchantKey, Env, Currency, CartExpirationMinutes)
        {
        }
    }
}
