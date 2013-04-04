using Machine.Specifications;
using Moolah.PayPal;

namespace Moolah.Specs.PayPal
{
    [Subject(typeof(PayPalRequestBuilder))]
    public class When_building_mass_payment_request_using_receivers_email_addresses : PayPalRequestBuilderContext
    {
        Behaves_like<PayPalCommonRequestBehavior> a_paypal_nvp_request;

        It should_include_mass_pay_as_a_method = () =>
            Request["METHOD"].ShouldEqual("MassPay");

        It should_include_currency_code = () =>
            Request["CURRENCYCODE"].ShouldEqual("GBP");

        It should_include_receiver_type = () =>
            Request["RECEIVERTYPE"].ShouldEqual("EmailAddress");

        It should_include_email_subject_if_specified = () =>
            Request["EMAILSUBJECT"].ShouldEqual("Email subject");

        It should_include_receivers = () =>
        {
            Request["L_EMAIL0"].ShouldEqual("buyer1@gmail.com");
            Request["L_AMT0"].ShouldEqual("10.00");
            Request["L_EMAIL1"].ShouldEqual("buyer2@gmail.com");
            Request["L_AMT1"].ShouldEqual("5.00");
        };

        It should_include_unique_id_and_note_where_specified = () =>
        {
            Request["L_UNIQUEID0"].ShouldEqual("12345");
            Request["L_NOTE0"].ShouldEqual("Note 1");
        };

        Because of = () =>
            Request = SUT.MassPayment(new[] 
                {
                    new PayReceiver("buyer1@gmail.com", 10, "12345", "Note 1"),
                    new PayReceiver("buyer2@gmail.com", 5)
                },
                CurrencyCodeType.GBP,
                ReceiverType.EmailAddress, 
                "Email subject");
    }

    [Subject(typeof(PayPalRequestBuilder))]
    public class When_building_mass_payment_request_using_receivers_user_ids : PayPalRequestBuilderContext
    {
        It should_include_receivers = () =>
        {
            Request["L_RECEIVERID0"].ShouldEqual("123400");
            Request["L_RECEIVERID1"].ShouldEqual("76583");
        };

        Because of = () =>
            Request = SUT.MassPayment(new[] 
                {
                    new PayReceiver("123400", 10),
                    new PayReceiver("76583", 5)
                },
                CurrencyCodeType.GBP,
                ReceiverType.UserID,
                null);
    }
}