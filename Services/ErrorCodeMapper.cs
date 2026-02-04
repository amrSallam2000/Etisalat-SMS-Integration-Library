using System.Collections.Generic;

namespace Etisalat.SMS.Integration.Services
{
    /// <summary>
    /// Error code mapper from Etisalat documentation
    /// </summary>
    public static class ErrorCodeMapper
    {
        private static readonly Dictionary<string, string> _errorMessages = new Dictionary<string, string>
        {
            // Success
            { "ESB-00000", "Success" },

            // Authorization / Account errors
            { "ESB-69011", "Agent not found" },
            { "ESB-69012", "Agent not active" },
            { "ESB-69013", "Agent is available only through SOAP call" },
            { "ESB-69016", "Account status must be ACTIVE" },

            // Template errors
            { "ESB-69018", "Template not found" },
            { "ESB-69019", "Template does not belong to caller account" },
            { "ESB-69021", "Template contains parameters, expecting parameters in request" },
            { "ESB-69022", "Parameter is not provided in request" },

            // Quota / Balance
            { "ESB-69023", "Not enough balance" },

            // Missing mandatory fields
            { "ESB-69026", "Transaction id is mandatory" },
            { "ESB-69027", "Authorization token is mandatory" },
            { "ESB-69029", "MSISDN is mandatory" },
            { "ESB-69030", "Template id is mandatory" },

            // Invalid values
            { "ESB-69031", "Invalid MSISDN" },
            { "ESB-69036", "Illegal input" },

            // Unknown / general errors
            { "ESB-99999", "Unknown Error Happened" },
            { "ESB-9999", "Unknown error" },

            // System / internal errors
            { "ESB-0030", "Database Error" },
            { "ESB-00036", "BW Engine Error" },
            { "ESB-00031", "Error in BW Process" },
            { "ESB-00037", "Checkpoint Error" },
            { "ESB-00038", "Error in Fault Tolerant Setup" },
            { "ESB-00050", "Connection Time Out Error" },
            { "ESB-00051", "Input Data Invalid" },
            { "ESB-00052", "Output Data Invalid" },
            { "ESB-00053", "Variable name not specified" },
            { "ESB-00054", "Mapping Error" },
            { "ESB-00042", "Transaction Error" },
            { "ESB-00041", "Error in Engine command" },
            { "ESB-00034", "Library Error" },
            { "ESB-00035", "Catch Activity Error" },
            { "ESB-00043", "Transition condition XPath has an error" },
            { "ESB-00039", "Expression Error" },
            { "ESB-00040", "Error in BW Log" }
        };

        /// <summary>
        /// Get all error messages
        /// </summary>
        public static Dictionary<string, string> GetErrorMessages()
        {
            return new Dictionary<string, string>(_errorMessages);
        }

        /// <summary>
        /// Get error message from error code
        /// </summary>
        public static string GetErrorMessage(string errorCode)
        {
            if (string.IsNullOrEmpty(errorCode))
                return "Unknown error";

            return _errorMessages.ContainsKey(errorCode)
                ? _errorMessages[errorCode]
                : $"Unknown error code: {errorCode}";
        }

        /// <summary>
        /// Check if the code indicates success
        /// </summary>
        public static bool IsSuccessCode(string errorCode)
        {
            return errorCode == "ESB-00000";
        }

        /// <summary>
        /// Check if the code indicates authentication error
        /// </summary>
        public static bool IsAuthenticationError(string errorCode)
        {
            return errorCode == "ESB-69011" ||
                   errorCode == "ESB-69012" ||
                   errorCode == "ESB-69013" ||
                   errorCode == "ESB-69016";
        }

        /// <summary>
        /// Check if the code indicates template error
        /// </summary>
        public static bool IsTemplateError(string errorCode)
        {
            return errorCode == "ESB-69018" ||
                   errorCode == "ESB-69019" ||
                   errorCode == "ESB-69021" ||
                   errorCode == "ESB-69022";
        }

        /// <summary>
        /// Check if the code indicates insufficient balance
        /// </summary>
        public static bool IsBalanceError(string errorCode)
        {
            return errorCode == "ESB-69023";
        }

        /// <summary>
        /// Check if the code indicates validation error
        /// </summary>
        public static bool IsValidationError(string errorCode)
        {
            return errorCode == "ESB-69026" ||
                   errorCode == "ESB-69027" ||
                   errorCode == "ESB-69029" ||
                   errorCode == "ESB-69030" ||
                   errorCode == "ESB-69031" ||
                   errorCode == "ESB-69036";
        }

        /// <summary>
        /// Check if the code indicates system error
        /// </summary>
        public static bool IsSystemError(string errorCode)
        {
            return errorCode?.StartsWith("ESB-00") == true && errorCode != "ESB-00000";
        }
    }
}