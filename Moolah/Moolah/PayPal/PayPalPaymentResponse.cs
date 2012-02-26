using System;
using System.Xml.Linq;

namespace Moolah.PayPal
{
    public class PayPalPaymentResponse : IPaymentResponse
    {
        public PayPalPaymentResponse(XDocument payPalResponse)
        {
            if (payPalResponse == null) throw new ArgumentNullException("payPalResponse");
            PayPalResponse = payPalResponse;
        }

        public XDocument PayPalResponse { get; private set; }

        public string TransactionReference { get; internal set; }

        public PaymentStatus Status { get; internal set; }

        public bool IsSystemFailure { get; internal set; }

        public string FailureMessage { get; internal set; }
    }
}