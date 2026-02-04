using Etisalat.SMS.Integration.Infrastructure;
using Etisalat.SMS.Integration.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Etisalat.SMS.Integration.Services
{
    /// <summary>
    /// Comprehensive service for sending SMS messages via Etisalat API
    /// Completely independent and integrable with any system
    /// </summary>
    public class EtisalatSmsService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly EtisalatApiConfiguration _config;
        private readonly bool _ownHttpClient;

        #region Constructors

        /// <summary>
        /// Create service with existing HttpClient
        /// </summary>
        public EtisalatSmsService(HttpClient httpClient, EtisalatApiConfiguration config)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _ownHttpClient = false;

            ValidateConfiguration();
        }

        /// <summary>
        /// Create service with simple settings (without certificate)
        /// </summary>
        public EtisalatSmsService(EtisalatApiConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _httpClient = HttpClientFactory.CreateStandardClient(30);
            _ownHttpClient = true;

            ValidateConfiguration();
        }

        /// <summary>
        /// Create service with digital certificate
        /// </summary>
        public EtisalatSmsService(
            EtisalatApiConfiguration config,
            CertificateConfiguration certConfig)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));

            if (certConfig == null)
                throw new ArgumentNullException(nameof(certConfig));

            _httpClient = HttpClientFactory.CreateClientWithCertificate(certConfig);
            _ownHttpClient = true;

            ValidateConfiguration();
        }

        #endregion

        #region Configuration Validation

        private void ValidateConfiguration()
        {
            if (string.IsNullOrWhiteSpace(_config.Username))
                throw new ArgumentException("Username is required", nameof(_config.Username));

            if (string.IsNullOrWhiteSpace(_config.Password))
                throw new ArgumentException("Password is required", nameof(_config.Password));

            if (string.IsNullOrWhiteSpace(_config.ApiKey))
                throw new ArgumentException("API Key is required", nameof(_config.ApiKey));

            if (!Guid.TryParse(_config.ApiKey, out _))
                throw new ArgumentException("API Key must be a valid GUID", nameof(_config.ApiKey));
        }

        #endregion

        #region Core SMS Sending Methods

        /// <summary>
        /// Send single SMS message using Template
        /// </summary>
        public async Task<SmsResult> SendSmsAsync(
            string phoneNumber,
            string templateId,
            string authToken,
            Dictionary<string, string> templateParams = null)
        {
            try
            {
                var characteristics = new List<Characteristic>
                {
                    new Characteristic { Name = "Authorization", Value = authToken },
                    new Characteristic { Name = "templateID", Value = templateId }
                };

                // Add template parameters
                if (templateParams != null)
                {
                    foreach (var param in templateParams)
                    {
                        characteristics.Add(new Characteristic
                        {
                            Name = param.Key,
                            Value = param.Value
                        });
                    }
                }

                var request = new SendSmsRequest
                {
                    Id = GenerateValidTransactionId(),
                    MessageType = "SMS",
                    Characteristic = characteristics,
                    Receiver = new List<Receiver>
                    {
                        new Receiver { PhoneNumber = CleanPhoneNumber(phoneNumber) }
                    }
                };

                var response = await SendSmsRequestAsync(request);
                return ConvertToSmsResult(phoneNumber, response);
            }
            catch (Exception ex)
            {
                return new SmsResult
                {
                    PhoneNumber = phoneNumber,
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    Exception = ex
                };
            }
        }

        /// <summary>
        /// Send SMS message with custom text (without Template or with empty Template)
        /// </summary>
        public async Task<SmsResult> SendCustomSmsAsync(
            string phoneNumber,
            string messageBody,
            string authToken,
            string templateId = null)
        {
            try
            {
                var characteristics = new List<Characteristic>
                {
                    new Characteristic { Name = "Authorization", Value = authToken }
                };

                if (!string.IsNullOrWhiteSpace(templateId))
                {
                    characteristics.Add(new Characteristic
                    {
                        Name = "templateID",
                        Value = templateId
                    });
                }

                if (!string.IsNullOrWhiteSpace(messageBody))
                {
                    characteristics.Add(new Characteristic
                    {
                        Name = "body",
                        Value = messageBody
                    });
                }

                var request = new SendSmsRequest
                {
                    Id = GenerateValidTransactionId(),
                    MessageType = "SMS",
                    Characteristic = characteristics,
                    Receiver = new List<Receiver>
                    {
                        new Receiver { PhoneNumber = CleanPhoneNumber(phoneNumber) }
                    }
                };

                var response = await SendSmsRequestAsync(request);
                return ConvertToSmsResult(phoneNumber, response);
            }
            catch (Exception ex)
            {
                return new SmsResult
                {
                    PhoneNumber = phoneNumber,
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    Exception = ex
                };
            }
        }

        /// <summary>
        /// Send multiple SMS messages (Bulk SMS) using Template
        /// </summary>
        public async Task<BulkSmsResult> SendBulkSmsAsync(
            List<string> phoneNumbers,
            string templateId,
            string authToken,
            Dictionary<string, string> templateParams = null,
            int delayBetweenRequests = 1000,
            IProgress<BulkSmsProgress> progress = null)
        {
            var results = new List<SmsResult>();
            int successCount = 0;
            int failCount = 0;

            for (int i = 0; i < phoneNumbers.Count; i++)
            {
                var phone = phoneNumbers[i];

                try
                {
                    var result = await SendSmsAsync(phone, templateId, authToken, templateParams);
                    results.Add(result);

                    if (result.IsSuccess)
                        successCount++;
                    else
                        failCount++;

                    // Report progress
                    progress?.Report(new BulkSmsProgress
                    {
                        TotalCount = phoneNumbers.Count,
                        ProcessedCount = i + 1,
                        SuccessCount = successCount,
                        FailCount = failCount,
                        CurrentPhone = phone,
                        LastResult = result
                    });

                    // Delay between requests
                    if (i < phoneNumbers.Count - 1 && delayBetweenRequests > 0)
                    {
                        await Task.Delay(delayBetweenRequests);
                    }
                }
                catch (Exception ex)
                {
                    var errorResult = new SmsResult
                    {
                        PhoneNumber = phone,
                        IsSuccess = false,
                        ErrorMessage = ex.Message,
                        Exception = ex
                    };
                    results.Add(errorResult);
                    failCount++;
                }
            }

            return new BulkSmsResult
            {
                Results = results,
                TotalCount = phoneNumbers.Count,
                SuccessCount = successCount,
                FailCount = failCount,
                SuccessRate = phoneNumbers.Count > 0
                    ? (successCount * 100.0 / phoneNumbers.Count)
                    : 0
            };
        }

        /// <summary>
        /// Send multiple SMS messages with custom text
        /// </summary>
        public async Task<BulkSmsResult> SendBulkCustomSmsAsync(
            List<string> phoneNumbers,
            string messageBody,
            string authToken,
            string templateId = null,
            int delayBetweenRequests = 1000,
            IProgress<BulkSmsProgress> progress = null)
        {
            var results = new List<SmsResult>();
            int successCount = 0;
            int failCount = 0;

            for (int i = 0; i < phoneNumbers.Count; i++)
            {
                var phone = phoneNumbers[i];

                try
                {
                    var result = await SendCustomSmsAsync(phone, messageBody, authToken, templateId);
                    results.Add(result);

                    if (result.IsSuccess)
                        successCount++;
                    else
                        failCount++;

                    progress?.Report(new BulkSmsProgress
                    {
                        TotalCount = phoneNumbers.Count,
                        ProcessedCount = i + 1,
                        SuccessCount = successCount,
                        FailCount = failCount,
                        CurrentPhone = phone,
                        LastResult = result
                    });

                    if (i < phoneNumbers.Count - 1 && delayBetweenRequests > 0)
                    {
                        await Task.Delay(delayBetweenRequests);
                    }
                }
                catch (Exception ex)
                {
                    var errorResult = new SmsResult
                    {
                        PhoneNumber = phone,
                        IsSuccess = false,
                        ErrorMessage = ex.Message,
                        Exception = ex
                    };
                    results.Add(errorResult);
                    failCount++;
                }
            }

            return new BulkSmsResult
            {
                Results = results,
                TotalCount = phoneNumbers.Count,
                SuccessCount = successCount,
                FailCount = failCount,
                SuccessRate = phoneNumbers.Count > 0
                    ? (successCount * 100.0 / phoneNumbers.Count)
                    : 0
            };
        }

        #endregion

        #region Internal Request Handling

        /// <summary>
        /// Send actual SMS request to API
        /// </summary>
        private async Task<SendSmsResponse> SendSmsRequestAsync(SendSmsRequest request)
        {
            // Validate data
            var validation = EtisalatValidator.ValidateSmsRequest(request);
            if (!validation.IsValid)
            {
                throw new ArgumentException(
                    $"Data validation error: {string.Join(", ", validation.Errors)}");
            }

            var url = $"{_config.BaseUrl}/communicationManagement/v1/sendSms";
            var httpRequest = CreateHttpRequest(url, request);

            try
            {
                var response = await _httpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                // Handle HTTP errors
                HandleHttpErrors(response, responseContent);

                // Parse response
                if (!string.IsNullOrEmpty(responseContent))
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<SendSmsResponse>(responseContent);
                    }
                    catch (JsonException jsonEx)
                    {
                        throw new Exception(
                            $"JSON response parsing error: {jsonEx.Message}. Content: {responseContent}");
                    }
                }
                else
                {
                    throw new Exception(
                        $"Empty response from server. HTTP Status: {response.StatusCode}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                throw new Exception($"HTTP request error: {httpEx.Message}", httpEx);
            }
            catch (TaskCanceledException timeoutEx)
            {
                throw new TimeoutException(
                    "Request timeout - possible network or firewall issue", timeoutEx);
            }
        }

        /// <summary>
        /// Create HTTP request with all required Headers
        /// </summary>
        private HttpRequestMessage CreateHttpRequest(string url, SendSmsRequest smsRequest)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);

            // JSON Content
            var json = JsonConvert.SerializeObject(smsRequest, Formatting.None);
            httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");

            // Required Headers according to documentation
            var credentials = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{_config.Username}:{_config.Password}"));

            httpRequest.Headers.Add("Authorization", $"Basic {credentials}");
            httpRequest.Headers.Add("x-Gateway-APIKey", _config.ApiKey);
            httpRequest.Headers.Add("Accept", "application/json");

            return httpRequest;
        }

        /// <summary>
        /// Handle HTTP errors
        /// </summary>
        private void HandleHttpErrors(HttpResponseMessage response, string content)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException(
                    "Authentication error - check Username/Password/ApiKey");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new UnauthorizedAccessException(
                    "Access forbidden - IP may not be allowed or insufficient permissions");
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ArgumentException("API endpoint not found - check URL");
            }
        }

        /// <summary>
        /// Convert API response to SmsResult
        /// </summary>
        private SmsResult ConvertToSmsResult(string phoneNumber, SendSmsResponse response)
        {
            var isSuccess = response?.ResponseMessage?.Header?.Code == "ESB-00000" ||
                           response?.ResponseMessage?.Header?.Status?.ToLower() == "success";

            return new SmsResult
            {
                PhoneNumber = phoneNumber,
                IsSuccess = isSuccess,
                TrackingId = response?.ResponseMessage?.ResponsePayload?.SendSMSResponse?.TrackingID,
                TransactionId = response?.ResponseMessage?.Header?.TransactionId,
                ResponseCode = response?.ResponseMessage?.Header?.Code,
                ErrorMessage = isSuccess
                    ? null
                    : GetFriendlyErrorMessage(response?.ResponseMessage?.Header?.Code),
                RawResponse = response
            };
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Generate valid Transaction ID according to documentation
        /// </summary>
        public static string GenerateValidTransactionId()
        {
            return Guid.NewGuid().ToString("D").ToLower();
        }

        /// <summary>
        /// Clean phone number and convert to required format
        /// </summary>
        public string CleanPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number is required");

            // Remove all symbols and spaces
            var cleaned = Regex.Replace(phoneNumber, @"[^\d]", "");

            // Handle Egyptian numbers
            if (cleaned.StartsWith("00"))
            {
                cleaned = cleaned.Substring(2);
            }
            else if (cleaned.StartsWith("0") && cleaned.Length == 11)
            {
                cleaned = "20" + cleaned.Substring(1);
            }
            else if (cleaned.StartsWith("1") && cleaned.Length == 10)
            {
                cleaned = "20" + cleaned;
            }
            else if (!cleaned.StartsWith("20") && cleaned.Length >= 10)
            {
                if (cleaned.StartsWith("1"))
                    cleaned = "20" + cleaned;
            }

            // Validate final number
            if (!EtisalatValidator.IsValidEgyptianPhoneNumber(cleaned))
            {
                throw new ArgumentException($"Invalid phone number: {phoneNumber}");
            }

            return cleaned;
        }

        /// <summary>
        /// Get friendly error message from error code
        /// </summary>
        private string GetFriendlyErrorMessage(string errorCode)
        {
            if (string.IsNullOrEmpty(errorCode))
                return "Unknown error";

            var errorMessages = ErrorCodeMapper.GetErrorMessages();
            return errorMessages.ContainsKey(errorCode)
                ? errorMessages[errorCode]
                : $"Unknown error: {errorCode}";
        }

        #endregion

        #region Health & Diagnostics

        /// <summary>
        /// Check service health and connection
        /// </summary>
        public async Task<ServiceHealthResult> CheckServiceHealthAsync()
        {
            var startTime = DateTime.UtcNow;
            var result = new ServiceHealthResult
            {
                ServerUrl = _config.BaseUrl,
                CheckTime = DateTime.UtcNow
            };

            try
            {
                // Test server access
                var response = await _httpClient.GetAsync(_config.BaseUrl);

                result.IsHealthy = true;
                result.ResponseTime = DateTime.UtcNow - startTime;
                result.HttpStatusCode = (int)response.StatusCode;
            }
            catch (Exception ex)
            {
                result.IsHealthy = false;
                result.ResponseTime = DateTime.UtcNow - startTime;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        #endregion

        #region IDisposable Implementation

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose HttpClient only if owned by this service
                    if (_ownHttpClient && _httpClient != null)
                    {
                        _httpClient.Dispose();
                    }
                }

                _disposed = true;
            }
        }

        #endregion
    }
}