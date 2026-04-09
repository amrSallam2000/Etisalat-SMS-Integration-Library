using Newtonsoft.Json;
using System.Collections.Generic;

namespace Etisalat.SMS.Integration.Models
{

// =============== Request Models ===============

    /// <summary>
    /// Represents an SMS send request to Etisalat API
    /// </summary>
    public class SendSmsRequest
    {
        /// <summary>
        /// Unique identifier for the message (usually GUID)
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Type of message (default: "SMS")
        /// </summary>
        [JsonProperty("messageType")]
        public string MessageType { get; set; } = "SMS";

        /// <summary>
        /// Message characteristics like template ID, token, etc.
        /// </summary>
        [JsonProperty("characteristic")]
        public List<Characteristic> Characteristic { get; set; }

        /// <summary>
        /// List of recipients
        /// </summary>
        [JsonProperty("receiver")]
        public List<Receiver> Receiver { get; set; }
    }
}
