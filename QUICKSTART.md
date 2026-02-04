# ⚡ Quick Start Guide - Etisalat SMS Integration

Quick guide to start using the library in 5 minutes.

---

## 📋 Prerequisites

- ✅ Visual Studio 2017 or later
- ✅ .NET Framework 4.7.2 or later
- ✅ NuGet Package: Newtonsoft.Json
- ✅ Etisalat account credentials (Username, Password, API Key)
- ⚠️ Digital certificate (.pfx) if required

---

## 🚀 Quick Steps

### 1️⃣ Add Library to Project

#### Method 1: Copy Files
```
1. Copy EtisalatSmsIntegration folder to your project
2. Add Reference to the files in your project
3. Install Newtonsoft.Json from NuGet:
   Install-Package Newtonsoft.Json
```

#### Method 2: NuGet (Coming Soon)
```powershell
Install-Package Etisalat.SMS.Integration
```

---

### 2️⃣ Configure Web.config or App.config

```xml
<appSettings>
  <!-- Etisalat account credentials -->
  <add key="EtisalatUsername" value="YOUR_USERNAME" />
  <add key="EtisalatPassword" value="YOUR_PASSWORD" />
  <add key="EtisalatApiKey" value="YOUR_API_KEY_GUID" />
  <add key="EtisalatIsProduction" value="false" />
  
  <!-- Certificate settings (optional) -->
  <add key="CertificatePath" value="~/Certificates/YourCert.pfx" />
  <add key="CertPassword" value="YOUR_CERT_PASSWORD" />
  <add key="UseEmbeddedCertificate" value="false" />
</appSettings>
```

⚠️ **Very Important:** Do not publish these credentials to GitHub!

---

### 3️⃣ First Message - Simple Code

```csharp
using Etisalat.SMS.Integration.Helpers;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Create service from Configuration
            using (var smsService = ConfigurationHelper.CreateServiceFromConfiguration())
            {
                // Send message
                var result = await smsService.SendCustomSmsAsync(
                    phoneNumber: "201234567890",
                    messageBody: "Hello! This is the first message from Etisalat API",
                    authToken: "YOUR_AUTH_TOKEN"
                );

                // Display result
                if (result.IsSuccess)
                {
                    Console.WriteLine($"✅ Sent successfully!");
                    Console.WriteLine($"TrackingID: {result.TrackingId}");
                }
                else
                {
                    Console.WriteLine($"❌ Failed: {result.ErrorMessage}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.ReadKey();
    }
}
```

---

### 4️⃣ Bulk Sending (Bulk SMS)

```csharp
using Etisalat.SMS.Integration.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

var phoneNumbers = new List<string>
{
    "201234567890",
    "201111111111",
    "201222222222"
};

using (var smsService = ConfigurationHelper.CreateServiceFromConfiguration())
{
    // With Progress Reporting
    var progress = new Progress<BulkSmsProgress>(p =>
    {
        Console.WriteLine($"📊 {p.ProcessedCount}/{p.TotalCount} ({p.ProgressPercentage:F0}%)");
    });

    var bulkResult = await smsService.SendBulkCustomSmsAsync(
        phoneNumbers: phoneNumbers,
        messageBody: "Bulk message",
        authToken: "YOUR_AUTH_TOKEN",
        delayBetweenRequests: 1000,
        progress: progress
    );

    Console.WriteLine($"✅ Success: {bulkResult.SuccessCount}");
    Console.WriteLine($"❌ Failed: {bulkResult.FailCount}");
}
```

---

### 5️⃣ Usage in ASP.NET Web Forms

**In Code-Behind:**
```csharp
using Etisalat.SMS.Integration.Helpers;

protected async void btnSend_Click(object sender, EventArgs e)
{
    using (var smsService = ConfigurationHelper.CreateServiceFromConfiguration())
    {
        var result = await smsService.SendCustomSmsAsync(
            txtPhone.Text,
            txtMessage.Text,
            txtAuthToken.Text
        );

        lblResult.Text = result.IsSuccess 
            ? $"✅ Sent successfully: {result.TrackingId}"
            : $"❌ Failed: {result.ErrorMessage}";
    }
}
```

---

## 🔐 Using Digital Certificate

### From external file:
```csharp
var builder = new EtisalatConfigurationBuilder()
    .WithUsername("USERNAME")
    .WithPassword("PASSWORD")
    .WithApiKey("API_KEY")
    .UseTestEnvironment()
    .WithCertificateFromFile(
        @"C:\Certificates\YourCert.pfx",
        "CERT_PASSWORD"
    );

var (apiConfig, certConfig) = builder.Build();

using (var smsService = new EtisalatSmsService(apiConfig, certConfig))
{
    // Use service
}
```

### From Embedded Resource:
```csharp
var builder = new EtisalatConfigurationBuilder()
    .WithUsername("USERNAME")
    .WithPassword("PASSWORD")
    .WithApiKey("API_KEY")
    .UseProduction()
    .WithCertificateFromResourceByFileName(
        fileName: "YourCert.pfx",
        password: "CERT_PASSWORD"
    );

var (apiConfig, certConfig) = builder.Build();
```

---

## 🔍 Connectivity Check

```csharp
using (var smsService = ConfigurationHelper.CreateServiceFromConfiguration())
{
    var health = await smsService.CheckServiceHealthAsync();
    
    Console.WriteLine($"Status: {(health.IsHealthy ? "✅" : "❌")}");
    Console.WriteLine($"Response time: {health.ResponseTime.TotalMilliseconds} ms");
}
```

---

## 📱 Common Usage Examples

### Send appointment confirmation:
```csharp
var templateParams = new Dictionary<string, string>
{
    { "name", "Ahmed Mohamed" },
    { "date", "2025-12-15" },
    { "time", "10:00 AM" }
};

var result = await smsService.SendSmsAsync(
    "201234567890",
    "APPOINTMENT_TEMPLATE",
    "AUTH_TOKEN",
    templateParams
);
```

### Send OTP:
```csharp
string otp = GenerateOTP(); // 123456

var result = await smsService.SendCustomSmsAsync(
    "201234567890",
    $"Your verification code: {otp}",
    "AUTH_TOKEN"
);
```

### Password change notification:
```csharp
var result = await smsService.SendCustomSmsAsync(
    "201234567890",
    "Your password has been changed successfully. If you did not make this change, please contact us immediately.",
    "AUTH_TOKEN"
);
```

---

## ❌ Troubleshooting Common Issues

### Issue: "Authentication Error"
```
Solution:
1. Check Username/Password in Web.config
2. Ensure API Key is correct (GUID)
3. Verify account is activated with Etisalat
```

### Issue: "Invalid MSISDN"
```
Solution:
1. Correct format: 20XXXXXXXXXX (12 digits)
2. Example: 201234567890
3. Use smsService.CleanPhoneNumber() for automatic cleaning
```

### Issue: "Template not found"
```
Solution:
1. Check Template ID
2. Verify template is linked to your account
3. Use correct template (Test vs Production)
```

### Issue: "Not enough balance"
```
Solution:
1. Recharge your Etisalat account
2. Contact Etisalat for balance inquiry
```

---

## 📚 Next Steps

Now that you've sent your first message, review:

1. [README.md](./README.md) - Complete documentation
2. [ERRORS.md](./ERRORS.md) - All error codes
3. [Examples/](./Examples/) - Advanced examples
4. [Etisalat API Document](./Etisalat-APIGateway-SendSMS_Interface_Document.pdf) - Official document

---

## 💡 Quick Tips

✅ **Do:**
- Use `ConfigurationHelper` for easy setup
- Always check `result.IsSuccess`
- Use Bulk SMS for large quantities with Progress
- Save TrackingID for message tracking

❌ **Don't:**
- Don't publish account credentials to GitHub
- Don't send more than 10 messages/second
- Don't ignore error codes
- Don't use BypassCertificateValidation in production

---

## 📞 Support

- 📧 Email: amr74513@gmail.com
- 🐛 GitHub Issues: Open an Issue
- 📚 Documentation: Review [README.md](./README.md)

---

**🎉 Congratulations! You are now ready to use Etisalat SMS API**

**Last updated:** February 2025