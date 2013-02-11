using Machine.Specifications;
using Moolah.PayPal;

namespace Moolah.Specs.PayPal
{
    [Subject(typeof(PayPalRequestBuilder))]
    public class When_refunding_full_transaction : PayPalRequestBuilderContext
    {
        Behaves_like<PayPalCommonRequestBehavior> a_paypal_nvp_request;

        It should_specify_correct_method = () =>
            Request["METHOD"].ShouldEqual("RefundTransaction");

        It should_specify_transaction_id = () =>
            Request["TRANSACTIONID"].ShouldEqual(TransactionId);

        It should_specify_refund_type = () =>
            Request["REFUNDTYPE"].ShouldEqual("FULL");

        Because of = () =>
            Request = SUT.RefundFullTransaction(TransactionId);

        const string TransactionId = "QWE123";
    }

    [Subject(typeof(PayPalRequestBuilder))]
    public class When_refunding_partial_transaction : PayPalRequestBuilderContext
    {
        Behaves_like<PayPalCommonRequestBehavior> a_paypal_nvp_request;

        It should_specify_correct_method = () =>
            Request["METHOD"].ShouldEqual("RefundTransaction");

        It should_specify_refund_type = () =>
            Request["REFUNDTYPE"].ShouldEqual("Partial");

        It should_specify_transaction_id = () =>
            Request["TRANSACTIONID"].ShouldEqual(TransactionId);

        It should_specify_amount_rounded_to_two_decimal_places = () =>
            Request["AMT"].ShouldEqual("100.10");

        It should_specify_currency_code = () =>
            Request["CURRENCYCODE"].ShouldEqual("PLN");

        It should_specify_description = () =>
            Request["NOTE"].ShouldEqual(Description);

        Because of = () =>
            Request = SUT.RefundPartialTransaction(TransactionId, Amount, CurrencyCode, Description);

        const string TransactionId = "QWE123";
        const decimal Amount = 100.1000m;
        const CurrencyCodeType CurrencyCode = CurrencyCodeType.PLN;
        const string Description = "Refund!";
    }
}