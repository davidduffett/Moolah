using System;
using Machine.Specifications;
using Moolah.DataCash;

namespace Moolah.Specs
{
    public static class DataCashMoTo
    {
        // Provide your own DataCash Test Server credentials!
        public const string MerchantId = "merchantId";
        public const string Password = "password";

        public const string AuthorisedCardNumber = "1000010000000007";
        public const string DeclinedCardNumber = "1000350000000106";

        public static DataCashConfiguration Configuration()
        {
            return new DataCashConfiguration(PaymentEnvironment.Test, MerchantId, Password);
        }

        /// <summary>
        /// DataCash requires unique merchant references (even on their TEST servers!),
        /// so use clock ticks for now.
        /// </summary>
        public static string MerchantReference()
        {
            return DateTime.UtcNow.Ticks.ToString();
        }
    }

    public abstract class DataCashMotoIntegrationContext
    {
        // NOTE: Provide your own DataCash Test Server credentials!
        const string MerchantId = "merchantId";
        const string Password = "password";

        Because of = () =>
        {
            var gateway = new DataCashMoToGateway(Configuration);
            Response = gateway.Payment(DataCashMoTo.MerchantReference(),
                                       12.99m,
                                       new CardDetails
                                       {
                                           Number = CardNumber,
                                           Cv2 = "123",
                                           ExpiryDate = "02/18"
                                       });
        };

        protected static IPaymentResponse Response;
        protected static string CardNumber;
        protected static DataCashConfiguration Configuration = new DataCashConfiguration(PaymentEnvironment.Test, MerchantId, Password);
        protected const string AuthorisedCardNumber = "1000010000000007";
        protected const string DeclinedCardNumber = "1000350000000106";
    }

    [Subject(typeof(DataCashMoToGateway), "Integration")]
    [Ignore("Integration requires DataCash MerchantId and Password to be provided")]
    public class DataCash_MoTo_payment_authorised : DataCashMotoIntegrationContext
    {
        It should_be_successful = () =>
            Response.Status.ShouldEqual(PaymentStatus.Successful);

        It should_not_be_a_system_failure = () =>
            Response.IsSystemFailure.ShouldBeFalse();

        It should_provide_a_datacash_transaction_reference = () =>
            Response.TransactionReference.ShouldNotBeEmpty();

        It should_not_have_a_failure_message = () =>
            Response.FailureMessage.ShouldBeNull();

        Establish context = () =>
            CardNumber = AuthorisedCardNumber;
    }

    [Subject(typeof(DataCashMoToGateway), "Integration")]
    [Ignore("Integration requires DataCash MerchantId and Password to be provided")]
    public class DataCash_MoTo_payment_not_authorised : DataCashMotoIntegrationContext
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
    public class DataCash_MoTo_payment_system_failure : DataCashMotoIntegrationContext
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