using System;

namespace Moolah.PayPal
{
    public class PayReceiver
    {
        const int MaxNoteLength = 4000;
        const int MaxUniqueIdLength = 30;

        /// <param name="receiverId">It's either email or unique PayPal customer account number. In a single request you can only use one type.</param>
        /// <param name="amount">Payment amount.</param>
        /// <param name="uniqueId">Transaction-specific identification number for tracking in an accounting system. Character length and limitations: 30 single-byte characters. No whitespace allowed. Optional.</param>
        /// <param name="note">Custom note for each recipient. Character length and limitations: 4,000 single-byte alphanumeric characters. Optional.</param>
        public PayReceiver(string receiverId, decimal amount, string uniqueId = null, string note = null)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException("amount", "Amount must be greater than zero.");
            if (string.IsNullOrWhiteSpace(receiverId)) throw new ArgumentNullException("receiverId");
            if (uniqueId != null && uniqueId.IndexOf(' ') >= 0) throw new ArgumentException("UniqueId cannot contain white spaces.", "uniqueId");
            if (uniqueId != null && uniqueId.Length > MaxUniqueIdLength) throw new ArgumentOutOfRangeException("uniqueId", "UniqueId cannot be longer than 30 characters.");
            if (note != null && note.Length > MaxNoteLength) throw new ArgumentOutOfRangeException("note", "Note cannot be longer than 4000 characters.");

            ReceiverId = receiverId;
            Amount = amount;
            UniqueId = uniqueId;
            Note = note;
        }

        public string ReceiverId { get; protected set; }
        public decimal Amount { get; protected set; }
        public string UniqueId { get; protected set; }
        public string Note { get; protected set; }
    }
}