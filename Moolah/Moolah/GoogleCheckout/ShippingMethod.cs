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

            if ((shippingMethod.AllowedPostalAreas == null || !shippingMethod.AllowedPostalAreas.Any()) &&
                (shippingMethod.ExcludedPostalAreas == null || !shippingMethod.ExcludedPostalAreas.Any()))
            {
                // Entire world
                restrictions.AddAllowedWorldArea();
                return restrictions;
            }

            if (shippingMethod.AllowedPostalAreas != null)
                foreach (var area in shippingMethod.AllowedPostalAreas)
                    restrictions.AddAllowedPostalArea(area.countrycode, area.postalcodepattern);

            if (shippingMethod.ExcludedPostalAreas != null)
                foreach (var area in shippingMethod.ExcludedPostalAreas)
                    restrictions.AddExcludedPostalArea(area.countrycode, area.postalcodepattern);

            return restrictions;
        }
    }
}