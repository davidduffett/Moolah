using System.Collections.Generic;

namespace Moolah.DataCash
{
    /// <summary>
    /// See https://testserver.datacash.com/software/returncodes.shtml for full list of DataCash status codes.
    /// </summary>
    public static class DataCashFailureMessages
    {
        /// <summary>
        /// Failures with messages that can be displayed directly to the customer.
        /// </summary>
        public static readonly Dictionary<int, string> CleanFailures = new Dictionary<int, string>
        {
            { DataCashStatus.NotAuthorised, "Transaction was declined by your bank. Please check your details or contact your bank and try again." },
            { 24, "The supplied expiry date is in the past." },
            { 25, "The card number does not pass the standard Luhn checksum test." },
            { 26, "The card number does not have the expected number of digits." },
            { 27, "The card issue number is missing or invalid." },
            { 28, "The card start date is missing or invalid." },
            { 29, "The supplied start date is in the future." },
            { 30, "The start date cannot be after the expiry date." },
            { 53, "The transaction was not processed due to a high number of requests.  Please try again." },
            { 56, "The transaction was declined due to the card being used too recently.  Please try again later." },
            // 3D-Secure related
            { 179, "Transaction was declined by your bank. Please check your details or contact your bank and try again." },
            { 184, "The time limit for entering your password expired.  You have not been charged.  Please try again." },
            { 185, "The time limit for entering your password expired.  You have not been charged.  Please try again." }
        };

        /// <summary>
        /// Failures with messages that should not be displayed to the customer, but may indicate a system failure.
        /// </summary>
        public static readonly Dictionary<int, string> SystemFailures = new Dictionary<int, string>
        {
            { 2, "Communication was interrupted. The argument is e.g. 523/555 (523 bytes written but 555 expected)." },
            { 3, "A timeout occurred while we were reading the transaction details." },
            { 5, "A field was specified twice, you sent us too much or invalid data, a pre-auth lookup failed during a fulfill transaction, the swipe field was incorrectly specified, or you omitted a field. The argument will give a better indication of what exactly went wrong." },
            { 6, "Error in communications link; resend." },
            { 9, "The currency you specified does not exist" },
            { 10, "The vTID or password were incorrect" },
            { 12, "The authcode you supplied was invalid" },
            { 13, "You did not supply a transaction type." },
            { 14, "Transaction details could not be committed to our database." },
            { 15, "You specified an invalid transaction type." },
            { 19, "You attempted to fulfill a transaction that either could not be fulfilled (e.g. auth, refund) or already has been." },
            { 20, "A successful transaction has already been sent using this vTID and reference number." },
            { 21, "This terminal does not accept transactions for this type of card." },
            { 22, "Reference numbers should be 16 digits for fulfill transactions, or between 6 and 30 digits for all others." },
            { 23, "The expiry dates should be specified as MM/YY or MM-YY." },
            { 34, "The amount is missing, is not fully specified to x.xx format" },
            { 51, "A transaction with this reference number is already going through the system." },
            { 59, "This combination of currency, card type and environment is not supported by this vTID" },
            { 60, "The XML Document is not valid with our Request schema. The reason is detailed in the <information> element of the Response document." },
            { 61, "An error in account configuration caused the transaction to fail. Contact DataCash Technical Support" },
            { 63, "The transaction type is not supported by the Acquirer" },
            { 104, "Error in bank authorization, where APACS30 Response message refers to different TID to that used in APACS30 Request message; resend." },
            { 105, "Error in bank authorization, where APACS30 Response message refers to different message number to that used in APACS30 Request message; resend." },
            { 106, "Error in bank authorization, where APACS30 Response message refers to different amount to that used in APACS30 Request message; resend." },
            { 190, "Your vTID is capable of dealing with transactions from different environments (e.g. MoTo, e-comm), but you have not specified from which environment this transaction has taken place." },
            { 280, "The datacash reference should be a 16 digit number. The first digit (2, 9, 3 or 4) indicates the format used and whether the txn was processed in a live or test environment." },
            { 281, "The new format of datacash reference includes a luhn check digit. The number supplied failed to pass the luhn check." },
            { 282, "The site_id extracted from the datacash reference does not match the current environment" },
            { 283, "The mode flag extracted from the datacash reference does not match the current environment" },
            { 440, "Out of external connections" },
            { 470, "Maestro transactions for Card Holder not present are not supported for the given clearinghouse" },
            { 471, "This transaction must be a 3dsecure transaction" },
            { 472, "International Maestro is not permitted in a Mail order / telephone order environment." },
            { 473, "Keyed International Maestro transaction not permitted" },
            { 480, "The Merchant Id provided is invalid" },
            { 481, "The merchant is expected to provide a Merchant Id with each transaction" },
            { 482, "The merchant is not set to provide Merchant Id for a transaction" },
            { 510, "The merchant attempted to use a GE Capital card with a BIN that does not belong to them" },
            { 1100, "No referenced transaction found" },
            { 1101, "Only referred transactions can be authorised" },
            { 1102, "Only pre or auth transaction can be authorised" },
            { 1103, "Must supply updated authcode to authorise transaction" },
            { 1104, "Transactions cannot be authorized after time limit expired. The default timeout value is set to 6hours but can be amended per Vtid by contacting DataCash Support."},
            { 1105, "The transaction referenced was both referred by the acquiring bank, and challanged by the Retail Decisions (ReD) Service. This transaction cannot be authorised through the use of an authorize_referral_request or fulfill transaction, nor have the challenge accepted with an accept_fraud transaction. If a new authorisation code is obtained for the acquiring bank, the merchant can continue with the transaction by resubmitting the entrie transaction and providing the new authorisation code." },
            { 1106, "Historic reference already in use" },
            { 12001, "There are more than one active passwords already registered against your vTID at the time the txn was received." },
            { 12002, "The IP address of the system submitting the vtidconfiguration request is not registered against your vTID." },
            // 3D-Secure Related
            { 151, "A transaction type other than 'auth' or 'pre' was received in the 3-D-Secure Enrolment Check Request" },
            { 152, "An authcode was supplied in the 3-D Secure Authorization Request, this is not allowed" },
            { 153, "The mandatory 'verify' element was not supplied in the 3-D Secure Enrolment Check Request" },
            { 154, "The mandatory 'verify' element was supplied, but its value was something other than 'yes' or 'no'" },
            { 155, "One of the required fields: 'merchant_url', 'purchase_datetime', 'purchase_desc' or 'device_category' was not supplied" },
            { 156, "The required field 'device_category' was supplied, but contains a value other than 0 or 1" },
            { 157, "The merchant is not setup to do 3-D Secure transactions" },
            { 158, "The card scheme is not supported in the 3-D Secure environment" },
            { 159, "The enrolment verification with scheme directory server failed" },
            { 160, "Received an invalid response from the scheme directory server" },
            { 161, "The 3-D Secure Authorization Request was not authorized and returned a referral response" },
            { 163, "Not enabled for this card scheme with that acquiring bank" },
            { 164, "The acquirer is not supported by 3-D Secure" },
            { 165, "Not enabled to do 3-D Secure transactions with this acquirer" },
            { 166, "The format of the 'purchase_datetime' field supplied in the 3-D Secure Enrolment Check Request is invalid" },
            { 167, "An invalid reference was supplied in the 3-D Secure Authorization Request" },
            { 168, "The transaction could not be submitted for authorization as no valid 3-D Secure Enrolment Check Request was found" },
            { 169, "A magic card as not been supplied in transaction where 3-D Secure subscription mode is test" },
            { 170, "The DataCash MPI software has no directory server URL details for this scheme" },
            { 171, "A PARes message has been supplied in 3-D Secure Authorization Request. This is not allowed" },
            { 172, "The required PARes message was not supplied in 3-D Secure Authorization Request" },
            { 174, "The PARes response message from the Issuer could not be verified" },
            { 175, "A PARes message has been received in 3-D Secure Authorization Request, but no Matching PAReq message was found" },
            { 176, "The PARes message received in the 3-D Secure Authorization Request is invalid" },
            { 177, "The PAReq message received from the 3-D Secure Enrolment Check Request is invalid" },
            { 178, "The PAReq and PARes messages do not match on one of these key fields: 'message_id', 'acqBIN', 'merID', 'xid', 'date', 'purchAmount', 'currency' or 'exponent'" },
            { 180, "The DataCash MPI does not support recurring transactions" },
            { 181, "A 3-D Secure Authorization Request found there was no matching referral to be authorized" },
            { 182, "A 3-D Secure Authorization Request found that the timelimit for an authorization on a transaction that had previously received a response had been exceeded" },
            { 183, "A 3-D Secure Enrolment Check Request had the verify element set to 'no' meaning no 3-D Secure verification is to be done." },
            { 186, "3DS Invalid VEReq" },
            { 187, "Unable to Verify" },
            { 188, "Unable to Authenticate" }
        };
    }
}