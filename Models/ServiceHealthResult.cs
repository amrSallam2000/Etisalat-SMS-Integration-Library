using System;
using System.Collections.Generic;

namespace Etisalat.SMS.Integration.Models
{
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
