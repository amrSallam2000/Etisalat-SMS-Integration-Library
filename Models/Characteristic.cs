using Newtonsoft.Json;
using System.Collections.Generic;

namespace Etisalat.SMS.Integration.Models
{

    /// <summary>
    /// Represents a characteristic/property of the SMS message
    /// Used for template parameters, tokens, etc.
    /// </summary>
    public class Characteristic
    {
        /// <summary>
        /// Characteristic name (e.g., "authToken", "templateId", "param1")
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Characteristic value
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
    }

}
