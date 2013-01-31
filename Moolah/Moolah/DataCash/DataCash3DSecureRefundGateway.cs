namespace Moolah.DataCash
{
    public interface IDataCash3DSecureRefundGateway : IRefundGateway
    {
    }

    public class DataCash3DSecureRefundGateway : RefundGateway, IDataCash3DSecureRefundGateway
    {
        public DataCash3DSecureRefundGateway()
            : base(MoolahConfiguration.Current.DataCash3DSecure)
        {
        }
    }    
}