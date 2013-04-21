namespace Moolah.PayPal
{
    public static class PayPalDecimalExtension
    {
        public static string AsPayPalFormatString(this decimal? value)
        {
            return value.HasValue ? value.Value.AsPayPalFormatString() : null;
        }

        /// <summary>
        /// It includes no currency symbol. It must have 2 decimal places, the decimal separator must be a period (.), and the optional thousands separator must be a comma (,).
        /// </summary>
        public static string AsPayPalFormatString(this decimal value)
        {
            return value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static decimal? AsNegativeValue(this decimal? value)
        {
            return value.HasValue ? value.Value.AsNegativeValue() : (decimal?)null;
        }

        public static decimal AsNegativeValue(this decimal value)
        {
            return value < 0 ? value : 0 - value;
        }
    }
}
