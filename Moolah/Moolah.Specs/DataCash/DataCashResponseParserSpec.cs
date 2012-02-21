using Machine.Specifications;
using Moolah.DataCash;

namespace Moolah.Specs.DataCash
{
    [Behaviors]
    public class DataCashResponseBehavior
    {
        It should_set_transaction_reference = () =>
            Response.TransactionReference.ShouldEqual(DataCashReference);

        It should_set_correct_payment_status = () =>
            Response.Status.ShouldEqual(ExpectedStatus);

        It should_set_correct_failure_message = () =>
            Response.FailureMessage.ShouldEqual(ExpectedFailureMessage);

        It should_set_correct_system_failure = () =>
            Response.IsSystemFailure.ShouldEqual(IsSystemFailure);

        protected static IPaymentResponse Response;
        protected static string DataCashReference;
        protected static PaymentStatus ExpectedStatus;
        protected static string ExpectedFailureMessage;
        protected static bool IsSystemFailure;
    }

    public abstract class DataCashResponseParserContext
    {
        Establish context = () =>
            SUT = new DataCashResponseParser();

        Because of = () =>
            Response = SUT.Parse(ResponseXml);

        protected static DataCashResponseParser SUT;
        protected static string ResponseXml;
        protected static IPaymentResponse Response;
        protected static string DataCashResponse;
        protected static string TransactionReference;
        protected static PaymentStatus ExpectedStatus;
        protected static string ExpectedFailureMessage;
        protected static bool IsSystemFailure;
        protected static string DataCashReference;
        protected static int StatusCode;
    }

    [Subject(typeof(DataCashResponseParser))]
    public class When_transaction_was_successfully_authorised : DataCashResponseParserContext
    {
        Behaves_like<DataCashResponseBehavior> correctly_parsed_response;

        Establish context = () =>
        {
            StatusCode = DataCashStatus.Success;
            ExpectedStatus = PaymentStatus.Successful;
            ExpectedFailureMessage = null;
            IsSystemFailure = false;
            DataCashReference = "3000000088888888";
            ResponseXml = string.Format(DataCashResponses.AuthoriseResponseFormat, DataCashReference, StatusCode);
        };
    }

    [Subject(typeof(DataCashResponseParser))]
    public class When_transaction_was_not_authorised_with_non_system_failure : DataCashResponseParserContext
    {
        Behaves_like<DataCashResponseBehavior> correctly_parsed_response;

        Establish context = () =>
        {
            StatusCode = DataCashStatus.NotAuthorised;
            ExpectedStatus = PaymentStatus.Failed;
            ExpectedFailureMessage = DataCashFailureMessages.CleanFailures[DataCashStatus.NotAuthorised];
            IsSystemFailure = false;
            DataCashReference = "3000000088888888";
            ResponseXml = string.Format(DataCashResponses.AuthoriseResponseFormat, DataCashReference, StatusCode);
        };
    }

    [Subject(typeof(DataCashResponseParser))]
    public class When_datacash_status_code_represents_a_system_failure : DataCashResponseParserContext
    {
        Behaves_like<DataCashResponseBehavior> correctly_parsed_response;

        Establish context = () =>
        {
            const int systemFailureCode = 10;
            StatusCode = systemFailureCode;
            ExpectedStatus = PaymentStatus.Failed;
            ExpectedFailureMessage = DataCashFailureMessages.SystemFailures[systemFailureCode];
            IsSystemFailure = true;
            DataCashReference = "3000000088888888";
            ResponseXml = string.Format(DataCashResponses.AuthoriseResponseFormat, DataCashReference, StatusCode);
        };
    }

    [Subject(typeof(DataCashResponseParser))]
    public class When_datacash_status_code_is_unknown : DataCashResponseParserContext
    {
        Behaves_like<DataCashResponseBehavior> correctly_parsed_response;

        Establish context = () =>
        {
            StatusCode = 999999;
            ExpectedStatus = PaymentStatus.Failed;
            ExpectedFailureMessage = string.Format("Unknown DataCash status code: 999999");
            IsSystemFailure = true;
            DataCashReference = "3000000088888888";
            ResponseXml = string.Format(DataCashResponses.AuthoriseResponseFormat, DataCashReference, StatusCode);
        };
    }

    [Subject(typeof(DataCashResponseParser))]
    public class When_datacash_reference_is_not_present_in_response : DataCashResponseParserContext
    {
        Behaves_like<DataCashResponseBehavior> correctly_parsed_response;

        Establish context = () =>
        {
            StatusCode = 987654;
            ExpectedStatus = PaymentStatus.Failed;
            ExpectedFailureMessage = string.Format("Unknown DataCash status code: 987654");
            IsSystemFailure = true;
            DataCashReference = null;
            ResponseXml = string.Format(DataCashResponses.AuthoriseResponseWithoutDataCashReference, StatusCode);
        };
    }
}