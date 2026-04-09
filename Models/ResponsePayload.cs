using Newtonsoft.Json;
using System.Collections.Generic;

namespace Etisalat.SMS.Integration.Models
{
/ =============== Response Models ===============
    /// <summary>
    /// Response payload containing actual SMS sending result
    /// </summary>
    public class ResponsePayload
    {
        /// <summary>
        /// SMS sending response data
        /// </summary>
        [JsonProperty("sendSMSResponse")]
        public SendSMSResponseData SendSMSResponse { get; set; }
    }

}
