/// <summary>
    /// Certificate validation result
    /// </summary>
    public class CertificateValidationResult
    {
        public bool IsValid { get; set; }
        public string Subject { get; set; }
        public string Issuer { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool HasPrivateKey { get; set; }
        public string Thumbprint { get; set; }
        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            if (!IsValid)
                return $"❌ Certificate invalid: {ErrorMessage}";

            return $@"✅ Certificate valid:
   Subject: {Subject}
   Issuer: {Issuer}
   Valid from: {ValidFrom:yyyy-MM-dd}
   Valid until: {ValidTo:yyyy-MM-dd}
   Private key: {(HasPrivateKey ? "✅ Present" : "❌ Missing")}
   Thumbprint: {Thumbprint}";
        }
    }
