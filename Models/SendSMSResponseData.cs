using Newtonsoft.Json;
using System.Collections.Generic;

namespace Etisalat.SMS.Integration.Models
{
    /// <summary>
    /// SMS sending response data
    /// </summary>
    public class SendSMSResponseData
    {
        /// <summary>
        /// Tracking ID for the sent message (for future reference)
        /// </summary>
        [JsonProperty("trackingID")]
        public string TrackingID { get; set; }
    }

}
