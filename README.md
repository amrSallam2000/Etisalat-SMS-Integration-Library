# 📱 Etisalat SMS Integration Library

Professional and independent library for integrating with Etisalat SMS API, 100% compatible with the `APIGateway-SendSMS Interface Specification` document.

## ✨ Features

- ✅ **Fully compatible** with official Etisalat documentation
- ✅ **Digital certificate support** (SSL Client Certificates)
- ✅ **Single and bulk sending** (Single & Bulk SMS)
- ✅ **Template support** with parameters
- ✅ **Professional error handling**
- ✅ **Easy integration** with any .NET system
- ✅ **Fully documented** with ready examples
- ✅ **Secure and stable** for production

---



## 🚀 Quick Start

### Simple Example (Without Certificate)

```csharp
using Etisalat.SMS.Integration.Models;
using Etisalat.SMS.Integration.Services;

// 1. Setup Configuration
var config = new EtisalatConfigurationBuilder()
    .WithUsername("YOUR_USERNAME")
    .WithPassword("YOUR_PASSWORD")
    .WithApiKey("YOUR_API_KEY")
    .UseTestEnvironment() // or UseProduction()
    .BuildApiConfig();

// 2. Create service and send message
using (var smsService = new EtisalatSmsService(config))
{
    var result = await smsService.SendCustomSmsAsync(
        phoneNumber: "201234567890",
        messageBody: "Welcome!",
        authToken: "YOUR_AUTH_TOKEN"
    );

    if (result.IsSuccess)
        Console.WriteLine($"✅ Sent successfully! TrackingID: {result.TrackingId}");
    else
        Console.WriteLine($"❌ Failed: {result.ErrorMessage}");
}
```

---

## 🔐 Using Digital Certificate

### From .pfx file

```csharp
var builder = new EtisalatConfigurationBuilder()
    .WithUsername("YOUR_USERNAME")
    .WithPassword("YOUR_PASSWORD")
    .WithApiKey("YOUR_API_KEY")
    .UseProduction()
    .WithCertificateFromFile(
        certificatePath: @"C:\Certificates\YourCert.pfx",
        password: "CERT_PASSWORD"
    );

var (apiConfig, certConfig) = builder.Build();

using (var smsService = new EtisalatSmsService(apiConfig, certConfig))
{
    // Use service with certificate
}
```

### From Embedded Resource

```csharp
var builder = new EtisalatConfigurationBuilder()
    .WithUsername("YOUR_USERNAME")
    .WithPassword("YOUR_PASSWORD")
    .WithApiKey("YOUR_API_KEY")
    .UseProduction()
    .WithCertificateFromResourceByFileName(
        fileName: "YourCert.pfx", // File name in Resources
        password: "CERT_PASSWORD"
    );

var (apiConfig, certConfig) = builder.Build();
```

---

## 📨 Usage Examples

### 1. Send message with Template

```csharp
var result = await smsService.SendSmsAsync(
    phoneNumber: "201234567890",
    templateId: "WELCOME_TEMPLATE",
    authToken: "YOUR_AUTH_TOKEN"
);
```

### 2. Send message with Template and parameters

```csharp
var templateParams = new Dictionary<string, string>
{
    { "name", "Mohamed Ahmed" },
    { "appointment_date", "2025-12-15" }
};

var result = await smsService.SendSmsAsync(
    phoneNumber: "201234567890",
    templateId: "APPOINTMENT_TEMPLATE",
    authToken: "YOUR_AUTH_TOKEN",
    templateParams: templateParams
);
```

### 3. Send message with custom text

```csharp
var result = await smsService.SendCustomSmsAsync(
    phoneNumber: "201234567890",
    messageBody: "Hello, your appointment is Sunday at 10 AM",
    authToken: "YOUR_AUTH_TOKEN"
);
```

### 4. Bulk sending (Bulk SMS)

```csharp
var phoneNumbers = new List<string>
{
    "201234567890",
    "201111111111",
    "201222222222"
};

var progress = new Progress<BulkSmsProgress>(p =>
{
    Console.WriteLine($"Progress: {p.ProgressPercentage:F1}%");
});

var bulkResult = await smsService.SendBulkSmsAsync(
    phoneNumbers: phoneNumbers,
    templateId: "YOUR_TEMPLATE",
    authToken: "YOUR_AUTH_TOKEN",
    delayBetweenRequests: 1000,
    progress: progress
);

Console.WriteLine($"Success: {bulkResult.SuccessCount}/{bulkResult.TotalCount}");
```

---

## 🔧 Integration with ASP.NET Web Forms

### Web.config

```xml
<appSettings>
  <add key="EtisalatUsername" value="" />
  <add key="EtisalatPassword" value="" />
  <add key="EtisalatApiKey" value="" />
  <add key="CertificatePath" value="~/Certificates/YourCert.pfx" />
  <add key="CertPassword" value="" />
</appSettings>
```

### In Code-Behind

```csharp
protected async void btnSendSms_Click(object sender, EventArgs e)
{
    var username = ConfigurationManager.AppSettings["EtisalatUsername"];
    var password = ConfigurationManager.AppSettings["EtisalatPassword"];
    var apiKey = ConfigurationManager.AppSettings["EtisalatApiKey"];
    var certPath = Server.MapPath(ConfigurationManager.AppSettings["CertificatePath"]);
    var certPassword = ConfigurationManager.AppSettings["CertPassword"];

    var builder = new EtisalatConfigurationBuilder()
        .WithUsername(username)
        .WithPassword(password)
        .WithApiKey(apiKey)
        .UseTestEnvironment()
        .WithCertificateFromFile(certPath, certPassword);

    var (apiConfig, certConfig) = builder.Build();

    using (var smsService = new EtisalatSmsService(apiConfig, certConfig))
    {
        var result = await smsService.SendCustomSmsAsync(
            txtPhoneNumber.Text,
            txtMessage.Text,
            txtAuthToken.Text
        );

        lblResult.Text = result.IsSuccess 
            ? $"✅ Sent: {result.TrackingId}"
            : $"❌ Failed: {result.ErrorMessage}";
    }
}
```

---

## 📊 Processing Results

```csharp
var result = await smsService.SendSmsAsync(...);

if (result.IsSuccess)
{
    Console.WriteLine($"TrackingID: {result.TrackingId}");
    Console.WriteLine($"TransactionID: {result.TransactionId}");
}
else
{
    Console.WriteLine($"Error Code: {result.ResponseCode}");
    Console.WriteLine($"Error Message: {result.ErrorMessage}");
    
    // Access full response
    var rawResponse = result.RawResponse;
}
```

---

## 🔍 Error Codes

| Code | Description |
|------|-------------|
| `ESB-00000` | ✅ Success |
| `ESB-69011` | Agent not found |
| `ESB-69012` | Agent not active |
| `ESB-69018` | Template not found |
| `ESB-69023` | Not enough balance |
| `ESB-69031` | Invalid MSISDN |

[See full error codes documentation](./ERRORS.md)

---

## 🔐 Security

### ⚠️ Do NOT publish this data to GitHub:
- ❌ Username / Password
- ❌ API Key
- ❌ Digital certificate (.pfx)
- ❌ Auth Token

### ✅ Use instead:
- `appsettings.json` files outside Git
- Environment Variables
- Azure Key Vault / AWS Secrets Manager
- Local `.config` files

### Example .gitignore
```
*.pfx
*.p12
appsettings.*.json
Web.config
App.config.local
Certificates/
```

---


## 📄 License

This library is open source and available for personal and commercial use.

---

## 🙏 Contribution

Contributions are welcome! Please:
1. Fork the project
2. Create a new branch
3. Commit your changes
4. Open a Pull Request

---

## ⚡ Performance

- Async/Await support for high performance
- Automatic HttpClient management
- Bulk SMS support with Progress Reporting
- Efficient error handling

---

## 📚 Resources

- [Official Etisalat document](./Etisalat-APIGateway-SendSMS_Interface_Document.pdf)
- [Complete examples](./Examples/)
- [Technical documentation](./API.md)

---

**Developed by:** Amr Sallam  
**Last Updated:** February 2025  
**Version:** 1.0.0