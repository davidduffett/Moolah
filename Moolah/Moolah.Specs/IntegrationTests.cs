using System;
using Machine.Specifications;
using Moolah.DataCash;

namespace Moolah.Specs
{
    public static class DataCashMoToCredentials
    {
        // Provide your own DataCash Test Server credentials!
        public const string MerchantId = "merchantId";
        public const string Password = "password";

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

    [Subject("Integration (DataCash MoTo)")]
    [Ignore("Integration requires DataCash MerchantId and Password to be provided")]
    public class DataCash_MoTo_payment_authorised
    {
        It should_be_successful = () =>
            response.Status.ShouldEqual(PaymentStatus.Successful);

        It should_not_be_a_system_failure = () =>
            response.IsSystemFailure.ShouldEqual(false);

        It should_provide_a_datacash_transaction_reference = () =>
            response.TransactionReference.ShouldNotBeEmpty();

        It should_not_have_a_failure_message = () =>
            response.FailureMessage.ShouldBeNull();

        Because of = () =>
        {
            var gateway = new DataCashMoToGateway(DataCashMoToCredentials.Configuration());
            response = gateway.Payment(DataCashMoToCredentials.MerchantReference(), 
                                       12.99m,
                                       new CardDetails
                                           {
                                               Number = "1000010000000007",
                                               Cv2 = "123",
                                               ExpiryDate = "02/18"
                                           });
        };

        static IPaymentResponse response;
    }
}