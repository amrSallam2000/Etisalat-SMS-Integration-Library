using Newtonsoft.Json;
using System.Collections.Generic;

namespace Etisalat.SMS.Integration.Models
{
    
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
