using Etisalat.SMS.Integration.Infrastructure;
using Etisalat.SMS.Integration.Models;
using Etisalat.SMS.Integration.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Etisalat.SMS.Integration.Examples
{
    /// <summary>
    /// Examples of how to use Etisalat SMS Integration Library
    /// </summary>
    public class UsageExamples
    {
        #region Example 1: Send Single SMS Without Certificate

        public static async Task Example1_SendSingleSmsWithoutCertificate()
        {
            Console.WriteLine("=== Example 1: Send Single SMS Without Certificate ===\n");

            // 1. Setup Configuration
            var config = new EtisalatConfigurationBuilder()
                .WithUsername("YOUR_USERNAME")
                .WithPassword("YOUR_PASSWORD")
                .WithApiKey("YOUR_API_KEY_GUID")
                .UseTestEnvironment() // or UseProduction()
                .BuildApiConfig();

            // 2. Create Service
            using (var smsService = new EtisalatSmsService(config))
            {
                // 3. Send Message
                var result = await smsService.SendSmsAsync(
                    phoneNumber: "201234567890",
                    templateId: "YOUR_TEMPLATE_ID",
                    authToken: "YOUR_AUTH_TOKEN"
                );

                // 4. Process Result
                if (result.IsSuccess)
                {
                    Console.WriteLine($"✅ Sent successfully!");
                    Console.WriteLine($"   TrackingID: {result.TrackingId}");
                    Console.WriteLine($"   TransactionID: {result.TransactionId}");
                }
                else
                {
                    Console.WriteLine($"❌ Send failed: {result.ErrorMessage}");
                    Console.WriteLine($"   Response Code: {result.ResponseCode}");
                }
            }
        }

        #endregion

        #region Example 2: Send SMS With Certificate From File

        public static async Task Example2_SendSmsWithCertificateFromFile()
        {
            Console.WriteLine("=== Example 2: Send SMS With Certificate From File ===\n");

            // 1. Setup Configuration with Certificate
            var builder = new EtisalatConfigurationBuilder()
                .WithUsername("YOUR_USERNAME")
                .WithPassword("YOUR_PASSWORD")
                .WithApiKey("YOUR_API_KEY_GUID")
                .UseTestEnvironment()
                .WithCertificateFromFile(
                    certificatePath: @"C:\Certificates\MyCertificate.pfx",
                    password: "CERT_PASSWORD"
                )
                .BypassCertificateValidation(true); // For development only

            var (apiConfig, certConfig) = builder.Build();

            // 2. Create Service with Certificate
            using (var smsService = new EtisalatSmsService(apiConfig, certConfig))
            {
                // 3. Send Message
                var result = await smsService.SendSmsAsync(
                    phoneNumber: "201234567890",
                    templateId: "YOUR_TEMPLATE_ID",
                    authToken: "YOUR_AUTH_TOKEN"
                );

                Console.WriteLine(result.IsSuccess
                    ? $"✅ Success: {result.TrackingId}"
                    : $"❌ Failed: {result.ErrorMessage}");
            }
        }

        #endregion

        #region Example 3: Send SMS With Embedded Certificate

        public static async Task Example3_SendSmsWithEmbeddedCertificate()
        {
            Console.WriteLine("=== Example 3: Send SMS With Embedded Certificate ===\n");

            var builder = new EtisalatConfigurationBuilder()
                .WithUsername("YOUR_USERNAME")
                .WithPassword("YOUR_PASSWORD")
                .WithApiKey("YOUR_API_KEY_GUID")
                .UseTestEnvironment()
                .WithCertificateFromResourceByFileName(
                    fileName: "MyCertificate.pfx",
                    password: "CERT_PASSWORD"
                );

            var (apiConfig, certConfig) = builder.Build();

            using (var smsService = new EtisalatSmsService(apiConfig, certConfig))
            {
                var result = await smsService.SendCustomSmsAsync(
                    phoneNumber: "201234567890",
                    messageBody: "Welcome to Etisalat service!",
                    authToken: "YOUR_AUTH_TOKEN"
                );

                Console.WriteLine(result.IsSuccess
                    ? $"✅ Success"
                    : $"❌ Failed: {result.ErrorMessage}");
            }
        }

        #endregion

        #region Example 4: Bulk SMS Sending

        public static async Task Example4_SendBulkSms()
        {
            Console.WriteLine("=== Example 4: Bulk SMS Sending ===\n");

            var config = new EtisalatConfigurationBuilder()
                .WithUsername("YOUR_USERNAME")
                .WithPassword("YOUR_PASSWORD")
                .WithApiKey("YOUR_API_KEY_GUID")
                .UseTestEnvironment()
                .BuildApiConfig();

            using (var smsService = new EtisalatSmsService(config))
            {
                var phoneNumbers = new List<string>
                {
                    "201234567890",
                    "201111111111",
                    "201222222222"
                };

                // With Progress Reporting
                var progress = new Progress<BulkSmsProgress>(p =>
                {
                    Console.WriteLine($"Progress: {p.ProcessedCount}/{p.TotalCount} " +
                                    $"({p.ProgressPercentage:F1}%) - " +
                                    $"Success: {p.SuccessCount}, Failed: {p.FailCount}");
                });

                var bulkResult = await smsService.SendBulkSmsAsync(
                    phoneNumbers: phoneNumbers,
                    templateId: "YOUR_TEMPLATE_ID",
                    authToken: "YOUR_AUTH_TOKEN",
                    templateParams: null,
                    delayBetweenRequests: 1000, // 1 second delay between requests
                    progress: progress
                );

                Console.WriteLine($"\n📊 Final Result:");
                Console.WriteLine($"   {bulkResult}");
                Console.WriteLine($"\n📋 Details:");

                foreach (var result in bulkResult.Results)
                {
                    var status = result.IsSuccess ? "✅" : "❌";
                    Console.WriteLine($"   {status} {result.PhoneNumber}: " +
                                    $"{(result.IsSuccess ? result.TrackingId : result.ErrorMessage)}");
                }
            }
        }

        #endregion

        #region Example 5: Send SMS With Template Parameters

        public static async Task Example5_SendSmsWithTemplateParameters()
        {
            Console.WriteLine("=== Example 5: Send SMS With Template Parameters ===\n");

            var config = new EtisalatConfigurationBuilder()
                .WithUsername("YOUR_USERNAME")
                .WithPassword("YOUR_PASSWORD")
                .WithApiKey("YOUR_API_KEY_GUID")
                .UseTestEnvironment()
                .BuildApiConfig();

            using (var smsService = new EtisalatSmsService(config))
            {
                // Template parameters (example: template with {name} and {appointment_date})
                var templateParams = new Dictionary<string, string>
                {
                    { "name", "Mohamed Ahmed" },
                    { "appointment_date", "2025-12-15" },
                    { "time", "10:00 AM" }
                };

                var result = await smsService.SendSmsAsync(
                    phoneNumber: "201234567890",
                    templateId: "APPOINTMENT_TEMPLATE",
                    authToken: "YOUR_AUTH_TOKEN",
                    templateParams: templateParams
                );

                Console.WriteLine(result.IsSuccess
                    ? $"✅ Appointment sent successfully"
                    : $"❌ Failed: {result.ErrorMessage}");
            }
        }

        #endregion

        #region Example 6: Check Service Health

        public static async Task Example6_CheckServiceHealth()
        {
            Console.WriteLine("=== Example 6: Check Service Health ===\n");

            var config = new EtisalatConfigurationBuilder()
                .WithUsername("YOUR_USERNAME")
                .WithPassword("YOUR_PASSWORD")
                .WithApiKey("YOUR_API_KEY_GUID")
                .UseTestEnvironment()
                .BuildApiConfig();

            using (var smsService = new EtisalatSmsService(config))
            {
                var healthResult = await smsService.CheckServiceHealthAsync();

                Console.WriteLine($"Server: {healthResult.ServerUrl}");
                Console.WriteLine($"Status: {(healthResult.IsHealthy ? "✅ Working" : "❌ Not Working")}");
                Console.WriteLine($"Response Time: {healthResult.ResponseTime.TotalMilliseconds:F0} ms");
                Console.WriteLine($"Check Time: {healthResult.CheckTime:yyyy-MM-dd HH:mm:ss}");

                if (!healthResult.IsHealthy)
                {
                    Console.WriteLine($"Error: {healthResult.ErrorMessage}");
                }
            }
        }

        #endregion

        #region Example 7: Validate Certificate Before Use

        public static void Example7_ValidateCertificate()
        {
            Console.WriteLine("=== Example 7: Validate Certificate ===\n");

            var certConfig = new CertificateConfiguration
            {
                UseEmbeddedResource = false,
                CertificatePath = @"C:\Certificates\MyCertificate.pfx",
                CertificatePassword = "CERT_PASSWORD"
            };

            var certManager = new CertificateManager(certConfig);
            var validationResult = certManager.ValidateCertificate();

            Console.WriteLine(validationResult.ToString());

            if (validationResult.IsValid)
            {
                Console.WriteLine("\n✅ Certificate ready for use!");
            }
            else
            {
                Console.WriteLine("\n❌ Certificate issues must be resolved before proceeding");
            }
        }

        #endregion

        #region Example 8: Integration with ASP.NET Web Forms

        public class WebFormsIntegrationExample
        {
            // In .aspx.cs page
            public async Task<bool> SendSmsFromWebForm(
                string phoneNumber,
                string templateId,
                string authToken)
            {
                try
                {
                    // Read settings from Web.config
                    var username = System.Configuration.ConfigurationManager.AppSettings["EtisalatUsername"];
                    var password = System.Configuration.ConfigurationManager.AppSettings["EtisalatPassword"];
                    var apiKey = System.Configuration.ConfigurationManager.AppSettings["EtisalatApiKey"];
                    var certPath = System.Configuration.ConfigurationManager.AppSettings["CertificatePath"];
                    var certPassword = System.Configuration.ConfigurationManager.AppSettings["CertPassword"];

                    var builder = new EtisalatConfigurationBuilder()
                        .WithUsername(username)
                        .WithPassword(password)
                        .WithApiKey(apiKey)
                        .UseTestEnvironment();

                    if (!string.IsNullOrEmpty(certPath))
                    {
                        builder.WithCertificateFromFile(certPath, certPassword);
                    }

                    var (apiConfig, certConfig) = builder.Build();

                    using (var smsService = certConfig != null
                        ? new EtisalatSmsService(apiConfig, certConfig)
                        : new EtisalatSmsService(apiConfig))
                    {
                        var result = await smsService.SendSmsAsync(
                            phoneNumber,
                            templateId,
                            authToken
                        );

                        return result.IsSuccess;
                    }
                }
                catch (Exception ex)
                {
                    // Log the error
                    System.Diagnostics.Debug.WriteLine($"SMS sending error: {ex.Message}");
                    return false;
                }
            }
        }

        #endregion

        #region Example 9: Integration with Console Application

        public static async Task Example9_ConsoleApplication()
        {
            Console.WriteLine("=== Complete Console Application ===\n");

            // Read data from user
            Console.Write("Enter phone number (example: 201234567890): ");
            var phoneNumber = Console.ReadLine();

            Console.Write("Enter message text: ");
            var messageBody = Console.ReadLine();

            Console.Write("Enter Username: ");
            var username = Console.ReadLine();

            Console.Write("Enter Password: ");
            var password = ReadPassword();

            Console.Write("Enter API Key: ");
            var apiKey = Console.ReadLine();

            Console.Write("Enter Auth Token: ");
            var authToken = Console.ReadLine();

            // Create Configuration
            var config = new EtisalatConfigurationBuilder()
                .WithUsername(username)
                .WithPassword(password)
                .WithApiKey(apiKey)
                .UseTestEnvironment()
                .BuildApiConfig();

            // Send Message
            Console.WriteLine("\n🔄 Sending...\n");

            using (var smsService = new EtisalatSmsService(config))
            {
                var result = await smsService.SendCustomSmsAsync(
                    phoneNumber,
                    messageBody,
                    authToken
                );

                if (result.IsSuccess)
                {
                    Console.WriteLine($"✅ Sent successfully!");
                    Console.WriteLine($"   TrackingID: {result.TrackingId}");
                }
                else
                {
                    Console.WriteLine($"❌ Send failed!");
                    Console.WriteLine($"   Error: {result.ErrorMessage}");
                }
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }

        #endregion
    }
}