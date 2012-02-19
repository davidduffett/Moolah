using Machine.Specifications;
using Moolah.DataCash;

namespace Moolah.Specs.DataCash
{
    [Behaviors]
    public class DataCashResponseBehavior
    {
        It should_set_transaction_reference = () =>
            Response.TransactionReference.ShouldEqual(DataCashResponses.DataCashReference);

        It should_set_correct_payment_status = () =>
            Response.Status.ShouldEqual(ExpectedStatus);

        It should_set_correct_reason = () =>
            Response.Reason.ShouldEqual(ExpectedReason);

        protected static IPaymentResponse Response;
        protected static PaymentStatus ExpectedStatus;
        protected static string ExpectedReason;
    }

    public abstract class DataCashResponseParserContext
    {
        Establish context = () =>
            SUT = new DataCashResponseParser();

        Because of = () =>
            Response = SUT.Parse(DataCashResponse);

        protected static DataCashResponseParser SUT;
        protected static IPaymentResponse Response;
        protected static string DataCashResponse;
        protected static string TransactionReference;
        protected static PaymentStatus ExpectedStatus;
        protected static string ExpectedReason;
    }

    [Subject(typeof(DataCashResponseParser))]
    public class When_transaction_was_successfully_authorised : DataCashResponseParserContext
    {
        Behaves_like<DataCashResponseBehavior> correctly_parsed_response;

        Establish context = () =>
        {
            DataCashResponse = DataCashResponses.Authorised;
            ExpectedStatus = PaymentStatus.Successful;
            ExpectedReason = null;
        };
    }

    [Subject(typeof(DataCashResponseParser))]
    public class When_transaction_was_not_authorised : DataCashResponseParserContext
    {
        Behaves_like<DataCashResponseBehavior> correctly_parsed_response;

        Establish context = () =>
        {
            DataCashResponse = DataCashResponses.NotAuthorised;
            ExpectedStatus = PaymentStatus.Failed;
            // TODO: more specific test cases for types of failure (eg. DECLINED or REFERRED)
            ExpectedReason = null;
        };
    }
}