# 📂 Project Structure - Etisalat SMS Integration

```
EtisalatSmsIntegration/
│
├── 📁 Infrastructure/           # Infrastructure
│   ├── CertificateManager.cs   # Digital certificate management
│   └── HttpClientFactory.cs    # HttpClient factory
│
├── 📁 Models/                   # Data models
│   ├── EtisalatApiConfiguration.cs  # API configurations
│   ├── EtisalatModels.cs        # Request/Response Models
│   └── Results.cs               # Result models
│
├── 📁 Services/                 # Core services
│   ├── EtisalatSmsService.cs   # Main service ⭐
│   ├── EtisalatValidator.cs    # Data validation
│   └── ErrorCodeMapper.cs      # Error codes
│
├── 📁 Helpers/                  # Helpers
│   └── ConfigurationHelper.cs  # Configuration reader
│
├── 📁 Examples/                 # Examples
│   ├── UsageExamples.cs        # Comprehensive examples
│   └── WebForms/
│       └── SendSmsPage.aspx.cs # Web Forms example
│
├── 📁 Config/                   # Configurations
│   └── Web.config.example      # Example configuration file
│
├── 📁 Properties/               # Project properties
│   └── AssemblyInfo.cs
│
├── 📄 README.md                # Main documentation
├── 📄 QUICKSTART.md           # Quick start guide
├── 📄 ERRORS.md               # Error documentation
├── 📄 .gitignore              # Git excluded files
├── 📄 packages.config         # NuGet packages
└── 📄 Etisalat.SMS.Integration.csproj
```

---

## 🎯 Core Files

### ⭐ EtisalatSmsService.cs
**Main service** - Entry point for all SMS operations

```csharp
// Create service
var service = new EtisalatSmsService(config);

// Send message
var result = await service.SendSmsAsync(...);

// Bulk send
var bulkResult = await service.SendBulkSmsAsync(...);

// Health check
var health = await service.CheckServiceHealthAsync();
```

---

### 🔐 CertificateManager.cs
**Digital certificate management** - Load and use SSL certificates

```csharp
var certManager = new CertificateManager(certConfig);
var httpClient = certManager.CreateHttpClientWithCertificate();
var validation = certManager.ValidateCertificate();
```

---

### 🎛️ ConfigurationHelper.cs
**Configuration helper** - Automatic reading from Web.config/App.config

```csharp
// Automatic reading from Configuration
using (var service = ConfigurationHelper.CreateServiceFromConfiguration())
{
    // Direct use
}
```

---

### ✅ EtisalatValidator.cs
**Data validation** - Validate data before sending

```csharp
// Check Transaction ID
bool isValid = EtisalatValidator.IsValidTransactionId(id);

// Check phone number
bool isValid = EtisalatValidator.IsValidEgyptianPhoneNumber(phone);

// Full request validation
var validation = EtisalatValidator.ValidateSmsRequest(request);
```

---

### 🔍 ErrorCodeMapper.cs
**Error codes** - Translate error codes to clear messages

```csharp
// Get error message
string message = ErrorCodeMapper.GetErrorMessage(errorCode);

// Check error type
bool isAuth = ErrorCodeMapper.IsAuthenticationError(code);
bool isTemplate = ErrorCodeMapper.IsTemplateError(code);
bool isBalance = ErrorCodeMapper.IsBalanceError(code);
```

---

## 🔄 Workflow

```
1. User
   ↓
2. ConfigurationHelper or EtisalatConfigurationBuilder
   ↓ (Read configurations)
3. CertificateManager (if needed)
   ↓ (Load certificate)
4. HttpClientFactory
   ↓ (Create HttpClient)
5. EtisalatSmsService
   ↓ (Validate data)
6. EtisalatValidator
   ↓ (Send request)
7. Etisalat API
   ↓ (Receive response)
8. ErrorCodeMapper
   ↓ (Process result)
9. SmsResult / BulkSmsResult
   ↓
10. User
```

---

## 📦 Required Packages (NuGet)

```xml
<package id="Newtonsoft.Json" version="13.0.3" />
```

---

## 🎨 Design Patterns Used

### Factory Pattern
- `HttpClientFactory` - Create HttpClient

### Builder Pattern
- `EtisalatConfigurationBuilder` - Build Configuration

### Singleton Pattern
- `ErrorCodeMapper` - static class for codes

### Dispose Pattern
- `EtisalatSmsService` - implements IDisposable

### Strategy Pattern
- `CertificateManager` - Different loading strategies

---

## 🔧 Customization and Extension

### Add custom Validation:
```csharp
public static class CustomValidator
{
    public static bool IsValidCustomFormat(string input)
    {
        // Your validation logic
    }
}
```

### Add new Error Code:
```csharp
// In ErrorCodeMapper.cs
{ "CUSTOM-001", "Custom error message" }
```

### Add new Certificate Source:
```csharp
// In CertificateManager.cs
private X509Certificate2 LoadFromCustomSource()
{
    // Your loading logic
}
```

---

## 📊 Dependencies

```
EtisalatSmsService
    ├── HttpClient (from HttpClientFactory)
    ├── EtisalatApiConfiguration
    └── CertificateConfiguration (optional)

HttpClientFactory
    └── CertificateManager (optional)

CertificateManager
    └── CertificateConfiguration

ConfigurationHelper
    ├── System.Configuration
    └── System.Web (optional)
```

---

## 🚀 Main Entry Points

### For new developers:
```csharp
// Start here
var service = ConfigurationHelper.CreateServiceFromConfiguration();
```

### For full control:
```csharp
// Use Builder
var builder = new EtisalatConfigurationBuilder()...
```

### For quick integration:
```csharp
// Use Service directly
var service = new EtisalatSmsService(httpClient, config);
```

---

## 📝 Important Files to Read

| File | Purpose | Priority |
|------|---------|----------|
| README.md | Comprehensive documentation | ⭐⭐⭐⭐⭐ |
| QUICKSTART.md | Quick start guide | ⭐⭐⭐⭐⭐ |
| ERRORS.md | Error codes | ⭐⭐⭐⭐ |
| UsageExamples.cs | Practical examples | ⭐⭐⭐⭐ |
| Web.config.example | Configuration template | ⭐⭐⭐ |

---

## 🔒 Security Files (Do not publish)

```
❌ Web.config (actual)
❌ *.pfx (certificates)
❌ *.p12 (certificates)
❌ appsettings.json (actual)
```

✅ **Use instead:**
```
✅ Web.config.example
✅ README explanation
✅ Environment variables
```

---

## 📞 For Contribution

1. Fork the project
2. Create new branch
3. Follow Coding Standards
4. Write Unit Tests
5. Open Pull Request

---

**Last updated:** February 2025  
**Version:** 1.0.0