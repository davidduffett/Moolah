using System.Collections.Generic;
using System.Linq;
using GCheckout.AutoGen;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.GoogleCheckout;
using ShippingRestrictions = GCheckout.Checkout.ShippingRestrictions;

namespace Moolah.Specs.GoogleCheckout
{
    public abstract class ShippingRestrictionsContext : WithFakes
    {
        Establish context = () =>
            ShippingMethod = new ShippingMethod();

        Because of = () =>
            ShippingRestrictions = ShippingMethod.ToShippingRestrictions();

        protected static ShippingMethod ShippingMethod;
        protected static ShippingRestrictions ShippingRestrictions;
    }

    [Subject(typeof(ShippingMethodExtensions))]
    public class When_getting_shipping_restrictions_from_shipping_method_with_no_areas : ShippingRestrictionsContext
    {
        It should_apply_to_the_entire_world = () =>
            ShippingRestrictions.XmlRestrictions.allowedareas.Items[0].ShouldBeOfType<WorldArea>();

        It should_have_no_excluded_areas = () =>
            ShippingRestrictions.XmlRestrictions.excludedareas.Items.Count().ShouldEqual(0);
    }

    [Subject(typeof(ShippingMethodExtensions))]
    public class When_getting_shipping_restrictions_from_shipping_method_with_allowed_areas : ShippingRestrictionsContext
    {
        It should_apply_only_to_those_areas = () =>
        {
            var postalArea = ShippingRestrictions.XmlRestrictions.allowedareas.Items[0] as PostalArea;
            postalArea.countrycode.ShouldEqual("UK");
            postalArea.postalcodepattern.ShouldEqual("ED");
        };

        Establish context = () =>
            ShippingMethod.AllowedPostalAreas = new List<PostalArea> { new PostalArea { countrycode = "UK", postalcodepattern = "ED" } };
    }

    [Subject(typeof(ShippingMethodExtensions))]
    public class When_getting_shipping_restrictions_from_shipping_method_with_excluded_areas : ShippingRestrictionsContext
    {
        It should_not_apply_to_those_areas = () =>
        {
            var postalArea = ShippingRestrictions.XmlRestrictions.excludedareas.Items[0] as PostalArea;
            postalArea.countrycode.ShouldEqual("UK");
            postalArea.postalcodepattern.ShouldEqual("ED");
        };

        Establish context = () =>
            ShippingMethod.ExcludedPostalAreas = new List<PostalArea> { new PostalArea { countrycode = "UK", postalcodepattern = "ED" } };
    }

    [Subject(typeof(ShippingMethodExtensions))]
    public class When_getting_shipping_restrictions_from_shipping_method_with_allowed_and_excluded_areas : ShippingRestrictionsContext
    {
        It should_apply_to_the_allowed_areas = () =>
        {
            var postalArea = ShippingRestrictions.XmlRestrictions.allowedareas.Items[0] as PostalArea;
            postalArea.countrycode.ShouldEqual("UK");
            postalArea.postalcodepattern.ShouldEqual("ED");
        };

        It should_not_apply_to_the_excluded_areas = () =>
        {
            var postalArea = ShippingRestrictions.XmlRestrictions.excludedareas.Items[0] as PostalArea;
            postalArea.countrycode.ShouldEqual("UK");
            postalArea.postalcodepattern.ShouldEqual("W4");
        };

        Establish context = () =>
        {
            ShippingMethod.AllowedPostalAreas = new List<PostalArea> { new PostalArea { countrycode = "UK", postalcodepattern = "ED" } };
            ShippingMethod.ExcludedPostalAreas = new List<PostalArea> { new PostalArea { countrycode = "UK", postalcodepattern = "W4" } };
        };
    }
}