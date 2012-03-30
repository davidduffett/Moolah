using System.Globalization;
using Machine.Fakes;
using Machine.Specifications;
using Moolah.PayPal;

namespace Moolah.Specs.PayPal
{
    [Subject(typeof(LocaleCodes))]
    public class When_converting_culture_info_to_paypal_locale_code : WithFakes
    {
        It should_return_the_language_and_country_paypal_locale_code_if_supported = () =>
            new CultureInfo("da-DK").ToPayPalLocaleCode().ShouldEqual("da_DK");

        It should_return_the_country_paypal_locale_code_if_supported = () =>
            new CultureInfo("en-AU").ToPayPalLocaleCode().ShouldEqual("AU");

        It should_return_null_if_country_is_not_supported = () =>
            new CultureInfo("af-ZA").ToPayPalLocaleCode().ShouldBeNull();
    }
}