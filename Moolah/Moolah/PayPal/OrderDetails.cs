using System;
using System.Collections.Generic;

namespace Moolah.PayPal
{
    [Serializable]
    public class OrderDetails
    {
        public OrderDetails()
        {
            CurrencyCodeType = CurrencyCodeType.GBP;
            Items = new OrderDetailsItem[0];
            Discounts = new DiscountDetails[0];
        }

        /// <summary>
        /// Currency to use.  Defaults to GBP.
        /// </summary>
        public CurrencyCodeType CurrencyCodeType { get; set; }

        /// <summary>
        /// If you pass the generic order description parameter along with any two of the following line-item parameters, the order description value does not display.
        /// <see cref="OrderDetailsItem.Name"/>
        /// <see cref="OrderDetailsItem.Number"/>
        /// <see cref="OrderDetailsItem.Description"/>
        /// </summary>
        public string OrderDescription { get; set; }

        public IEnumerable<OrderDetailsItem> Items { get; set; }
        public IEnumerable<DiscountDetails> Discounts { get; set; } 

        /// <summary>
        /// Sum of tax for all items in this order.
        /// </summary>
        public decimal? TaxTotal { get; set; }
        /// <summary>
        /// Total shipping cost for this order. PayPal calculates the sum of the shipping cost and the handling cost.
        /// Although you may change the value later, try to pass in a shipping amount that is reasonably accurate. 
        /// If you specify a value then you must also specify <see cref="OrderTotal"/>.
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
        /// <summary>
        /// Enables the buyer to provide an email address to be used for notifying them of promotions or special events.
        /// </summary>
        public bool? EnableCustomerMarketingEmailOptIn { get; set; }
        /// <summary>
        /// A free-form field for your own use. The value you specify is available only if the transaction includes a purchase.
        /// This field is ignored if you set up a billing agreement for a recurring payment that is not immediately charged.
        /// Character length and limitations: 256 single-byte alphanumeric characters
        /// </summary>
        public string CustomField { get; set; }
    }

    [Serializable]
    public class DiscountDetails
    {
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
        /// Tax component of the discount, can be specified as either negative or positive values but will be displayed as negative on the paypal invoice.
        /// </summary>
        public decimal? Tax { get; set; }
    }

    [Serializable]
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
