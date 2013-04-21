using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Moolah.PayPal
{
    public interface IPayPalRecurringResponse : IPaymentResponse
    {
        string ProfileId { get; set; }
        string ProfileStatus { get; set; }
    }
    public class PayPalRecurringProfileResponse : IPayPalRecurringResponse
    {

        public string TransactionReference { get; private set; }

        public PaymentStatus Status { get; internal set; }

        public bool IsSystemFailure { get; internal set; }

        public string FailureMessage { get; internal set; }

        public string ProfileId { get; set; }
        public string ProfileStatus { get; set; }
    }
}
