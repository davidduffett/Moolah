namespace Moolah
{
    public interface IThreeDSecureResponse : IPaymentResponse
    {
        /// <summary>
        /// Access Control Server (ACS) URL to POST 3D-Secure requests to.
        /// </summary>
        string ACSUrl { get; }

        /// <summary>
        /// Payer Authentication Request (PAReq) which should be posted to the ACS.
        /// </summary>
        string PAReq { get; }
    }
}