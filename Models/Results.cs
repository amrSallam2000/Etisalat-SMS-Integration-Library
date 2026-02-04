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

    /// <summary>
    /// Result of service health check
    /// </summary>
    public class ServiceHealthResult
    {
        /// <summary>
        /// Indicates whether the service is healthy
        /// </summary>
        public bool IsHealthy { get; set; }

        /// <summary>
        /// Response time for the health check
        /// </summary>
        public TimeSpan ResponseTime { get; set; }

        /// <summary>
        /// Server URL that was checked
        /// </summary>
        public string ServerUrl { get; set; }

        /// <summary>
        /// Timestamp of the health check
        /// </summary>
        public DateTime CheckTime { get; set; }

        /// <summary>
        /// Error message if the health check failed
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// HTTP status code received from the server
        /// </summary>
        public int HttpStatusCode { get; set; }
    }
}