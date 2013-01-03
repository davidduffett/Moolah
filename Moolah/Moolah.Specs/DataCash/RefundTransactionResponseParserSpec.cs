using Machine.Fakes;
using Machine.Specifications;
using Moolah.DataCash;

namespace Moolah.Specs.DataCash
{
    [Subject(typeof(RefundTransactionResponseParser))]
    public class When_refund_transaction_is_successful : WithSubject<RefundTransactionResponseParser>
    {
        It should_set_transaction_reference = () =>
            Response.TransactionReference.ShouldEqual(DataCashReference);

        It should_set_status_to_successful = () =>
            Response.Status.ShouldEqual(PaymentStatus.Successful);

        It should_not_have_a_failure_message = () =>
            Response.FailureMessage.ShouldBeNull();

        It should_not_be_a_system_failure = () =>
            Response.IsSystemFailure.ShouldBeFalse();

        Because of = () =>
            Response = Subject.Parse(string.Format(DataCashResponses.RefundResponseFormat, DataCashReference, DataCashStatus.Success));

        static IPaymentResponse Response;
        const string DataCashReference = "123456";
    }

    [Subject(typeof(RefundTransactionResponseParser))]
    public class When_refund_transaction_is_not_successful : WithSubject<RefundTransactionResponseParser>
    {
        It should_set_status_to_failed = () =>
            Response.Status.ShouldEqual(PaymentStatus.Failed);

        It should_have_a_failure_message = () =>
            Response.FailureMessage.ShouldNotBeNull();

        It should_not_be_a_system_failure = () =>
            Response.IsSystemFailure.ShouldBeFalse();

        Because of = () =>
            Response = Subject.Parse(string.Format(DataCashResponses.RefundResponseFormat, "123456", DataCashStatus.NotAuthorised));

        static IPaymentResponse Response;
    }

    [Subject(typeof(RefundTransactionResponseParser))]
    public class When_refund_transaction_is_not_successful_due_to_system_failure : WithSubject<RefundTransactionResponseParser>
    {
        It should_set_status_to_failed = () =>
            Response.Status.ShouldEqual(PaymentStatus.Failed);

        It should_have_a_failure_message = () =>
            Response.FailureMessage.ShouldNotBeNull();

        It should_specify_system_failure = () =>
            Response.IsSystemFailure.ShouldBeTrue();

        Because of = () =>
            Response = Subject.Parse(string.Format(DataCashResponses.RefundResponseFormat, "123456", 10));

        static IPaymentResponse Response;
    }
}