using System.Linq;

namespace Moolah
{
    public class CardUtility
    {
        /// <summary>
        /// Uses the LUHN formula to determine whether the specified card number is valid.
        /// See http://en.wikipedia.org/wiki/Luhn
        /// </summary>
        /// <param name="cardNumber">Card number to validate.  It may contain spaces and/or dashes.</param>
        public static bool IsValidCardNumber(string cardNumber)
        {
            if (cardNumber == null) return false;
            cardNumber = cardNumber.Replace(" ", string.Empty).Replace("-", string.Empty);
            if (cardNumber.Any(c => !char.IsDigit(c))) return false;

            var checksum = cardNumber
                .Select((c, i) => (c - '0') << ((cardNumber.Length - i - 1) & 1))
                .Sum(n => n > 9 ? n - 9 : n);

            return (checksum%10) == 0 && checksum > 0;
        }
    }
}