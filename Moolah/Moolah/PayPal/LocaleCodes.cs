using System.Globalization;
using System.Linq;

namespace Moolah.PayPal
{
    public static class LocaleCodes
    {
        /// <summary>
        /// Supported locale codes listed in the API reference
        /// https://cms.paypal.com/uk/cgi-bin/?cmd=_render-content&content_ID=developer/e_howto_api_nvp_r_SetExpressCheckout
        /// </summary>
        public static string[] PayPalLocaleCodes = 
        {
            "AU", "AT", "BE", "BR", "CA", "CH", "CN", "DE", "ES", "GB", "FR", "IT", "NL", "PL", "PT", "RU", "US",
            "da_DK", "he_IL", "id_ID", "jp_JP", "no_NO", "pt_BR", "ru_RU", "sv_SE", "th_TH", "tr_TR", "zh_CN", "zh_HK", "zh_TW"
        };

        public static bool LocaleCodeSupported(string localeCode)
        {
            return localeCode != null && PayPalLocaleCodes.Contains(localeCode);
        }

        /// <summary>
        /// Translates the culture info into a PayPal locale code, first by searching for the language & country code (eg. "da-DK" => "da_DK")
        /// then for just the country code (eg. "AU").
        /// </summary>
        public static string ToPayPalLocaleCode(this CultureInfo cultureInfo)
        {
            var paypalCultureName = cultureInfo.Name.Replace('-', '_');
            if (LocaleCodeSupported(paypalCultureName))
                return paypalCultureName;

            var paypalCountryCode = paypalCultureName.Substring(paypalCultureName.Length - 2, 2);
            if (LocaleCodeSupported(paypalCountryCode))
                return paypalCountryCode;

            return null;
        }
    }
}