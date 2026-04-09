using System;
using System.Collections.Generic;

namespace Etisalat.SMS.Integration.Models
{
    /// <summary>
    /// Result of a single SMS message sending operation
    /// </summary>
    public class SmsResult
    {
        /// <summary>
        /// Recipient phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Indicates whether the SMS was sent successfully
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Tracking ID for the sent message (provided by Etisalat API)
        /// </summary>
        public string TrackingId { get; set; }

        /// <summary>
        /// Transaction ID for the operation
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// Response code from Etisalat API
        /// </summary>
        public string ResponseCode { get; set; }

        /// <summary>
        /// Error message if the operation failed
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Raw response object from Etisalat API
        /// </summary>
        public SendSmsResponse RawResponse { get; set; }

        /// <summary>
        /// Exception that occurred during the operation (if any)
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Timestamp when the SMS was sent
        /// </summary>
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
