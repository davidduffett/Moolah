using Machine.Specifications;
using Moolah.DataCash;

namespace Moolah.Specs.DataCash
{
    public abstract class DataCashMoToContext : DataCashIntegrationContext
    {
        Establish context = () =>
        {
            Configuration = new DataCashConfiguration(PaymentEnvironment.Test, MerchantId, Password);
            ExpiryDate = MoToExpiryDate;
        };

        Because of = () =>
        {
            Gateway = new DataCashMoToGateway(Configuration);
            Response = Gateway.Payment(MerchantReference(), 12.99m, new CardDetails { Number = CardNumber, Cv2 = "123", ExpiryDate = ExpiryDate });
        };

        protected static DataCashConfiguration Configuration;
        protected static IPaymentGateway Gateway;
        protected static ICardPaymentResponse Response;
    }

    [Subject(typeof(DataCashMoToGateway), "Integration")]
    [Ignore("Integration requires DataCash MerchantId and Password to be provided")]
    public class DataCash_MoTo_payment_authorised : DataCashMoToContext
    {
        It should_be_successful = () =>
            Response.Status.ShouldEqual(PaymentStatus.Successful);

        It should_not_be_a_system_failure = () =>
            Response.IsSystemFailure.ShouldBeFalse();

        It should_provide_a_datacash_transaction_reference = () =>
            Response.TransactionReference.ShouldNotBeEmpty();

        It should_provide_an_avscv2_result = () =>
            Response.AvsCv2Result.ShouldNotBeEmpty();

        It should_not_have_a_failure_message = () =>
            Response.FailureMessage.ShouldBeNull();

        Establish context = () =>
            CardNumber = AuthorisedCardNumber;
    }

    [Subject(typeof(DataCashMoToGateway), "Integration")]
    [Ignore("Integration requires DataCash MerchantId and Password to be provided")]
    public class DataCash_MoTo_payment_not_authorised : DataCashMoToContext
    {
        It should_have_failed = () =>
            Response.Status.ShouldEqual(PaymentStatus.Failed);

        It should_not_be_a_system_failure = () =>
            Response.IsSystemFailure.ShouldBeFalse();

        It should_provide_a_datacash_transaction_reference = () =>
            Response.TransactionReference.ShouldNotBeEmpty();

        It should_have_the_expected_failure_message = () =>
            Response.FailureMessage.ShouldEqual("Transaction was declined by your bank. Please check your details or contact your bank and try again.");

        Establish context = () =>
            CardNumber = DeclinedCardNumber;
    }

    [Subject(typeof(DataCashMoToGateway), "Integration")]
    [Ignore("Integration requires DataCash MerchantId and Password to be provided")]
    public class DataCash_MoTo_payment_system_failure : DataCashMoToContext
    {
        It should_have_failed = () =>
            Response.Status.ShouldEqual(PaymentStatus.Failed);

        It should_be_a_system_failure = () =>
            Response.IsSystemFailure.ShouldBeTrue();

        It should_not_provide_a_datacash_transaction_reference = () =>
            Response.TransactionReference.ShouldBeNull();

        It should_have_a_failure_message = () =>
            Response.FailureMessage.ShouldEqual("The vTID or password were incorrect");

        Establish context = () =>
        {
            // Use incorrect vTID username/password
            Configuration = new DataCashConfiguration(PaymentEnvironment.Test, "bad_id", "bad_password");
            CardNumber = AuthorisedCardNumber;
        };
    }
}