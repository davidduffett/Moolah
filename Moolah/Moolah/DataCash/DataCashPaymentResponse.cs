using System;
using System.Xml.Linq;

namespace Moolah.DataCash
{
    public class CardPaymentPaymentResponse : ICardPaymentResponse
    {
        public CardPaymentPaymentResponse(XDocument dataCashResponse)
        {
            if (dataCashResponse == null) throw new ArgumentNullException("dataCashResponse");
            DataCashResponse = dataCashResponse;
        }

        public XDocument DataCashResponse { get; private set; }

        public string TransactionReference { get; internal set; }

        public PaymentStatus Status { get; internal set; }

        public bool IsSystemFailure { get; internal set; }

        public string FailureMessage { get; internal set; }

        public CardFailureType FailureType { get; internal set; }

        public string AvsCv2Result { get; set; }
    }

    public class DataCash3DSecurePaymentResponse : CardPaymentPaymentResponse, I3DSecureResponse
    {
        public DataCash3DSecurePaymentResponse(XDocument dataCashResponse) 
            : base(dataCashResponse)
        {
        }

        public bool Requires3DSecurePayerVerification { get; internal set; }

        public string ACSUrl { get; internal set; }

        public string PAReq { get; internal set; }
    }

    public static class DataCashResponseExtensions
    {
        /// <summary>
        /// Indicates whether Moolah can authorise without 3D-Secure payer verification.
        /// </summary>
        public static bool CanAuthorize(this I3DSecureResponse response)
        {
            return response.Status == PaymentStatus.Pending && !response.Requires3DSecurePayerVerification;
        }
    }
}