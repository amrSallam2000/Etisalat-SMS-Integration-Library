# 📦 Installation and Usage Instructions

## ✅ What You Received

A compressed file containing the complete **Etisalat SMS Integration Library**, ready to integrate with any .NET system.

---

## 📂 Package Contents

```
EtisalatSmsIntegration/
├── Infrastructure/      # Certificate and HttpClient management
├── Models/             # Data models
├── Services/           # Core services
├── Helpers/            # Configuration helpers
├── Examples/           # Ready-to-use examples
├── Config/             # Configuration example files
├── README.md           # Complete documentation
├── QUICKSTART.md       # Quick start guide (Start here!)
├── ERRORS.md           # All error codes
└── STRUCTURE.md        # Project structure

Total: 20 C# files + 5 documentation files
```

---

## 🚀 Installation Steps

### 1️⃣ Extract Files
```
Extract EtisalatSmsIntegration.zip to a folder in your project
```

### 2️⃣ Add to Project

**In Visual Studio:**
```
1. Right-click on Solution → Add → Existing Project
2. Choose: EtisalatSmsIntegration.csproj
3. Or: Copy files directly to your project
```

### 3️⃣ Install Newtonsoft.Json
```powershell
Install-Package Newtonsoft.Json
```

### 4️⃣ Setup Web.config
```xml
<!-- Copy from Config/Web.config.example -->
<appSettings>
  <add key="EtisalatUsername" value="YOUR_USERNAME" />
  <add key="EtisalatPassword" value="YOUR_PASSWORD" />
  <add key="EtisalatApiKey" value="YOUR_API_KEY" />
  <add key="EtisalatIsProduction" value="false" />
  
  <!-- For certificate (optional) -->
  <add key="CertificatePath" value="~/Certificates/YourCert.pfx" />
  <add key="CertPassword" value="CERT_PASSWORD" />
</appSettings>
```

### 5️⃣ First Use
```csharp
using Etisalat.SMS.Integration.Helpers;

// Create service from Configuration
using (var smsService = ConfigurationHelper.CreateServiceFromConfiguration())
{
    var result = await smsService.SendCustomSmsAsync(
        "201234567890",
        "Welcome!",
        "YOUR_AUTH_TOKEN"
    );

    Console.WriteLine(result.IsSuccess ? "✅ Success" : "❌ Failed");
}
```

---

## 📚 Documentation

| File | Usage |
|------|-------|
| **QUICKSTART.md** | ⭐ **Start here!** - 5-minute guide |
| **README.md** | Complete documentation and examples |
| **ERRORS.md** | All error codes and solutions |
| **STRUCTURE.md** | Project structure and files |
| **Examples/** | Ready-to-copy examples |

---

## ✨ Key Features

✅ **100% compatible** with official Etisalat documentation  
✅ **Digital certificate support** (SSL Client Certificates)  
✅ **Single and bulk sending** (Single & Bulk SMS)  
✅ **Professional error handling**  
✅ **Easy integration** with any .NET system  
✅ **Fully documented** with practical examples  

---

## 🔐 Digital Certificate

### If required:

**Method 1: From external file**
```csharp
var builder = new EtisalatConfigurationBuilder()
    .WithUsername("USERNAME")
    .WithPassword("PASSWORD")
    .WithApiKey("API_KEY")
    .UseProduction()
    .WithCertificateFromFile(
        @"C:\Certificates\YourCert.pfx",
        "CERT_PASSWORD"
    );
```

**Method 2: From Embedded Resource**
```csharp
// 1. Add certificate as Embedded Resource in Visual Studio
// 2. Use:
builder.WithCertificateFromResourceByFileName(
    "YourCert.pfx",
    "CERT_PASSWORD"
);
```

---

## 🎯 Quick Examples

### Send single message:
```csharp
var result = await smsService.SendCustomSmsAsync(
    "201234567890",
    "Message text",
    "AUTH_TOKEN"
);
```

### Bulk sending:
```csharp
var phones = new List<string> { "201111111111", "201222222222" };

var bulkResult = await smsService.SendBulkCustomSmsAsync(
    phones,
    "Bulk message",
    "AUTH_TOKEN"
);

Console.WriteLine($"Success: {bulkResult.SuccessCount}/{bulkResult.TotalCount}");
```

### Using Template:
```csharp
var params = new Dictionary<string, string>
{
    { "name", "Ahmed" },
    { "date", "2025-12-15" }
};

var result = await smsService.SendSmsAsync(
    "201234567890",
    "TEMPLATE_ID",
    "AUTH_TOKEN",
    params
);
```

---

## ❌ Troubleshooting

### "Authentication error"
- Check Username/Password/ApiKey in Web.config
- Verify account activation with Etisalat

### "Invalid MSISDN"
- Correct format: `20XXXXXXXXXX` (12 digits)
- Example: `201234567890`

### "Template not found"
- Check Template ID
- Verify template is associated with your account

### "Not enough balance"
- Recharge your Etisalat account

---

## 🔒 Security

### ⚠️ Do NOT publish on GitHub:
- ❌ Username/Password
- ❌ API Key
- ❌ Digital certificate (.pfx)
- ❌ Actual Web.config

### ✅ Use instead:
- Environment Variables
- Azure Key Vault
- Local .config files (outside Git)

---

## 📞 Support

- 📖 **Documentation:** Refer to QUICKSTART.md and README.md
- 🐛 **Problems:** Refer to ERRORS.md
- 💡 **Examples:** Check Examples/ folder
- 📧 **Inquiries:** Open an Issue on GitHub

---

## 🎉 Ready to Use!

**Next Steps:**
1. ✅ Read QUICKSTART.md (5 minutes)
2. ✅ Setup Web.config with your settings
3. ✅ Send first test message
4. ✅ Check Examples/ for advanced examples

---

**Library Version:** 1.0.0  
**Last Update:** February 2025  
**Compatibility:** .NET Framework 4.7.2+

**Congratulations! 🎊 You are now ready to use Etisalat SMS API professionally**