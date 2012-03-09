using Machine.Specifications;
using Moolah.DataCash;
using Moolah.PayPal;

namespace Moolah.Specs
{
    [Subject(typeof(MoolahConfiguration))]
    public class When_loading_datacash_moto_configuration_from_application_config
    {
        It should_provide_correct_environment = () =>
            Config.Environment.ShouldEqual(PaymentEnvironment.Test);

        It should_provide_correct_merchant_id = () =>
            Config.MerchantId.ShouldEqual("motoMerchantId");

        It should_provide_correct_password = () =>
            Config.Password.ShouldEqual("motoPassword");

        Because of = () =>
            Config = MoolahConfiguration.Current.DataCashMoTo;

        static DataCashConfiguration Config;
    }

    [Subject(typeof(MoolahConfiguration))]
    public class When_loading_datacash_3dsecure_configuration_from_application_config
    {
        It should_provide_correct_environment = () =>
            Config.Environment.ShouldEqual(PaymentEnvironment.Test);

        It should_provide_correct_merchant_id = () =>
            Config.MerchantId.ShouldEqual("3dsMerchantId");

        It should_provide_correct_password = () =>
            Config.Password.ShouldEqual("3dsPassword");

        It should_provide_correct_merchant_url = () =>
            Config.MerchantUrl.ShouldEqual("3dsMerchantUrl");

        It should_provide_correct_purchase_description = () =>
            Config.PurchaseDescription.ShouldEqual("3dsPurchaseDescription");

        Because of = () =>
            Config = MoolahConfiguration.Current.DataCash3DSecure;

        static DataCash3DSecureConfiguration Config;
    }

    [Subject(typeof(MoolahConfiguration))]
    public class When_loading_paypal_configuration_from_application_config
    {
        It should_provide_correct_user_id = () =>
            Config.UserId.ShouldEqual("paypalUserId");

        It should_provide_correct_password = () =>
            Config.Password.ShouldEqual("paypalPassword");

        It should_provide_correct_signature = () =>
            Config.Signature.ShouldEqual("paypalSignature");

        Because of = () =>
            Config = MoolahConfiguration.Current.PayPal;

        static PayPalConfiguration Config;
    }
}