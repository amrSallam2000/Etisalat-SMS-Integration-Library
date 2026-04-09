using Newtonsoft.Json;
using System.Collections.Generic;

namespace Etisalat.SMS.Integration.Models
{
    /// <summary>
    /// Response header containing transaction details and status
    /// </summary>
    public class ResponseHeader
    {
        /// <summary>
        /// Unique transaction identifier
        /// </summary>
        [JsonProperty("TransactionId")]
        public string TransactionId { get; set; }

        /// <summary>
        /// Status of the request (e.g., "SUCCESS", "FAILURE")
        /// </summary>
        [JsonProperty("Status")]
        public string Status { get; set; }

        /// <summary>
        /// Status code (e.g., "00" for success)
        /// </summary>
        [JsonProperty("Code")]
        public string Code { get; set; }

        /// <summary>
        /// Status message description
        /// </summary>
        [JsonProperty("Message")]
        public string Message { get; set; }
    }


}
