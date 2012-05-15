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
}