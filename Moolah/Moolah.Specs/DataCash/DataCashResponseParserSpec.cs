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

        It should_set_correct_failure_type = () =>
            Response.FailureType.ShouldEqual(ExpectedFailureType);

        It should_set_correct_system_failure = () =>
            Response.IsSystemFailure.ShouldEqual(IsSystemFailure);

        It should_set_avscv2_result = () =>
            Response.AvsCv2Result.ShouldEqual(ExpectedAvsCv2Result);

        protected static ICardPaymentResponse Response;
        protected static string DataCashReference;
        protected static PaymentStatus ExpectedStatus;        
        protected static string ExpectedFailureMessage;
        protected static CardFailureType ExpectedFailureType;
        protected static bool IsSystemFailure;
        protected static string ExpectedAvsCv2Result;
    }

    public abstract class DataCashResponseParserContext
    {
        Establish context = () =>
            SUT = new DataCashResponseParser();

        Because of = () =>
            Response = SUT.Parse(ResponseXml);

        protected static DataCashResponseParser SUT;
        protected static string ResponseXml;
        protected static ICardPaymentResponse Response;
        protected static string DataCashResponse;
        protected static string TransactionReference;
        protected static PaymentStatus ExpectedStatus;
        protected static string ExpectedFailureMessage;
        protected static CardFailureType ExpectedFailureType;
        protected static bool IsSystemFailure;
        protected static string DataCashReference;
        protected static int StatusCode;
        protected static string ExpectedAvsCv2Result;
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
            ExpectedFailureType = CardFailureType.None;
            IsSystemFailure = false;
            DataCashReference = "3000000088888888";
            ExpectedAvsCv2Result = "ALL MATCH";
            ResponseXml = string.Format(DataCashResponses.AuthoriseResponseFormat, DataCashReference, StatusCode, ExpectedAvsCv2Result);
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
            ExpectedFailureMessage = DataCashFailureReasons.CleanFailures[DataCashStatus.NotAuthorised].Message;
            ExpectedFailureType = DataCashFailureReasons.CleanFailures[DataCashStatus.NotAuthorised].Type;
            IsSystemFailure = false;
            DataCashReference = "3000000088888888";
            ExpectedAvsCv2Result = "NO DATA MATCHES";
            ResponseXml = string.Format(DataCashResponses.AuthoriseResponseFormat, DataCashReference, StatusCode, ExpectedAvsCv2Result);
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
            ExpectedFailureMessage = DataCashFailureReasons.SystemFailures[systemFailureCode].Message;
            ExpectedFailureType = DataCashFailureReasons.SystemFailures[systemFailureCode].Type;
            IsSystemFailure = true;
            DataCashReference = "3000000088888888";
            ExpectedAvsCv2Result = "NO DATA MATCHES";
            ResponseXml = string.Format(DataCashResponses.AuthoriseResponseFormat, DataCashReference, StatusCode, ExpectedAvsCv2Result);
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
            ExpectedFailureType = CardFailureType.General;
            IsSystemFailure = true;
            DataCashReference = "3000000088888888";
            ExpectedAvsCv2Result = "NO DATA MATCHES";
            ResponseXml = string.Format(DataCashResponses.AuthoriseResponseFormat, DataCashReference, StatusCode, ExpectedAvsCv2Result);
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
            ExpectedFailureType = CardFailureType.General;
            IsSystemFailure = true;
            DataCashReference = null;
            ExpectedAvsCv2Result = "NO DATA MATCHES";
            ResponseXml = string.Format(DataCashResponses.AuthoriseResponseWithoutDataCashReference, StatusCode, ExpectedAvsCv2Result);
        };
    }
}