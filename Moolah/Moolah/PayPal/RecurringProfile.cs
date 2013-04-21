using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moolah.PayPal
{
    [Serializable]
    public class RecurringProfile
    {
        public RecurringProfile()
        {
            CurrencyCodeType = CurrencyCodeType.GBP;
            Description = String.Empty;
            BillingPeriod = RecurringPeriod.Month;
            BillingFrequency = 12;
            StartDate = DateTime.Now.AddMonths(1);
        }

        /// <summary>
        /// Currency to use.  Defaults to GBP.
        /// </summary>
        public CurrencyCodeType CurrencyCodeType { get; set; }

        /// <summary>
        /// Profile description.  Defaults to String.Empty.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// The unit of measure for the billing cycle.  Defaults to Month.
        /// </summary>
        public RecurringPeriod BillingPeriod { get; set; }

        /// <summary>
        /// The number of billing periods that make up one billing cycle. The combination of billing frequency and billing period must be less than or equal to one year.
        /// NOTE:If the billing period is SemiMonth, the billing frequency must be 1.  Defaults to 12.
        /// </summary>
        public int BillingFrequency { get; set; }

        /// <summary>
        /// Amount to bill for each billing cycle.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Payer Email.
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// Item Name.
        /// </summary>
        public String ItemName { get; set; }

        /// <summary>
        /// Profile start date.The date when billing for this profile begins.
        /// NOTE:The profile may take up to 24 hours for activation.
        /// </summary>
        public DateTime StartDate { get; set; }
    }
}
