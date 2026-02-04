using Etisalat.SMS.Integration.Infrastructure;
using System;

namespace Etisalat.SMS.Integration.Models
{
    /// <summary>
    /// Etisalat API connection settings
    /// </summary>
    public class EtisalatApiConfiguration
    {
        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// API Key (should be a GUID)
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Use production environment (true) or test environment (false)
        /// </summary>
        public bool IsProduction { get; set; }

        /// <summary>
        /// Base URL based on environment
        /// </summary>
        public string BaseUrl => IsProduction
            ? "https://api.eand.com.eg"
            : "https://api-test.eand.com.eg";

        /// <summary>
        /// Full URL for SendSMS endpoint
        /// </summary>
        public string SendSmsEndpoint => $"{BaseUrl}/communicationManagement/v1/sendSms";
    }

    /// <summary>
    /// Advanced builder for Etisalat API settings
    /// </summary>
    public class EtisalatConfigurationBuilder
    {
        private EtisalatApiConfiguration _config = new EtisalatApiConfiguration();
        private CertificateConfiguration _certConfig;

        public EtisalatConfigurationBuilder WithUsername(string username)
        {
            _config.Username = username;
            return this;
        }

        public EtisalatConfigurationBuilder WithPassword(string password)
        {
            _config.Password = password;
            return this;
        }

        public EtisalatConfigurationBuilder WithApiKey(string apiKey)
        {
            _config.ApiKey = apiKey;
            return this;
        }

        public EtisalatConfigurationBuilder UseProduction(bool isProduction = true)
        {
            _config.IsProduction = isProduction;
            return this;
        }

        public EtisalatConfigurationBuilder UseTestEnvironment()
        {
            _config.IsProduction = false;
            return this;
        }

        public EtisalatConfigurationBuilder WithCertificateFromFile(
            string certificatePath,
            string password)
        {
            _certConfig = new CertificateConfiguration
            {
                UseEmbeddedResource = false,
                CertificatePath = certificatePath,
                CertificatePassword = password
            };
            return this;
        }

        public EtisalatConfigurationBuilder WithCertificateFromResource(
            string resourceName,
            string password)
        {
            _certConfig = new CertificateConfiguration
            {
                UseEmbeddedResource = true,
                EmbeddedResourceName = resourceName,
                CertificatePassword = password
            };
            return this;
        }

        public EtisalatConfigurationBuilder WithCertificateFromResourceByFileName(
            string fileName,
            string password)
        {
            _certConfig = new CertificateConfiguration
            {
                UseEmbeddedResource = true,
                CertificateFileName = fileName,
                CertificatePassword = password
            };
            return this;
        }

        public EtisalatConfigurationBuilder BypassCertificateValidation(bool bypass = true)
        {
            if (_certConfig == null)
                _certConfig = new CertificateConfiguration();

            _certConfig.BypassServerCertificateValidation = bypass;
            return this;
        }

        public (EtisalatApiConfiguration apiConfig, CertificateConfiguration certConfig) Build()
        {
            // Validate
            if (string.IsNullOrWhiteSpace(_config.Username))
                throw new InvalidOperationException("Username is required");

            if (string.IsNullOrWhiteSpace(_config.Password))
                throw new InvalidOperationException("Password is required");

            if (string.IsNullOrWhiteSpace(_config.ApiKey))
                throw new InvalidOperationException("API Key is required");

            if (!Guid.TryParse(_config.ApiKey, out _))
                throw new InvalidOperationException("API Key must be a valid GUID");

            return (_config, _certConfig);
        }

        public EtisalatApiConfiguration BuildApiConfig()
        {
            return Build().apiConfig;
        }
    }
}