using System.Collections.Generic;

namespace Moolah.PayPal
{
    public class OrderDetails
    {
        /// <summary>
        /// If you pass the generic order description parameter along with any two of the following line-item parameters, the order description value does not display.
        /// <see cref="OrderDetailsItem.Name"/>
        /// <see cref="OrderDetailsItem.Number"/>
        /// <see cref="OrderDetailsItem.Description"/>
        /// </summary>
        public string OrderDescription { get; set; }

        public IEnumerable<OrderDetailsItem> Items { get; set; }

        /// <summary>
        /// Sum of tax for all items in this order.
        /// </summary>
        public decimal? TaxTotal { get; set; }
        /// <summary>
        /// Total shipping cost for this order. PayPal calculates the sum of the shipping cost and the handling cost.
        /// Although you may change the value later, try to pass in a shipping amount that is reasonably accurate. 
        /// If you specify a value then you must also specify <see cref="ItemTotal"/>.
        /// </summary>
        public decimal? ShippingTotal { get; set; }
        /// <summary>
        /// Shipping discount for this order. You specify this value as a negative number.
        /// </summary>
        public decimal? ShippingDiscount { get; set; }
        /// <summary>
        /// Total cost of the transaction to the buyer. If shipping cost and tax charges are known, include them in this value. If not, this value should be the current sub-total of the order. 
        /// </summary>
        public decimal OrderTotal { get; set; }
        /// <summary>
        /// Special Instructions to Merchant.
        /// You can allow the buyer to send you special instructions about an order. This feature is especially helpful to buyers who want to customize merchandise. 
        /// A buyer also might want to tell you to ship their order at a later date because they are out of the country.
        /// Note: Users of this feature should be sure to read the instructions the buyer sends.
        /// This feature appears as the link on the Review your information page. When the buyer clicks Add, a Note to seller text box opens in which the 
        /// buyer can enter special instructions to the merchant and click Save. The instructions are returned in the responses to GetExpressCheckoutDetails and DoExpressCheckoutPayment.
        /// </summary>
        public bool? AllowNote { get; set; }
    }

    public class OrderDetailsItem
    {
        /// <summary>
        /// Item name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Item number.
        /// </summary>
        public int? Number { get; set; }
        /// <summary>
        /// Item description.
        /// The OrderDescription field still exists for backwards compatibility. However, Description 
        /// enables you to provide a more precise description for each different item purchased, such as hiking boots or cooking utensils 
        /// rather than one general purpose description such as camping supplies.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Item unit price. PayPal calculates the product of the item unit price and item unit quantity (below) in the Amount column 
        /// of the cart review area. The item unit price can be a positive or a negative value, but not 0. You may provide a negative 
        /// value to reflect a discount on an order, for example.
        /// </summary>
        public decimal? UnitPrice { get; set; }

        public decimal? Tax { get; set; }

        /// <summary>
        /// Item unit quantity.
        /// </summary>
        public int? Quantity { get; set; }

        public string ItemUrl { get; set; }
    }
}
