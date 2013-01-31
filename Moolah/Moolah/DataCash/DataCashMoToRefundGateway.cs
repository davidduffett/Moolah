namespace Moolah.DataCash
{
    public interface IDataCashMotoRefundGateway
    {
    }

    public class DataCashMoToRefundGateway : RefundGateway, IDataCashMotoRefundGateway
    {
        public DataCashMoToRefundGateway()
            : base(MoolahConfiguration.Current.DataCashMoTo)
        {
        }
    }    
}