using System.Collections.Generic;
using System.Linq;
using GCheckout.AutoGen;

namespace Moolah.GoogleCheckout
{
    public class ShippingMethod
    {
        /// <summary>
        /// The name of the shipping method displayed to the customer.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The price, excluding tax of shipping using this method.
        /// </summary>
        public decimal PriceExTax { get; set; }
        /// <summary>
        /// Postal areas that are allowed for this shipping method.  For example, the entire UK.
        /// Note that if no allowed postal areas are provided, the shipping method will be treated as Worldwide.
        /// </summary>
        public IEnumerable<PostalArea> AllowedPostalAreas { get; set; }
        /// <summary>
        /// Postal areas that are excluded from this shipping method.  For example, just postcodes beginning with "E".
        /// </summary>
        public IEnumerable<PostalArea> ExcludedPostalAreas { get; set; }
    }

    public static class ShippingMethodExtensions
    {
        public static GCheckout.Checkout.ShippingRestrictions ToShippingRestrictions(this ShippingMethod shippingMethod)
        {
            var restrictions = new GCheckout.Checkout.ShippingRestrictions();

            if (shippingMethod.AllowedPostalAreas == null || !shippingMethod.AllowedPostalAreas.Any())
                // Entire world
                restrictions.AddAllowedWorldArea();
            else if (shippingMethod.AllowedPostalAreas != null)
                foreach (var area in shippingMethod.AllowedPostalAreas)
                    restrictions.AddAllowedPostalArea(area.countrycode, area.postalcodepattern);

            if (shippingMethod.ExcludedPostalAreas != null)
                foreach (var area in shippingMethod.ExcludedPostalAreas)
                    restrictions.AddExcludedPostalArea(area.countrycode, area.postalcodepattern);

            return restrictions;
        }
    }
}