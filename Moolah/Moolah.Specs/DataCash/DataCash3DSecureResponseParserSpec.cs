using System.Web;
using Machine.Specifications;
using Moolah.DataCash;

namespace Moolah.Specs.DataCash
{
    public abstract class DataCash3DSecureResponseParserContext
    {
        Establish context = () =>
            SUT = new DataCash3DSecureResponseParser();

        Because of = () =>
            Response = SUT.Parse(ResponseXml);

        protected static DataCash3DSecureResponseParser SUT;
        protected static string ResponseXml;
        protected static I3DSecureResponse Response;
        protected static string DataCashResponse;
        protected static string TransactionReference;
        protected static PaymentStatus ExpectedStatus;        
        protected static string ExpectedFailureMessage;
        protected static CardFailureType ExpectedFailureType;
        protected static bool IsSystemFailure;
        protected static string DataCashReference;
        protected static int StatusCode;
    }

    [Subject(typeof(DataCash3DSecureResponseParser))]
    public class When_3ds_transaction_was_successfully_authorised : DataCash3DSecureResponseParserContext
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
            ResponseXml = string.Format(DataCashResponses.AuthoriseResponseFormat, DataCashReference, StatusCode);
        };
    }

    [Subject(typeof(DataCash3DSecureResponseParser))]
    public class When_3ds_transaction_was_not_authorised_with_non_system_failure : DataCash3DSecureResponseParserContext
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
            ResponseXml = string.Format(DataCashResponses.AuthoriseResponseFormat, DataCashReference, StatusCode);
        };
    }

    [Subject(typeof(DataCash3DSecureResponseParser))]
    public class When_3ds_datacash_status_code_represents_a_system_failure : DataCash3DSecureResponseParserContext
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
            ResponseXml = string.Format(DataCashResponses.AuthoriseResponseFormat, DataCashReference, StatusCode);
        };
    }

    [Subject(typeof(DataCash3DSecureResponseParser))]
    public class When_3ds_datacash_status_code_is_unknown : DataCash3DSecureResponseParserContext
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
            ResponseXml = string.Format(DataCashResponses.AuthoriseResponseFormat, DataCashReference, StatusCode);
        };
    }

    [Subject(typeof(DataCash3DSecureResponseParser))]
    public class When_3ds_datacash_reference_is_not_present_in_response : DataCash3DSecureResponseParserContext
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
            ResponseXml = string.Format(DataCashResponses.AuthoriseResponseWithoutDataCashReference, StatusCode);
        };
    }

    [Subject(typeof(DataCash3DSecureResponseParser))]
    public class When_3ds_payer_verification_is_required : DataCash3DSecureResponseParserContext
    {
        Behaves_like<DataCashResponseBehavior> correctly_parsed_response;

        It should_require_3ds_payer_verification = () =>
            Response.Requires3DSecurePayerVerification.ShouldBeTrue();

        It should_provide_pareq_in_response = () =>
            Response.PAReq.ShouldEqual(PAReq);

        It should_provide_acs_url_in_response = () =>
            Response.ACSUrl.ShouldEqual(ACSUrl);

        Establish context = () =>
        {
            StatusCode = DataCashStatus.RequiresThreeDSecureAuthentication;
            ExpectedStatus = PaymentStatus.Pending;
            ExpectedFailureMessage = null;
            ExpectedFailureType = CardFailureType.None;
            IsSystemFailure = false;
            DataCashReference = "3000000088888888";
            ResponseXml = string.Format(DataCashResponses.ThreeDSecureAuthenticationRequiredResponse, DataCashReference, 
                PAReq, HttpUtility.HtmlEncode(ACSUrl));
        };

        const string PAReq =
            @"eJxdUltugzAQ/M8pUA+AHyEQKscSLR/NB1HU5AKWsypIBRIbStrT14Y4JkGAdnYWdjRjdiwVQH4A2SvgiyBgBWgtviCoTpuXswgjuiZ4tUxeLGnoffYJl6k26AeUrtqGkxCHlCEHHV2AkqVoOtcwLSEvb9sdj5MkSglDN+j5GtQ252lKcJxgzNCEPd+IGvhBKFEyNNaekm3fdOqXr2nMkAOe7tU3H4YhPIlOSKHLULY1Q7br5KJnvWzf24aeb7lWJ17k2TB/dvk2Ko4ZKf7khiE74efNOuAU4xhTTAK6fF2l5mZo7M98qa1gTnCII2PLhDx9tkIyN2NH5p2ZB71S0EhngkN+AK7ntgHzjYnrXs/Ugpa8A90Zgba8W/PsBHv/eEhVdiYmowwv7Wu6xnxH4mF/ZWIh1LI3cF/i/mnWuWNmQxkPJF8w9HhY/wFTsbsh";
        const string ACSUrl = @"https://www.clicksafe.lloydstsb.com/Lloyds/tdsecure/pa.jsp?partner=mc&VAA=B";
    }

    [Subject(typeof(DataCash3DSecureResponseParser))]
    public class When_3ds_card_not_enrolled : DataCash3DSecureResponseParserContext
    {
        Behaves_like<DataCashResponseBehavior> correctly_parsed_response;

        It should_not_require_3ds_payer_verification = () =>
            Response.Requires3DSecurePayerVerification.ShouldBeFalse();

        Establish context = () =>
        {
            StatusCode = 162; // 3DS Card not enrolled
            ExpectedStatus = PaymentStatus.Pending;
            ExpectedFailureMessage = null;
            ExpectedFailureType = CardFailureType.None;
            IsSystemFailure = false;
            DataCashReference = "3000000088888888";
            ResponseXml = string.Format(DataCashResponses.AuthoriseResponseFormat, DataCashReference, StatusCode);
        };
    }
}