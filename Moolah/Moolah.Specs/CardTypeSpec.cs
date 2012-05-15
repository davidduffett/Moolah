using Machine.Specifications;

namespace Moolah.Specs
{
    [Subject(typeof(CardType))]
    public class Visa_card_type
    {
        It should_start_with_4 = () =>
        {
            CardType.Visa.IsMatch("4111111111111111").ShouldBeTrue();
            CardType.Visa.IsMatch("0111111111111111").ShouldBeFalse();
            CardType.Visa.IsMatch("1111111111111111").ShouldBeFalse();
            CardType.Visa.IsMatch("2111111111111111").ShouldBeFalse();
            CardType.Visa.IsMatch("3111111111111111").ShouldBeFalse();
            CardType.Visa.IsMatch("5111111111111111").ShouldBeFalse();
            CardType.Visa.IsMatch("6111111111111111").ShouldBeFalse();
            CardType.Visa.IsMatch("7111111111111111").ShouldBeFalse();
            CardType.Visa.IsMatch("8111111111111111").ShouldBeFalse();
            CardType.Visa.IsMatch("9111111111111111").ShouldBeFalse();
        };

        It should_be_16_digits = () =>
        {
            CardType.Visa.IsMatch("4234567890123456").ShouldBeTrue();
            CardType.Visa.IsMatch("42345678901234567").ShouldBeFalse();
            CardType.Visa.IsMatch("423456789012345").ShouldBeFalse();
        };

        It should_ignore_spaces_and_dashes = () =>
        {
            CardType.Visa.IsMatch("4234 5678 9012 3456").ShouldBeTrue();
            CardType.Visa.IsMatch("4234-5678-9012-3456").ShouldBeTrue();
        };

        It should_have_3_digit_security_code = () =>
            CardType.Visa.SecurityCodeLength.ShouldEqual(3);
    }

    [Subject(typeof(CardType))]
    public class Mastercard_card_type
    {
        It should_start_with_5_then_1_to_5 = () =>
        {
            CardType.Mastercard.IsMatch("5111111111111111").ShouldBeTrue();
            CardType.Mastercard.IsMatch("5211111111111111").ShouldBeTrue();
            CardType.Mastercard.IsMatch("5311111111111111").ShouldBeTrue();
            CardType.Mastercard.IsMatch("5411111111111111").ShouldBeTrue();
            CardType.Mastercard.IsMatch("5511111111111111").ShouldBeTrue();
            CardType.Mastercard.IsMatch("0111111111111111").ShouldBeFalse();
            CardType.Mastercard.IsMatch("1111111111111111").ShouldBeFalse();
            CardType.Mastercard.IsMatch("2111111111111111").ShouldBeFalse();
            CardType.Mastercard.IsMatch("3111111111111111").ShouldBeFalse();
            CardType.Mastercard.IsMatch("4111111111111111").ShouldBeFalse();
            CardType.Mastercard.IsMatch("5011111111111111").ShouldBeFalse();
            CardType.Mastercard.IsMatch("5611111111111111").ShouldBeFalse();
            CardType.Mastercard.IsMatch("5711111111111111").ShouldBeFalse();
            CardType.Mastercard.IsMatch("5811111111111111").ShouldBeFalse();
            CardType.Mastercard.IsMatch("5911111111111111").ShouldBeFalse();
            CardType.Mastercard.IsMatch("6111111111111111").ShouldBeFalse();
            CardType.Mastercard.IsMatch("7111111111111111").ShouldBeFalse();
            CardType.Mastercard.IsMatch("8111111111111111").ShouldBeFalse();
            CardType.Mastercard.IsMatch("9111111111111111").ShouldBeFalse();
        };

        It should_be_16_digits = () =>
        {
            CardType.Mastercard.IsMatch("5234567890123456").ShouldBeTrue();
            CardType.Mastercard.IsMatch("52345678901234567").ShouldBeFalse();
            CardType.Mastercard.IsMatch("523456789012345").ShouldBeFalse();
        };

        It should_ignore_spaces_and_dashes = () =>
        {
            CardType.Mastercard.IsMatch("5234 5678 9012 3456").ShouldBeTrue();
            CardType.Mastercard.IsMatch("5234-5678-9012-3456").ShouldBeTrue();
        };

        It should_have_3_digit_security_code = () =>
            CardType.Mastercard.SecurityCodeLength.ShouldEqual(3);
    }

    [Subject(typeof(CardType))]
    public class American_express_card_type
    {
        It should_start_with_34_or_37 = () =>
        {
            CardType.AmericanExpress.IsMatch("341111111111111").ShouldBeTrue();
            CardType.AmericanExpress.IsMatch("371111111111111").ShouldBeTrue();
            CardType.AmericanExpress.IsMatch("011111111111111").ShouldBeFalse();
            CardType.AmericanExpress.IsMatch("111111111111111").ShouldBeFalse();
            CardType.AmericanExpress.IsMatch("211111111111111").ShouldBeFalse();
            CardType.AmericanExpress.IsMatch("301111111111111").ShouldBeFalse();
            CardType.AmericanExpress.IsMatch("311111111111111").ShouldBeFalse();
            CardType.AmericanExpress.IsMatch("321111111111111").ShouldBeFalse();
            CardType.AmericanExpress.IsMatch("331111111111111").ShouldBeFalse();
            CardType.AmericanExpress.IsMatch("351111111111111").ShouldBeFalse();
            CardType.AmericanExpress.IsMatch("361111111111111").ShouldBeFalse();
            CardType.AmericanExpress.IsMatch("381111111111111").ShouldBeFalse();
            CardType.AmericanExpress.IsMatch("391111111111111").ShouldBeFalse();
            CardType.AmericanExpress.IsMatch("411111111111111").ShouldBeFalse();
            CardType.AmericanExpress.IsMatch("511111111111111").ShouldBeFalse();
            CardType.AmericanExpress.IsMatch("611111111111111").ShouldBeFalse();
            CardType.AmericanExpress.IsMatch("711111111111111").ShouldBeFalse();
            CardType.AmericanExpress.IsMatch("811111111111111").ShouldBeFalse();
            CardType.AmericanExpress.IsMatch("911111111111111").ShouldBeFalse();
        };

        It should_be_15_digits = () =>
        {
            CardType.AmericanExpress.IsMatch("343456789012345").ShouldBeTrue();
            CardType.AmericanExpress.IsMatch("3434567890123456").ShouldBeFalse();
            CardType.AmericanExpress.IsMatch("34345678901234").ShouldBeFalse();
        };

        It should_ignore_spaces_and_dashes = () =>
        {
            CardType.AmericanExpress.IsMatch("34345 678901 2345").ShouldBeTrue();
            CardType.AmericanExpress.IsMatch("34345-678901-2345").ShouldBeTrue();
        };

        It should_have_4_digit_security_code = () =>
            CardType.AmericanExpress.SecurityCodeLength.ShouldEqual(4);
    }

    [Subject(typeof(CardType))]
    public class Diners_club_card_type
    {
        It should_start_with_300_to_305_or_36_or_38 = () =>
        {
            CardType.DinersClub.IsMatch("30011111111111").ShouldBeTrue();
            CardType.DinersClub.IsMatch("30111111111111").ShouldBeTrue();
            CardType.DinersClub.IsMatch("30211111111111").ShouldBeTrue();
            CardType.DinersClub.IsMatch("30311111111111").ShouldBeTrue();
            CardType.DinersClub.IsMatch("30411111111111").ShouldBeTrue();
            CardType.DinersClub.IsMatch("30511111111111").ShouldBeTrue();
            CardType.DinersClub.IsMatch("36111111111111").ShouldBeTrue();
            CardType.DinersClub.IsMatch("38111111111111").ShouldBeTrue();
            CardType.DinersClub.IsMatch("30111111111111").ShouldBeTrue();
            CardType.DinersClub.IsMatch("30111111111111").ShouldBeTrue();
            CardType.DinersClub.IsMatch("30111111111111").ShouldBeTrue();
            CardType.DinersClub.IsMatch("01111111111111").ShouldBeFalse();
            CardType.DinersClub.IsMatch("11111111111111").ShouldBeFalse();
            CardType.DinersClub.IsMatch("21111111111111").ShouldBeFalse();
            CardType.DinersClub.IsMatch("30611111111111").ShouldBeFalse();
            CardType.DinersClub.IsMatch("31111111111111").ShouldBeFalse();
            CardType.DinersClub.IsMatch("32111111111111").ShouldBeFalse();
            CardType.DinersClub.IsMatch("33111111111111").ShouldBeFalse();
            CardType.DinersClub.IsMatch("35111111111111").ShouldBeFalse();
            CardType.DinersClub.IsMatch("37111111111111").ShouldBeFalse();
            CardType.DinersClub.IsMatch("39111111111111").ShouldBeFalse();
            CardType.DinersClub.IsMatch("41111111111111").ShouldBeFalse();
            CardType.DinersClub.IsMatch("51111111111111").ShouldBeFalse();
            CardType.DinersClub.IsMatch("61111111111111").ShouldBeFalse();
            CardType.DinersClub.IsMatch("71111111111111").ShouldBeFalse();
            CardType.DinersClub.IsMatch("81111111111111").ShouldBeFalse();
            CardType.DinersClub.IsMatch("91111111111111").ShouldBeFalse();
        };

        It should_be_14_digits = () =>
        {
            CardType.DinersClub.IsMatch("30045678901234").ShouldBeTrue();
            CardType.DinersClub.IsMatch("300456789012345").ShouldBeFalse();
            CardType.DinersClub.IsMatch("3004567890123").ShouldBeFalse();
        };

        It should_ignore_spaces_and_dashes = () =>
        {
            CardType.DinersClub.IsMatch("3004 5678 9012 34").ShouldBeTrue();
            CardType.DinersClub.IsMatch("3004-5678-9012-34").ShouldBeTrue();
        };

        It should_have_3_digit_security_code = () =>
            CardType.DinersClub.SecurityCodeLength.ShouldEqual(3);
    }

    [Subject(typeof(CardType))]
    public class Discover_card_type
    {
        It should_start_with_6011_or_65 = () =>
        {
            CardType.Discover.IsMatch("6011111111111111").ShouldBeTrue();
            CardType.Discover.IsMatch("6511111111111111").ShouldBeTrue();
            CardType.Discover.IsMatch("0111111111111111").ShouldBeFalse();
            CardType.Discover.IsMatch("1111111111111111").ShouldBeFalse();
            CardType.Discover.IsMatch("2111111111111111").ShouldBeFalse();
            CardType.Discover.IsMatch("3111111111111111").ShouldBeFalse();
            CardType.Discover.IsMatch("4111111111111111").ShouldBeFalse();
            CardType.Discover.IsMatch("5111111111111111").ShouldBeFalse();
            CardType.Discover.IsMatch("6111111111111111").ShouldBeFalse();
            CardType.Discover.IsMatch("6211111111111111").ShouldBeFalse();
            CardType.Discover.IsMatch("6311111111111111").ShouldBeFalse();
            CardType.Discover.IsMatch("6411111111111111").ShouldBeFalse();
            CardType.Discover.IsMatch("6611111111111111").ShouldBeFalse();
            CardType.Discover.IsMatch("6711111111111111").ShouldBeFalse();
            CardType.Discover.IsMatch("6811111111111111").ShouldBeFalse();
            CardType.Discover.IsMatch("6911111111111111").ShouldBeFalse();
            CardType.Discover.IsMatch("7111111111111111").ShouldBeFalse();
            CardType.Discover.IsMatch("8111111111111111").ShouldBeFalse();
            CardType.Discover.IsMatch("9111111111111111").ShouldBeFalse();
        };

        It should_be_16_digits = () =>
        {
            CardType.Discover.IsMatch("6011567890123456").ShouldBeTrue();
            CardType.Discover.IsMatch("60115678901234567").ShouldBeFalse();
            CardType.Discover.IsMatch("601156789012345").ShouldBeFalse();
        };

        It should_ignore_spaces_and_dashes = () =>
        {
            CardType.Discover.IsMatch("6011 5678 9012 3456").ShouldBeTrue();
            CardType.Discover.IsMatch("6011-5678-9012-3456").ShouldBeTrue();
        };

        It should_have_3_digit_security_code = () =>
            CardType.Discover.SecurityCodeLength.ShouldEqual(3);
    }

    [Subject(typeof(CardType))]
    public class JCB_card_type
    {
        It should_start_with_2131_or_1800_or_35 = () =>
        {
            CardType.JCB.IsMatch("213111111111111").ShouldBeTrue();
            CardType.JCB.IsMatch("180011111111111").ShouldBeTrue();
            CardType.JCB.IsMatch("3511111111111111").ShouldBeTrue();
            CardType.JCB.IsMatch("011111111111111").ShouldBeFalse();
            CardType.JCB.IsMatch("111111111111111").ShouldBeFalse();
            CardType.JCB.IsMatch("211111111111111").ShouldBeFalse();
            CardType.JCB.IsMatch("311111111111111").ShouldBeFalse();
            CardType.JCB.IsMatch("411111111111111").ShouldBeFalse();
            CardType.JCB.IsMatch("511111111111111").ShouldBeFalse();
            CardType.JCB.IsMatch("611111111111111").ShouldBeFalse();
            CardType.JCB.IsMatch("711111111111111").ShouldBeFalse();
            CardType.JCB.IsMatch("811111111111111").ShouldBeFalse();
            CardType.JCB.IsMatch("911111111111111").ShouldBeFalse();
        };

        It should_be_15_digits_if_starting_with_2131_or_1800 = () =>
        {
            CardType.JCB.IsMatch("213156789012345").ShouldBeTrue();
            CardType.JCB.IsMatch("2131567890123456").ShouldBeFalse();
            CardType.JCB.IsMatch("21315678901234").ShouldBeFalse();
            CardType.JCB.IsMatch("180056789012345").ShouldBeTrue();
            CardType.JCB.IsMatch("1800567890123456").ShouldBeFalse();
            CardType.JCB.IsMatch("18005678901234").ShouldBeFalse();
        };

        It should_be_16_digits_if_starting_with_35 = () =>
        {
            CardType.JCB.IsMatch("3534567890123456").ShouldBeTrue();
            CardType.JCB.IsMatch("35345678901234567").ShouldBeFalse();
            CardType.JCB.IsMatch("353456789012345").ShouldBeFalse();
        };

        It should_ignore_spaces_and_dashes = () =>
        {
            CardType.JCB.IsMatch("1800 5678 9012 345").ShouldBeTrue();
            CardType.JCB.IsMatch("1800-5678-9012-345").ShouldBeTrue();
        };

        It should_have_3_digit_security_code = () =>
            CardType.JCB.SecurityCodeLength.ShouldEqual(3);
    }
}