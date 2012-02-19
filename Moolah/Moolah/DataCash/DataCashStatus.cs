namespace Moolah.DataCash
{
    /// <summary>
    /// Subset of Datacash statuses.
    /// See https://testserver.datacash.com/software/returncodes.shtml for full list.
    /// </summary>
    public enum DataCashStatus
    {
        // General statuses
        Success = 1,
        NotAuthorised = 7,
        // Communication errors
        SocketWriteError = 2,
        Timeout = 3,
        EditError = 5,
        CommsError = 6,
        // User Authorisation errors
        AuthenticationError = 10,
        InvalidvTID = 52,
        // Card Authorisation errors
        InvalidCardType = 21,
        CardExpired = 24,
        CardNumberInvalid = 25,
        CardNumberWrongLength = 26,
        IssueNumberError = 27,
        StartDateError = 28,
        StartDateInFuture = 29,
        StartDateAfterExpiryDate = 30,
        CardUsedTooRecently = 56,
        Cv2NumberLength = 132,
        IssueSizeError = 16,
        SwitchStdError = 17,
        // Invalid User Data
        CurrencyError = 9,
        InvalidAuthorisationCode = 12,
        CannotFulfillTransaction = 19,
        DuplicateTransactionReference = 20,
        InvalidReference = 22,
        ExpiryDateInvalid = 23,
        ReferenceInUse = 51,
        CurrencyNotSupportedByvTID = 58,
        CurrencyNotSupportedByvTID2 = 59,
        NoIssueWanted = 32,
        // Malformed document
        TypeFieldMissing = 13,
        InvalidType = 15,
        InvalidAmount = 34,
        InvalidXml = 60, // also caused by non-UK postcodes
        Cv2StandardAndExtendedPolicy = 130,
        InvalidExtendedPolicy = 131,
        ExtendedPolicyNotSupported = 133,
        // Datacash error
        DatabaseServerError = 14,
        NoFreeTIDs = 53,
        PreAuthFailed = 11,
        // Temporary Bank Auth errors
        APACS30_0 = 104,
        APACS30_1 = 105,
        APACS30_2 = 106,
        // 3-D Secure is required for this card (International Maestro)
        ThreeDSecureRequired = 471
    }
}