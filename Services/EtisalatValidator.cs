using Etisalat.SMS.Integration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Etisalat.SMS.Integration.Services
{
    /// <summary>
    /// Data validation tool according to Etisalat API requirements
    /// </summary>
    public static class EtisalatValidator
    {
        /// <summary>
        /// Validate Transaction ID
        /// Pattern: [a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}
        /// Max Length: 36 characters
        /// </summary>
        public static bool IsValidTransactionId(string transactionId)
        {
            if (string.IsNullOrEmpty(transactionId))
                return false;

            if (transactionId.Length > 36)
                return false;

            var pattern = @"^[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}$";
            return Regex.IsMatch(transactionId, pattern);
        }

        /// <summary>
        /// Validate Egyptian phone number
        /// Format: 20XXXXXXXXXX (12 digits total)
        /// </summary>
        public static bool IsValidEgyptianPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            return Regex.IsMatch(phoneNumber, @"^20[0-9]{10}$");
        }

        /// <summary>
        /// Validate Template ID
        /// </summary>
        public static bool IsValidTemplateId(string templateId)
        {
            return !string.IsNullOrWhiteSpace(templateId);
        }

        /// <summary>
        /// Validate Authorization Token
        /// </summary>
        public static bool IsValidAuthToken(string authToken)
        {
            return !string.IsNullOrWhiteSpace(authToken);
        }

        /// <summary>
        /// Validate API settings
        /// </summary>
        public static bool IsValidConfiguration(EtisalatApiConfiguration config)
        {
            return config != null &&
                   !string.IsNullOrWhiteSpace(config.Username) &&
                   !string.IsNullOrWhiteSpace(config.Password) &&
                   !string.IsNullOrWhiteSpace(config.ApiKey) &&
                   Guid.TryParse(config.ApiKey, out _);
        }

        /// <summary>
        /// Comprehensive validation of SMS request
        /// </summary>
        public static ValidationResult ValidateSmsRequest(SendSmsRequest request)
        {
            var errors = new List<string>();

            if (request == null)
            {
                errors.Add("SMS request is required");
                return new ValidationResult { IsValid = false, Errors = errors };
            }

            // Validate Transaction ID
            if (!IsValidTransactionId(request.Id))
                errors.Add("Invalid or missing Transaction ID");

            // Validate Message Type
            if (request.MessageType != "SMS")
                errors.Add("MessageType must be 'SMS'");

            // Validate Characteristics
            if (request.Characteristic == null || !request.Characteristic.Any())
            {
                errors.Add("Characteristics are required");
            }
            else
            {
                // Validate Authorization
                var authChar = request.Characteristic.FirstOrDefault(c => c.Name == "Authorization");
                if (authChar == null || string.IsNullOrWhiteSpace(authChar.Value))
                    errors.Add("Authorization characteristic is required");

                // Validate either templateID or body exists
                var templateChar = request.Characteristic.FirstOrDefault(c => c.Name == "templateID");
                var bodyChar = request.Characteristic.FirstOrDefault(c => c.Name == "body");

                if (templateChar == null && bodyChar == null)
                    errors.Add("Either templateID or body characteristic must be present");
            }

            // Validate Receivers
            if (request.Receiver == null || !request.Receiver.Any())
            {
                errors.Add("Receiver is required");
            }
            else
            {
                foreach (var receiver in request.Receiver)
                {
                    if (!IsValidEgyptianPhoneNumber(receiver.PhoneNumber))
                        errors.Add($"Invalid phone number: {receiver.PhoneNumber}");
                }
            }

            return new ValidationResult
            {
                IsValid = !errors.Any(),
                Errors = errors
            };
        }

        /// <summary>
        /// Validate template parameters
        /// </summary>
        public static ValidationResult ValidateTemplateParameters(SendSmsRequest request)
        {
            var errors = new List<string>();

            var templateChar = request.Characteristic?.FirstOrDefault(c => c.Name == "templateID");
            if (templateChar != null)
            {
                var bodyChar = request.Characteristic?.FirstOrDefault(c => c.Name == "body");

                // For empty templates, body is required
                if (string.IsNullOrWhiteSpace(templateChar.Value) && bodyChar == null)
                {
                    errors.Add("For empty templates, 'body' parameter is required");
                }
            }

            return new ValidationResult
            {
                IsValid = !errors.Any(),
                Errors = errors
            };
        }
    }
}