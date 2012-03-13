using System.Collections.Generic;

namespace Moolah.DataCash
{
    /// <summary>
    /// See https://testserver.datacash.com/software/returncodes.shtml for full list of DataCash status codes.
    /// </summary>
    public static class DataCashFailureReasons
    {
        /// <summary>
        /// Failures with messages that can be displayed directly to the customer.
        /// </summary>
        public static readonly Dictionary<int, DataCashFailureReason> CleanFailures = new Dictionary<int, DataCashFailureReason>
        {
            { DataCashStatus.NotAuthorised, new DataCashFailureReason("Transaction was declined by your bank. Please check your details or contact your bank and try again.", CardFailureType.General) },
            { 24, new DataCashFailureReason("The supplied expiry date is in the past.", CardFailureType.ExpiryDate) },
            { 25, new DataCashFailureReason("The card number is invalid.", CardFailureType.CardNumber) },
            { 26, new DataCashFailureReason("The card number does not have the expected number of digits.", CardFailureType.CardNumber) },
            { 27, new DataCashFailureReason("The card issue number is missing or invalid.", CardFailureType.IssueNumber) },
            { 28, new DataCashFailureReason("The card start date is missing or invalid.", CardFailureType.StartDate) },
            { 29, new DataCashFailureReason("The supplied start date is in the future.", CardFailureType.StartDate) },
            { 30, new DataCashFailureReason("The start date cannot be after the expiry date.", CardFailureType.StartDate) },
            { 53, new DataCashFailureReason("The transaction was not processed due to a high number of requests.  Please try again.", CardFailureType.General) },
            { 56, new DataCashFailureReason("The transaction was declined due to the card being used too recently.  Please try again later.", CardFailureType.General) },
            // 3D-Secure related
            { 179, new DataCashFailureReason("Transaction was declined by your bank. Please check your details or contact your bank and try again.", CardFailureType.General) },
            { 184, new DataCashFailureReason("The time limit for entering your password expired. You have not been charged. Please try again.", CardFailureType.General) },
            { 185, new DataCashFailureReason("The time limit for entering your password expired. You have not been charged. Please try again.", CardFailureType.General) }
        };

        /// <summary>
        /// Failures with messages that should not be displayed to the customer, but may indicate a system failure.
        /// </summary>
        public static readonly Dictionary<int, DataCashFailureReason> SystemFailures = new Dictionary<int, DataCashFailureReason>
        {
            { 2, new DataCashFailureReason("Communication was interrupted. The argument is e.g. 523/555 (523 bytes written but 555 expected).", CardFailureType.General) },
            { 3, new DataCashFailureReason("A timeout occurred while we were reading the transaction details.", CardFailureType.General) },
            { 5, new DataCashFailureReason("A field was specified twice, you sent us too much or invalid data, a pre-auth lookup failed during a fulfill transaction, the swipe field was incorrectly specified, or you omitted a field. The argument will give a better indication of what exactly went wrong.", CardFailureType.General) },
            { 6, new DataCashFailureReason("Error in communications link; resend.", CardFailureType.General) },
            { 9, new DataCashFailureReason("The currency you specified does not exist", CardFailureType.General) },
            { 10, new DataCashFailureReason("The vTID or password were incorrect", CardFailureType.General) },
            { 12, new DataCashFailureReason("The authcode you supplied was invalid", CardFailureType.General) },
            { 13, new DataCashFailureReason("You did not supply a transaction type.", CardFailureType.General) },
            { 14, new DataCashFailureReason("Transaction details could not be committed to our database.", CardFailureType.General) },
            { 15, new DataCashFailureReason("You specified an invalid transaction type.", CardFailureType.General) },
            { 19, new DataCashFailureReason("You attempted to fulfill a transaction that either could not be fulfilled (e.g. auth, refund) or already has been.", CardFailureType.General) },
            { 20, new DataCashFailureReason("A successful transaction has already been sent using this vTID and reference number.", CardFailureType.General) },
            { 21, new DataCashFailureReason("This terminal does not accept transactions for this type of card.", CardFailureType.General) },
            { 22, new DataCashFailureReason("Reference numbers should be 16 digits for fulfill transactions, or between 6 and 30 digits for all others.", CardFailureType.General) },
            { 23, new DataCashFailureReason("The expiry dates should be specified as MM/YY or MM-YY.", CardFailureType.General) },
            { 34, new DataCashFailureReason("The amount is missing, is not fully specified to x.xx format", CardFailureType.General) },
            { 51, new DataCashFailureReason("A transaction with this reference number is already going through the system.", CardFailureType.General) },
            { 59, new DataCashFailureReason("This combination of currency, card type and environment is not supported by this vTID", CardFailureType.General) },
            { 60, new DataCashFailureReason("The XML Document is not valid with our Request schema. The reason is detailed in the <information> element of the Response document.", CardFailureType.General) },
            { 61, new DataCashFailureReason("An error in account configuration caused the transaction to fail. Contact DataCash Technical Support", CardFailureType.General) },
            { 63, new DataCashFailureReason("The transaction type is not supported by the Acquirer", CardFailureType.General) },
            { 104, new DataCashFailureReason("Error in bank authorization, where APACS30 Response message refers to different TID to that used in APACS30 Request message; resend.", CardFailureType.General) },
            { 105, new DataCashFailureReason("Error in bank authorization, where APACS30 Response message refers to different message number to that used in APACS30 Request message; resend.", CardFailureType.General) },
            { 106, new DataCashFailureReason("Error in bank authorization, where APACS30 Response message refers to different amount to that used in APACS30 Request message; resend.", CardFailureType.General) },
            { 190, new DataCashFailureReason("Your vTID is capable of dealing with transactions from different environments (e.g. MoTo, e-comm), but you have not specified from which environment this transaction has taken place.", CardFailureType.General) },
            { 280, new DataCashFailureReason("The datacash reference should be a 16 digit number. The first digit (2, 9, 3 or 4) indicates the format used and whether the txn was processed in a live or test environment.", CardFailureType.General) },
            { 281, new DataCashFailureReason("The new format of datacash reference includes a luhn check digit. The number supplied failed to pass the luhn check.", CardFailureType.General) },
            { 282, new DataCashFailureReason("The site_id extracted from the datacash reference does not match the current environment", CardFailureType.General) },
            { 283, new DataCashFailureReason("The mode flag extracted from the datacash reference does not match the current environment", CardFailureType.General) },
            { 440, new DataCashFailureReason("Out of external connections", CardFailureType.General) },
            { 470, new DataCashFailureReason("Maestro transactions for Card Holder not present are not supported for the given clearinghouse", CardFailureType.General) },
            { 471, new DataCashFailureReason("This transaction must be a 3dsecure transaction", CardFailureType.General) },
            { 472, new DataCashFailureReason("International Maestro is not permitted in a Mail order / telephone order environment.", CardFailureType.General) },
            { 473, new DataCashFailureReason("Keyed International Maestro transaction not permitted", CardFailureType.General) },
            { 480, new DataCashFailureReason("The Merchant Id provided is invalid", CardFailureType.General) },
            { 481, new DataCashFailureReason("The merchant is expected to provide a Merchant Id with each transaction", CardFailureType.General) },
            { 482, new DataCashFailureReason("The merchant is not set to provide Merchant Id for a transaction", CardFailureType.General) },
            { 510, new DataCashFailureReason("The merchant attempted to use a GE Capital card with a BIN that does not belong to them", CardFailureType.General) },
            { 1100, new DataCashFailureReason("No referenced transaction found", CardFailureType.General) },
            { 1101, new DataCashFailureReason("Only referred transactions can be authorised", CardFailureType.General) },
            { 1102, new DataCashFailureReason("Only pre or auth transaction can be authorised", CardFailureType.General) },
            { 1103, new DataCashFailureReason("Must supply updated authcode to authorise transaction", CardFailureType.General) },
            { 1104, new DataCashFailureReason("Transactions cannot be authorized after time limit expired. The default timeout value is set to 6hours but can be amended per Vtid by contacting DataCash Support.", CardFailureType.General)},
            { 1105, new DataCashFailureReason("The transaction referenced was both referred by the acquiring bank, and challanged by the Retail Decisions (ReD) Service. This transaction cannot be authorised through the use of an authorize_referral_request or fulfill transaction, nor have the challenge accepted with an accept_fraud transaction. If a new authorisation code is obtained for the acquiring bank, the merchant can continue with the transaction by resubmitting the entrie transaction and providing the new authorisation code.", CardFailureType.General) },
            { 1106, new DataCashFailureReason("Historic reference already in use", CardFailureType.General) },
            { 12001, new DataCashFailureReason("There are more than one active passwords already registered against your vTID at the time the txn was received.", CardFailureType.General) },
            { 12002, new DataCashFailureReason("The IP address of the system submitting the vtidconfiguration request is not registered against your vTID.", CardFailureType.General) },
            // 3D-Secure Related
            { 151, new DataCashFailureReason("A transaction type other than 'auth' or 'pre' was received in the 3-D-Secure Enrolment Check Request", CardFailureType.General) },
            { 152, new DataCashFailureReason("An authcode was supplied in the 3-D Secure Authorization Request, this is not allowed", CardFailureType.General) },
            { 153, new DataCashFailureReason("The mandatory 'verify' element was not supplied in the 3-D Secure Enrolment Check Request", CardFailureType.General) },
            { 154, new DataCashFailureReason("The mandatory 'verify' element was supplied, but its value was something other than 'yes' or 'no'", CardFailureType.General) },
            { 155, new DataCashFailureReason("One of the required fields: 'merchant_url', 'purchase_datetime', 'purchase_desc' or 'device_category' was not supplied", CardFailureType.General) },
            { 156, new DataCashFailureReason("The required field 'device_category' was supplied, but contains a value other than 0 or 1", CardFailureType.General) },
            { 157, new DataCashFailureReason("The merchant is not setup to do 3-D Secure transactions", CardFailureType.General) },
            { 158, new DataCashFailureReason("The card scheme is not supported in the 3-D Secure environment", CardFailureType.General) },
            { 159, new DataCashFailureReason("The enrolment verification with scheme directory server failed", CardFailureType.General) },
            { 160, new DataCashFailureReason("Received an invalid response from the scheme directory server", CardFailureType.General) },
            { 161, new DataCashFailureReason("The 3-D Secure Authorization Request was not authorized and returned a referral response", CardFailureType.General) },
            { 163, new DataCashFailureReason("Not enabled for this card scheme with that acquiring bank", CardFailureType.General) },
            { 164, new DataCashFailureReason("The acquirer is not supported by 3-D Secure", CardFailureType.General) },
            { 165, new DataCashFailureReason("Not enabled to do 3-D Secure transactions with this acquirer", CardFailureType.General) },
            { 166, new DataCashFailureReason("The format of the 'purchase_datetime' field supplied in the 3-D Secure Enrolment Check Request is invalid", CardFailureType.General) },
            { 167, new DataCashFailureReason("An invalid reference was supplied in the 3-D Secure Authorization Request", CardFailureType.General) },
            { 168, new DataCashFailureReason("The transaction could not be submitted for authorization as no valid 3-D Secure Enrolment Check Request was found", CardFailureType.General) },
            { 169, new DataCashFailureReason("A magic card as not been supplied in transaction where 3-D Secure subscription mode is test", CardFailureType.General) },
            { 170, new DataCashFailureReason("The DataCash MPI software has no directory server URL details for this scheme", CardFailureType.General) },
            { 171, new DataCashFailureReason("A PARes message has been supplied in 3-D Secure Authorization Request. This is not allowed", CardFailureType.General) },
            { 172, new DataCashFailureReason("The required PARes message was not supplied in 3-D Secure Authorization Request", CardFailureType.General) },
            { 174, new DataCashFailureReason("The PARes response message from the Issuer could not be verified", CardFailureType.General) },
            { 175, new DataCashFailureReason("A PARes message has been received in 3-D Secure Authorization Request, but no Matching PAReq message was found", CardFailureType.General) },
            { 176, new DataCashFailureReason("The PARes message received in the 3-D Secure Authorization Request is invalid", CardFailureType.General) },
            { 177, new DataCashFailureReason("The PAReq message received from the 3-D Secure Enrolment Check Request is invalid", CardFailureType.General) },
            { 178, new DataCashFailureReason("The PAReq and PARes messages do not match on one of these key fields: 'message_id', 'acqBIN', 'merID', 'xid', 'date', 'purchAmount', 'currency' or 'exponent'", CardFailureType.General) },
            { 180, new DataCashFailureReason("The DataCash MPI does not support recurring transactions", CardFailureType.General) },
            { 181, new DataCashFailureReason("A 3-D Secure Authorization Request found there was no matching referral to be authorized", CardFailureType.General) },
            { 182, new DataCashFailureReason("A 3-D Secure Authorization Request found that the timelimit for an authorization on a transaction that had previously received a response had been exceeded", CardFailureType.General) },
            { 183, new DataCashFailureReason("A 3-D Secure Enrolment Check Request had the verify element set to 'no' meaning no 3-D Secure verification is to be done.", CardFailureType.General) },
            { 186, new DataCashFailureReason("3DS Invalid VEReq", CardFailureType.General) },
            { 187, new DataCashFailureReason("Unable to Verify", CardFailureType.General) },
            { 188, new DataCashFailureReason("Unable to Authenticate", CardFailureType.General) }
        };
    }
}