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

        public const string ThreeDSecureAuthenticationRequiredResponse =
            @"
<Response>
    <status>150</status>
    <reason>3DS Payer Verification Required</reason>
    <merchantreference>4567890</merchantreference>
    <datacash_reference>{0}</datacash_reference>
    <CardTxn>
        <ThreeDSecure>
            <pareq_message>{1}</pareq_message>
            <acs_url>{2}</acs_url>
        </ThreeDSecure>
    </CardTxn>
</Response>";

        public const string RefundResponseFormat =
            @"
<Response>
    <datacash_reference>{0}</datacash_reference>
    <HistoricTxn>
        <authcode>896876</authcode>
    </HistoricTxn>
    <merchantreference>4100000088888888</merchantreference>
    <mode>LIVE</mode>
    <reason>ACCEPTED</reason>
    <status>{1}</status>
    <time>1071567375</time>
</Response>";
    }
}