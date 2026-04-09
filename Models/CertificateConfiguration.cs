  /// <summary>
    /// Digital certificate settings
    /// </summary>
    public class CertificateConfiguration
    {
        /// <summary>
        /// Use Embedded Resource (inside DLL)
        /// </summary>
        public bool UseEmbeddedResource { get; set; }

        /// <summary>
        /// Full resource name (example: MyApp.Certificates.cert.pfx)
        /// </summary>
        public string EmbeddedResourceName { get; set; }

        /// <summary>
        /// Certificate file name only (for automatic search)
        /// </summary>
        public string CertificateFileName { get; set; }

        /// <summary>
        /// Full certificate file path
        /// </summary>
        public string CertificatePath { get; set; }

        /// <summary>
        /// Password to open certificate
        /// </summary>
        public string CertificatePassword { get; set; }

        /// <summary>
        /// Bypass server certificate validation (for development only)
        /// </summary>
        public bool BypassServerCertificateValidation { get; set; }

        /// <summary>
        /// Request timeout in seconds
        /// </summary>
        public int TimeoutSeconds { get; set; } = 30;
    }
