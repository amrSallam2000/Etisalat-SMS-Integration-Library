using Etisalat.SMS.Integration.Infrastructure;
using Etisalat.SMS.Integration.Models;
using System;
using System.Configuration;
using System.IO;
using System.Web;

namespace Etisalat.SMS.Integration.Helpers
{
    /// <summary>
    /// Helper for reading settings from Web.config or App.config
    /// </summary>
    public static class ConfigurationHelper
    {
        /// <summary>
        /// Read API settings from Configuration
        /// </summary>
        public static EtisalatApiConfiguration LoadApiConfiguration()
        {
            return new EtisalatApiConfiguration
            {
                Username = GetAppSetting("EtisalatUsername", required: true),
                Password = GetAppSetting("EtisalatPassword", required: true),
                ApiKey = GetAppSetting("EtisalatApiKey", required: true),
                IsProduction = GetBoolSetting("EtisalatIsProduction", defaultValue: false)
            };
        }

        /// <summary>
        /// Read certificate settings from Configuration
        /// </summary>
        public static CertificateConfiguration LoadCertificateConfiguration()
        {
            var useEmbedded = GetBoolSetting("UseEmbeddedCertificate", defaultValue: false);
            var certPath = GetAppSetting("CertificatePath");
            var certFileName = GetAppSetting("CertificateFileName");
            var certResourceName = GetAppSetting("CertificateResourceName");
            var certPassword = GetAppSetting("CertPassword", required: true);
            var bypassValidation = GetBoolSetting("BypassCertificateValidation", defaultValue: false);

            // If no certificate settings exist, return null
            if (string.IsNullOrEmpty(certPassword))
                return null;

            var config = new CertificateConfiguration
            {
                UseEmbeddedResource = useEmbedded,
                CertificatePassword = certPassword,
                BypassServerCertificateValidation = bypassValidation,
                TimeoutSeconds = GetIntSetting("RequestTimeoutSeconds", defaultValue: 30)
            };

            if (useEmbedded)
            {
                if (!string.IsNullOrEmpty(certResourceName))
                {
                    config.EmbeddedResourceName = certResourceName;
                }
                else if (!string.IsNullOrEmpty(certFileName))
                {
                    config.CertificateFileName = certFileName;
                }
            }
            else if (!string.IsNullOrEmpty(certPath))
            {
                // Handle path (like ~/ in Web Applications)
                config.CertificatePath = ResolvePath(certPath);
            }

            return config;
        }

        /// <summary>
        /// Load full configuration using Builder
        /// </summary>
        public static (EtisalatApiConfiguration apiConfig, CertificateConfiguration certConfig)
            LoadFullConfiguration()
        {
            var apiConfig = LoadApiConfiguration();
            var certConfig = LoadCertificateConfiguration();

            return (apiConfig, certConfig);
        }

        /// <summary>
        /// Create ready-to-use Service from Configuration
        /// </summary>
        public static Services.EtisalatSmsService CreateServiceFromConfiguration()
        {
            var (apiConfig, certConfig) = LoadFullConfiguration();

            if (certConfig != null)
            {
                return new Services.EtisalatSmsService(apiConfig, certConfig);
            }
            else
            {
                return new Services.EtisalatSmsService(apiConfig);
            }
        }

        /// <summary>
        /// Get value from AppSettings
        /// </summary>
        private static string GetAppSetting(string key, bool required = false)
        {
            var value = ConfigurationManager.AppSettings[key];

            if (required && string.IsNullOrWhiteSpace(value))
            {
                throw new ConfigurationErrorsException(
                    $"Required setting is missing: {key}. Please add it to Web.config or App.config");
            }

            return value;
        }

        /// <summary>
        /// Get Boolean value
        /// </summary>
        private static bool GetBoolSetting(string key, bool defaultValue = false)
        {
            var value = GetAppSetting(key);

            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            if (bool.TryParse(value, out bool result))
                return result;

            return defaultValue;
        }

        /// <summary>
        /// Get Int value
        /// </summary>
        private static int GetIntSetting(string key, int defaultValue = 0)
        {
            var value = GetAppSetting(key);

            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            if (int.TryParse(value, out int result))
                return result;

            return defaultValue;
        }

        /// <summary>
        /// Handle paths like ~/ in Web Applications
        /// </summary>
        private static string ResolvePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            // In Web Applications
            if (HttpContext.Current != null && path.StartsWith("~/"))
            {
                return HttpContext.Current.Server.MapPath(path);
            }

            // In Console/Desktop Applications
            if (path.StartsWith("~/"))
            {
                path = path.Substring(2);
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }

            // Absolute path
            if (Path.IsPathRooted(path))
            {
                return path;
            }

            // Relative path
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        }
    }

    /// <summary>
    /// Extension Methods for easier usage
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Load Configuration from Web.config/App.config
        /// </summary>
        public static EtisalatConfigurationBuilder FromConfiguration(
            this EtisalatConfigurationBuilder builder)
        {
            var apiConfig = ConfigurationHelper.LoadApiConfiguration();

            builder.WithUsername(apiConfig.Username)
                   .WithPassword(apiConfig.Password)
                   .WithApiKey(apiConfig.ApiKey);

            if (apiConfig.IsProduction)
                builder.UseProduction();
            else
                builder.UseTestEnvironment();

            // Load certificate settings if they exist
            var certConfig = ConfigurationHelper.LoadCertificateConfiguration();
            if (certConfig != null)
            {
                if (certConfig.UseEmbeddedResource)
                {
                    if (!string.IsNullOrEmpty(certConfig.EmbeddedResourceName))
                    {
                        builder.WithCertificateFromResource(
                            certConfig.EmbeddedResourceName,
                            certConfig.CertificatePassword);
                    }
                    else if (!string.IsNullOrEmpty(certConfig.CertificateFileName))
                    {
                        builder.WithCertificateFromResourceByFileName(
                            certConfig.CertificateFileName,
                            certConfig.CertificatePassword);
                    }
                }
                else if (!string.IsNullOrEmpty(certConfig.CertificatePath))
                {
                    builder.WithCertificateFromFile(
                        certConfig.CertificatePath,
                        certConfig.CertificatePassword);
                }

                if (certConfig.BypassServerCertificateValidation)
                {
                    builder.BypassCertificateValidation();
                }
            }

            return builder;
        }
    }
}