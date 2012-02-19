using Moolah.DataCash;

namespace Moolah.Specs.DataCash
{
    public static class DataCashResponses
    {
        private const string AuthoriseResponseFormat =
            @"
<Response>
    <CardTxn>
        <authcode>987654</authcode>
        <card_scheme>Mastercard</card_scheme>
        <country>United Kingdom</country>
        <issuer>HSBC</issuer>
    </CardTxn>
    <datacash_reference>{0}</datacash_reference>
    <merchantreference>4567890</merchantreference>
    <mode>LIVE</mode>
    <reason>{1}</reason>
    <status>{2}</status>
    <time>1071567305</time>
</Response>";

        public const string DataCashReference = "3000000088888888";

        public static readonly string Authorised = string.Format(AuthoriseResponseFormat, 
            DataCashReference, DataCashReason.ACCEPTED, DataCashStatus.Success);

        public static readonly string NotAuthorised = string.Format(AuthoriseResponseFormat,
            DataCashReference, DataCashReason.REFERRED, DataCashStatus.NotAuthorised);
    }
}