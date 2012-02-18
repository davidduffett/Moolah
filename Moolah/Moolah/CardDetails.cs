namespace Moolah
{
    /// <summary>
    /// TODO: Validation of card details (LUHN check, date formats, lengths, etc).
    /// </summary>
    public struct CardDetails
    {
        public string Number { get; set; }
        public string ExpiryDate { get; set; }
        public string Cv2 { get; set; }
        public string StartDate { get; set; }
        public string IssueNumber { get; set; }
    }
}