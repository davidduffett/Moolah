using System.Collections.Generic;

namespace Moolah.GoogleCheckout
{
    public class ShoppingCart
    {
        public IEnumerable<ShoppingCartItem> Items { get; set; }
        public IEnumerable<ShoppingCartDiscount> Discounts { get; set; } 
    }

    public class ShoppingCartItem
    {
        /// <summary>
        /// Item name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Item description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Item unit price.
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// The tax included in the item unit price.  Amounts must be sent to Google ex-tax.
        /// </summary>
        public decimal Tax { get; set; }
        /// <summary>
        /// Item unit quantity.
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// The merchant identifier for the item.
        /// <remarks>Optional.</remarks>
        /// </summary>
        public string MerchantItemId { get; set; }
    }

    public class ShoppingCartDiscount
    {
        /// <summary>
        /// Name of the discount item.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description of the discount item.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Optional number of times to apply this discount, eg. a per-item discount.
        /// </summary>
        public int? Quantity { get; set; }
        /// <summary>
        /// Amount of the discount, can be specified as either negative or positive values but will always be deducted from the order value.
        /// If a quantity is specified then the discount will be applied that many times.
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// The tax included in the discount amount.  Amounts must be sent to Google ex-tax.
        /// </summary>
        public decimal Tax { get; set; }
    }
}