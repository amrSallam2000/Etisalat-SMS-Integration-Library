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

    // =============== Response Models ===============

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

    // =============== Validation Models ===============

    /// <summary>
    /// Represents the result of a validation operation
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Indicates whether validation passed
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// List of validation errors (if any)
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// Returns a formatted string representation of the validation result
        /// </summary>
        public override string ToString()
        {
            if (IsValid)
                return "✅ Validation successful";

            return $"❌ Validation failed:\n  - {string.Join("\n  - ", Errors)}";
        }
    }
}