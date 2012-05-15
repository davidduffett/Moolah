using System.Linq;

namespace Moolah
{
    public static class CardUtility
    {
        /// <summary>
        /// Uses the LUHN formula to determine whether the specified card number is valid.
        /// See http://en.wikipedia.org/wiki/Luhn
        /// </summary>
        /// <param name="cardNumber">Card number to validate.  It may contain spaces and/or dashes.</param>
        public static bool IsValidCardNumber(string cardNumber)
        {
            if (cardNumber == null) return false;
            cardNumber = cardNumber.StripSpacesAndDashes();
            if (cardNumber.Any(c => !char.IsDigit(c))) return false;

            var checksum = cardNumber
                .Select((c, i) => (c - '0') << ((cardNumber.Length - i - 1) & 1))
                .Sum(n => n > 9 ? n - 9 : n);

            return (checksum%10) == 0 && checksum > 0;
        }

        /// <summary>
        /// Determines the card type based on the card number specified.
        /// If the card number is unrecognised, null is returned.
        /// </summary>
        /// <param name="cardNumber">Card number to determine card type from.</param>
        public static string GetCardType(string cardNumber)
        {
            return (from cardType in CardType.AllCardTypes 
                    where cardType.IsMatch(cardNumber) 
                    select cardType.Name).FirstOrDefault();
        }

        /// <summary>
        /// Returns a masked partial card number, only showing the last 4 digits.
        /// </summary>
        /// <param name="cardNumber">Card number to be masked</param>
        /// <returns>For example, xxxxxxxxxxxx1234</returns>
        public static string GetPartialCardNumber(string cardNumber)
        {
            if (cardNumber == null) return null;
            cardNumber = cardNumber.StripSpacesAndDashes();

            var maskLength = cardNumber.Length - 4;
            return maskLength < 4
                ? null
                : new string('x', maskLength) + cardNumber.Substring(maskLength);
        }

        public static string StripSpacesAndDashes(this string cardNumber)
        {
            return cardNumber == null
                       ? null
                       : cardNumber.Replace(" ", string.Empty).Replace("-", string.Empty);
        }
    }
}