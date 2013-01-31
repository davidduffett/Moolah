namespace Moolah.DataCash
{
    public interface IDataCashMoToRefundGateway : IRefundGateway
    {
    }

    public class DataCashMoToRefundGateway : RefundGateway, IDataCashMoToRefundGateway
    {
        public DataCashMoToRefundGateway()
            : base(MoolahConfiguration.Current.DataCashMoTo)
        {
        }
    }    
}