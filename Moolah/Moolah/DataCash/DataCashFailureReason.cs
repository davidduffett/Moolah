namespace Moolah.DataCash
{
    public class DataCashFailureReason
    {
        public DataCashFailureReason(string message, CardFailureType failureType)
        {
            Message = message;
            Type = failureType;
        }

        public string Message { get; protected set; }
        public CardFailureType Type { get; protected set; }
    }
}