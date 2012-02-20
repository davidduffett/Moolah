namespace Moolah.Specs.DataCash
{
    public static class DataCashResponses
    {
        public const string AuthoriseResponseFormat =
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
    <reason></reason>
    <status>{1}</status>
    <time>1071567305</time>
</Response>";

        public const string AuthoriseResponseWithoutDataCashReference =
            @"
<Response>
    <CardTxn>
        <authcode>987654</authcode>
        <card_scheme>Mastercard</card_scheme>
        <country>United Kingdom</country>
        <issuer>HSBC</issuer>
    </CardTxn>
    <merchantreference>4567890</merchantreference>
    <mode>LIVE</mode>
    <reason></reason>
    <status>{0}</status>
    <time>1071567305</time>
</Response>";
    }
}