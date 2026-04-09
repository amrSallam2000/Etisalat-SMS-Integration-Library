using Newtonsoft.Json;
using System.Collections.Generic;

namespace Etisalat.SMS.Integration.Models
{
    /// <summary>
    /// Represents the complete response from Etisalat API when sending SMS
    /// </summary>
    public class SendSmsResponse
    {
        /// <summary>
        /// Response message containing header and payload
        /// </summary>
        [JsonProperty("ResponseMessage")]
        public ResponseMessage ResponseMessage { get; set; }
    }

}
