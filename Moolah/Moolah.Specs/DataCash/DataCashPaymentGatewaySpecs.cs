using Machine.Fakes;
using Machine.Specifications;
using Moolah.DataCash;

namespace Moolah.Specs.DataCash
{
    [Subject(typeof(DataCashPaymentGateway))]
    public class When_submitting_datacash_payment : WithFakes
    {
        It should_validate_card_number_against_bin_files;
        It should_validate_card_details_using_datacash_validation;
        It should_post_xml_to_datacash;
    }
}