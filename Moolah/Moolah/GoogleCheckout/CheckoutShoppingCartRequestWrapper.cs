using System;
using System.Collections.Generic;
using GCheckout;
using GCheckout.Checkout;

namespace Moolah.GoogleCheckout
{
    /// <summary>
    /// The GCheckout class does not provide access to items, and is therefore very
    /// difficult to test.
    /// </summary>
    public class CheckoutShoppingCartRequestWrapper : CheckoutShoppingCartRequest
    {
        private readonly List<GCheckout.Checkout.ShoppingCartItem> _items = new List<GCheckout.Checkout.ShoppingCartItem>();

        protected CheckoutShoppingCartRequestWrapper()
            : base("test", "test", EnvironmentType.Sandbox, "test", 0)
        {
        }

        public CheckoutShoppingCartRequestWrapper(string MerchantID, string MerchantKey, EnvironmentType Env, string Currency, int CartExpirationMinutes) 
            : base(MerchantID, MerchantKey, Env, Currency, CartExpirationMinutes)
        {
        }

        public void AddItem(ShoppingCartItem item)
        {
            _items.Add(AddItem(item.Name, item.Description, item.MerchantItemId, item.UnitPrice ?? 0, item.Quantity));
        }

        public void AddDiscount(ShoppingCartDiscount discount)
        {
            _items.Add(AddItem(discount.Name, discount.Description, -Math.Abs(discount.Amount), discount.Quantity ?? 1));
        }

        public IEnumerable<GCheckout.Checkout.ShoppingCartItem> Items
        {
            get { return _items; }
        }
    }
}
