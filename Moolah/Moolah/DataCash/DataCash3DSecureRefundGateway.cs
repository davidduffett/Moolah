namespace Moolah.DataCash
{
    public interface IDataCash3DSecureRefundGateway
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