using Machine.Specifications;

namespace Moolah.Specs
{
    [Subject(typeof(CardUtility))]
    public class When_checking_if_card_number_is_valid
    {
        It should_pass_valid_card_numbers = () =>
        {
            CardUtility.IsValidCardNumber("49927398716").ShouldBeTrue();
            CardUtility.IsValidCardNumber("1234567812345670").ShouldBeTrue();
        };

        It should_ignore_spaces_and_dashes = () =>
        {
            CardUtility.IsValidCardNumber("4992 7398 716").ShouldBeTrue();
            CardUtility.IsValidCardNumber("1234-5678-1234-5670").ShouldBeTrue();
        };

        It should_not_pass_luhn_failures = () =>
        {
            CardUtility.IsValidCardNumber("49927398717").ShouldBeFalse();
            CardUtility.IsValidCardNumber("1234567812345678").ShouldBeFalse();
        };

        It should_not_pass_non_digit_strings = () =>
        {
            CardUtility.IsValidCardNumber("abcdefg").ShouldBeFalse();
            CardUtility.IsValidCardNumber("1234567812345678?").ShouldBeFalse();
        };
    }

    [Subject(typeof(CardUtility))]
    public class When_getting_card_type
    {
        It should_recognise_visa = () =>
            CardUtility.GetCardType("4111111111111111").ShouldEqual(CardType.Visa.Name);

        It should_recognise_mastercard = () =>
            CardUtility.GetCardType("5111111111111111").ShouldEqual(CardType.Mastercard.Name);

        It should_recognise_american_express = () =>
            CardUtility.GetCardType("341111111111111").ShouldEqual(CardType.AmericanExpress.Name);

        It should_recognise_diners_club = () =>
            CardUtility.GetCardType("30011111111111").ShouldEqual(CardType.DinersClub.Name);

        It should_recognise_discover = () =>
            CardUtility.GetCardType("6011111111111111").ShouldEqual(CardType.Discover.Name);

        It should_recognise_jcb = () =>
            CardUtility.GetCardType("213111111111111").ShouldEqual(CardType.JCB.Name);

        It should_ignore_spaces_and_dashes = () =>
        {
            CardUtility.GetCardType("4111 1111 1111 1111").ShouldEqual(CardType.Visa.Name);
            CardUtility.GetCardType("4111-1111-1111-1111").ShouldEqual(CardType.Visa.Name);
        };

        It should_return_null_if_unrecognised = () =>
        {
            CardUtility.GetCardType(null).ShouldBeNull();
            CardUtility.GetCardType("").ShouldBeNull();
            CardUtility.GetCardType("89").ShouldBeNull();
        };
    }

    [Subject(typeof(CardUtility))]
    public class When_getting_partial_card_number
    {
        It should_mask_all_but_last_4_digits_of_card_number = () =>
            CardUtility.GetPartialCardNumber("1234567890123456").ShouldEqual("xxxxxxxxxxxx3456");

        It should_return_null_for_card_numbers_less_than_4_digits = () =>
            CardUtility.GetPartialCardNumber("123").ShouldEqual(null);

        It should_return_null_for_null_card_number = () =>
            CardUtility.GetPartialCardNumber(null).ShouldEqual(null);

        It should_strip_out_spaces_and_dashes = () =>
            CardUtility.GetPartialCardNumber("1234 5678-9012 3456").ShouldEqual("xxxxxxxxxxxx3456");
    }
}