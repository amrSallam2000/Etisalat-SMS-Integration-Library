using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Etisalat.SMS.Integration.Infrastructure
{
    /// <summary>
    /// Digital certificate manager for Etisalat API
    /// Supports loading certificates from Embedded Resources or files
    /// </summary>
    public class CertificateManager
    {
        private readonly CertificateConfiguration _config;

        public CertificateManager(CertificateConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// Create HttpClient with required digital certificate
        /// </summary>
        public HttpClient CreateHttpClientWithCertificate()
        {
            try
            {
                X509Certificate2 certificate = null;

                // Try to load from Embedded Resource first
                if (_config.UseEmbeddedResource)
                {
                    certificate = LoadCertificateFromEmbeddedResource();
                }

                // If loading from Embedded Resource failed, try from file
                if (certificate == null && !string.IsNullOrEmpty(_config.CertificatePath))
                {
                    certificate = LoadCertificateFromFile();
                }

                if (certificate == null)
                {
                    throw new InvalidOperationException(
                        "Failed to load digital certificate. Check settings and paths.");
                }

                // Create HttpClientHandler with certificate
                var handler = new HttpClientHandler();
                handler.ClientCertificates.Add(certificate);

                // Bypass server certificate validation (for development only)
                if (_config.BypassServerCertificateValidation)
                {
                    handler.ServerCertificateCustomValidationCallback =
                        (sender, cert, chain, sslPolicyErrors) => true;
                }

                var httpClient = new HttpClient(handler);
                httpClient.Timeout = TimeSpan.FromSeconds(_config.TimeoutSeconds);

                return httpClient;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Error creating HttpClient with certificate: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Load certificate from Embedded Resource
        /// </summary>
        private X509Certificate2 LoadCertificateFromEmbeddedResource()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();

                // If full resource name is specified
                if (!string.IsNullOrEmpty(_config.EmbeddedResourceName))
                {
                    return LoadFromResourceName(assembly, _config.EmbeddedResourceName);
                }

                // Try to find certificate in all resources
                if (!string.IsNullOrEmpty(_config.CertificateFileName))
                {
                    var resourceNames = assembly.GetManifestResourceNames();
                    foreach (var resourceName in resourceNames)
                    {
                        if (resourceName.EndsWith(_config.CertificateFileName,
                            StringComparison.OrdinalIgnoreCase))
                        {
                            return LoadFromResourceName(assembly, resourceName);
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Failed to load certificate from Embedded Resource: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Load certificate from specified resource name
        /// </summary>
        private X509Certificate2 LoadFromResourceName(Assembly assembly, string resourceName)
        {
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    return null;

                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    var certBytes = memoryStream.ToArray();

                    return new X509Certificate2(
                        certBytes,
                        _config.CertificatePassword,
                        X509KeyStorageFlags.MachineKeySet |
                        X509KeyStorageFlags.PersistKeySet |
                        X509KeyStorageFlags.Exportable);
                }
            }
        }

        /// <summary>
        /// Load certificate from file
        /// </summary>
        private X509Certificate2 LoadCertificateFromFile()
        {
            try
            {
                if (string.IsNullOrEmpty(_config.CertificatePath))
                    return null;

                if (!File.Exists(_config.CertificatePath))
                {
                    throw new FileNotFoundException(
                        $"Certificate file not found: {_config.CertificatePath}");
                }

                return new X509Certificate2(
                    _config.CertificatePath,
                    _config.CertificatePassword,
                    X509KeyStorageFlags.MachineKeySet |
                    X509KeyStorageFlags.PersistKeySet |
                    X509KeyStorageFlags.Exportable);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Failed to load certificate from file: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Validate certificate
        /// </summary>
        public CertificateValidationResult ValidateCertificate()
        {
            var result = new CertificateValidationResult();

            try
            {
                X509Certificate2 cert = null;

                // Try to load
                if (_config.UseEmbeddedResource)
                {
                    cert = LoadCertificateFromEmbeddedResource();
                }

                if (cert == null && !string.IsNullOrEmpty(_config.CertificatePath))
                {
                    cert = LoadCertificateFromFile();
                }

                if (cert == null)
                {
                    result.IsValid = false;
                    result.ErrorMessage = "Certificate not found";
                    return result;
                }

                result.IsValid = true;
                result.Subject = cert.Subject;
                result.Issuer = cert.Issuer;
                result.ValidFrom = cert.NotBefore;
                result.ValidTo = cert.NotAfter;
                result.HasPrivateKey = cert.HasPrivateKey;
                result.Thumbprint = cert.Thumbprint;

                // Check validity period
                var now = DateTime.Now;
                if (now < cert.NotBefore)
                {
                    result.IsValid = false;
                    result.ErrorMessage = "Certificate not yet valid";
                }
                else if (now > cert.NotAfter)
                {
                    result.IsValid = false;
                    result.ErrorMessage = "Certificate has expired";
                }

                // Check for private key
                if (!cert.HasPrivateKey)
                {
                    result.IsValid = false;
                    result.ErrorMessage = "Certificate does not contain private key";
                }

                cert.Dispose();
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.ErrorMessage = $"Error validating certificate: {ex.Message}";
            }

            return result;
        }
    }

}
