using Machine.Specifications;
using Moolah.DataCash;

namespace Moolah.Specs.DataCash
{
    public abstract class DataCashMoToRefundContext : DataCashIntegrationContext
    {
        Establish context = () =>
        {
            CardNumber = AuthorisedCardNumber;
            Configuration = new DataCashConfiguration(PaymentEnvironment.Test, MerchantId, Password);
            Gateway = new DataCashMoToGateway(Configuration);
            Response = Gateway.Payment(MerchantReference(), 12.99m, new CardDetails { Number = CardNumber, Cv2 = "123", ExpiryDate = MoToExpiryDate });
        };

        Because of = () =>
            RefundResponse = Gateway.RefundTransaction(Response.TransactionReference, RefundAmount);

        protected static decimal RefundAmount;
        protected static IRefundTransactionResponse RefundResponse;
        protected static DataCashConfiguration Configuration;
        protected static IPaymentGateway Gateway;
        protected static IPaymentResponse Response;
    }

    [Subject(typeof(DataCashMoToGateway), "Integration")]
    [Ignore("Integration requires DataCash MerchantId and Password to be provided")]
    public class DataCash_MoTo_refund_was_successful : DataCashMoToRefundContext
    {
        It should_be_successful = () =>
            RefundResponse.Status.ShouldEqual(PaymentStatus.Successful);

        It should_not_be_a_system_failure = () =>
            RefundResponse.IsSystemFailure.ShouldBeFalse();

        It should_provide_a_datacash_transaction_reference = () =>
            RefundResponse.TransactionReference.ShouldNotBeEmpty();

        It should_not_have_a_failure_message = () =>
            RefundResponse.FailureMessage.ShouldBeNull();

        Establish context = () =>
            RefundAmount = 12.99m;
    }

    [Subject(typeof(DataCashMoToGateway), "Integration")]
    [Ignore("Integration requires DataCash MerchantId and Password to be provided")]
    public class DataCash_MoTo_refund_was_unsuccessful : DataCashMoToRefundContext
    {
        It should_have_failed = () =>
            RefundResponse.Status.ShouldEqual(PaymentStatus.Failed);

        It should_be_a_system_failure = () =>
            RefundResponse.IsSystemFailure.ShouldBeTrue();

        It should_provide_a_datacash_transaction_reference = () =>
            RefundResponse.TransactionReference.ShouldNotBeEmpty();

        It should_have_a_failure_message = () =>
            RefundResponse.FailureMessage.ShouldNotBeEmpty();

        Establish context = () =>
            RefundAmount = 555.55m;
    }
}