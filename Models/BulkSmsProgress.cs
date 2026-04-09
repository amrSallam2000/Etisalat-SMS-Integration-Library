using System;
using System.Collections.Generic;

namespace Etisalat.SMS.Integration.Models
{

   /// <summary>
    /// Progress information for bulk SMS sending operations
    /// </summary>
    public class BulkSmsProgress
    {
        /// <summary>
        /// Total number of SMS messages to send
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Number of SMS messages already processed
        /// </summary>
        public int ProcessedCount { get; set; }

        /// <summary>
        /// Number of SMS messages sent successfully
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// Number of SMS messages that failed to send
        /// </summary>
        public int FailCount { get; set; }

        /// <summary>
        /// Current phone number being processed
        /// </summary>
        public string CurrentPhone { get; set; }

        /// <summary>
        /// Result of the last SMS sending operation
        /// </summary>
        public SmsResult LastResult { get; set; }

        /// <summary>
        /// Progress percentage (0-100)
        /// </summary>
        public double ProgressPercentage => TotalCount > 0
            ? (ProcessedCount * 100.0 / TotalCount)
            : 0;
    }

}
