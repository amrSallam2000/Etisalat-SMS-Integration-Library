using System;
using System.Net.Http;

namespace Etisalat.SMS.Integration.Infrastructure
{
    /// <summary>
    /// Factory for creating HttpClient with digital certificate support
    /// </summary>
    public class HttpClientFactory
    {
        /// <summary>
        /// Create standard HttpClient without certificate
        /// </summary>
        public static HttpClient CreateStandardClient(int timeoutSeconds = 30)
        {
            var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
            return httpClient;
        }

        /// <summary>
        /// Create HttpClient with digital certificate
        /// </summary>
        public static HttpClient CreateClientWithCertificate(CertificateConfiguration certConfig)
        {
            if (certConfig == null)
                throw new ArgumentNullException(nameof(certConfig));

            var certManager = new CertificateManager(certConfig);
            return certManager.CreateHttpClientWithCertificate();
        }

        /// <summary>
        /// Create HttpClient based on settings
        /// If certificate is required, it will be loaded; otherwise a standard client will be created
        /// </summary>
        public static HttpClient CreateClient(
            bool requiresCertificate,
            CertificateConfiguration certConfig = null,
            int timeoutSeconds = 30)
        {
            if (requiresCertificate)
            {
                if (certConfig == null)
                {
                    throw new ArgumentNullException(nameof(certConfig),
                        "Certificate configuration is required when requiresCertificate is true");
                }
                return CreateClientWithCertificate(certConfig);
            }

            return CreateStandardClient(timeoutSeconds);
        }
    }
}