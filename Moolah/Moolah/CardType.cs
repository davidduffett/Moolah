using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Moolah
{
    public class CardType
    {
        public static CardType Visa = new CardType("Visa", @"^4[0-9]{12}(?:[0-9]{3})?$");
        public static CardType Mastercard = new CardType("Mastercard", @"^5[1-5][0-9]{14}$");
        public static CardType AmericanExpress = new CardType("American Express", @"^3[47][0-9]{13}$", 4);
        public static CardType DinersClub = new CardType("Diners Club", @"^3(?:0[0-5]|[68][0-9])[0-9]{11}$");
        public static CardType Discover = new CardType("Discover", @"^6(?:011|5[0-9]{2})[0-9]{12}$");
        public static CardType JCB = new CardType("JCB", @"^(?:2131|1800|35\d{3})\d{11}$");

        public static IEnumerable<CardType> AllCardTypes = new[] 
            { Visa, Mastercard, AmericanExpress, DinersClub, Discover, JCB };

        readonly Regex _regex;

        public CardType(string name, string cardNumberRegex, int securityCodeLength = 3)
        {
            Name = name;
            _regex = new Regex(cardNumberRegex);
            SecurityCodeLength = securityCodeLength;
        }

        public string Name { get; protected set; }
        public int SecurityCodeLength { get; protected set; }

        public bool IsMatch(string cardNumber)
        {
            return cardNumber != null &&
                   _regex.IsMatch(cardNumber.StripSpacesAndDashes());
        }
    }
}