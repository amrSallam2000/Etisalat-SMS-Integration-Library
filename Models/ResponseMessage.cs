using Newtonsoft.Json;
using System.Collections.Generic;

namespace Etisalat.SMS.Integration.Models
{

    /// <summary>
    /// Main response message wrapper
    /// </summary>
    public class ResponseMessage
    {
        /// <summary>
        /// Response header with status information
        /// </summary>
        [JsonProperty("Header")]
        public ResponseHeader Header { get; set; }

        /// <summary>
        /// Response payload with actual data
        /// </summary>
        [JsonProperty("ResponsePayload")]
        public ResponsePayload ResponsePayload { get; set; }
    }

}
