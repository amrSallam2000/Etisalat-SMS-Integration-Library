using System;
using System.Collections.Generic;

namespace Etisalat.SMS.Integration.Models
{

 /// <summary>
    /// Result of bulk SMS sending operation
    /// </summary>
    public class BulkSmsResult
    {
        /// <summary>
        /// List of individual SMS sending results
        /// </summary>
        public List<SmsResult> Results { get; set; }

        /// <summary>
        /// Total number of SMS messages attempted to send
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Number of SMS messages sent successfully
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// Number of SMS messages that failed to send
        /// </summary>
        public int FailCount { get; set; }

        /// <summary>
        /// Success rate as a percentage
        /// </summary>
        public double SuccessRate { get; set; }

        /// <summary>
        /// Timestamp when the bulk operation completed
        /// </summary>
        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Returns a formatted summary of the bulk SMS result
        /// </summary>
        public override string ToString()
        {
            return $"Total: {TotalCount} | Success: {SuccessCount} | Failed: {FailCount} | Success Rate: {SuccessRate:F1}%";
        }
    }


}
