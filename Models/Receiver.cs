using Newtonsoft.Json;
using System.Collections.Generic;

namespace Etisalat.SMS.Integration.Models
{
    /// <summary>
    /// Represents a message recipient
    /// </summary>
    public class Receiver
    {
        /// <summary>
        /// Recipient phone number (with country code, e.g., "201234567890")
        /// </summary>
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
    }

}
