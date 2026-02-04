# 🔴 Error Codes - Etisalat SMS API

Comprehensive documentation of all error codes from Etisalat's official documentation.

---

## ✅ Success

| Code | Description | Action |
|------|-------------|--------|
| `ESB-00000` | Success | ✅ Message sent successfully |

---

## 🔐 Authentication and Account Errors

| Code | Description | Potential Cause | Solution |
|------|-------------|----------------|----------|
| `ESB-69011` | Agent not found | Invalid data sent in Authorization | Check Auth Token |
| `ESB-69012` | Agent not active | Account not activated in system | Contact Etisalat to activate account |
| `ESB-69013` | Agent is available only through SOAP call | Account created for SOAP only | Use REST API account or change account settings |
| `ESB-69016` | Account status must be ACTIVE | Account exists but not active | Contact Etisalat to activate account |

### 🔧 How to Handle Authentication Errors:
```csharp
if (ErrorCodeMapper.IsAuthenticationError(result.ResponseCode))
{
    Console.WriteLine("Account issue - contact Etisalat");
    // Send notification to administrator
    // Do not automatically retry
}
```

---

## 📋 Template Errors

| Code | Description | Potential Cause | Solution |
|------|-------------|----------------|----------|
| `ESB-69018` | Template not found | Template ID does not exist | Check Template ID |
| `ESB-69019` | Template does not belong to caller account | Template not associated with your account | Use template specific to your account |
| `ESB-69021` | Template contains parameters, expecting parameters in request | Template contains parameters not sent | Send required parameters |
| `ESB-69022` | Parameter is not provided in request | Required parameter missing | Add missing parameter |

### 🔧 How to Handle Template Errors:
```csharp
if (ErrorCodeMapper.IsTemplateError(result.ResponseCode))
{
    // Check Template ID and parameters
    if (result.ResponseCode == "ESB-69021" || result.ResponseCode == "ESB-69022")
    {
        // Add required parameters
        var templateParams = new Dictionary<string, string>
        {
            { "param1", "value1" },
            { "param2", "value2" }
        };
    }
}
```

---

## 💰 Balance Errors

| Code | Description | Potential Cause | Solution |
|------|-------------|----------------|----------|
| `ESB-69023` | Not enough balance | Insufficient account balance | Recharge account |

### 🔧 How to Handle Insufficient Balance:
```csharp
if (ErrorCodeMapper.IsBalanceError(result.ResponseCode))
{
    Console.WriteLine("⚠️ Insufficient balance!");
    // Send alert to administrator
    // Stop bulk sending
    // Display message to user
}
```

---

## ❌ Input Data Errors

| Code | Description | Potential Cause | Solution |
|------|-------------|----------------|----------|
| `ESB-69026` | Transaction id is mandatory | Transaction ID not sent | Add Transaction ID |
| `ESB-69027` | Authorization token is mandatory | Authorization token not sent in Characteristics | Add Authorization token |
| `ESB-69029` | MSISDN is mandatory | Phone number not sent | Add phone number |
| `ESB-69030` | Template id is mandatory | Template ID not sent | Add Template ID |
| `ESB-69031` | Invalid MSISDN | Invalid phone number | Check number format (20XXXXXXXXXX) |
| `ESB-69036` | Illegal input | Invalid input data | Review all fields |

### 🔧 How to Handle Data Errors:
```csharp
if (ErrorCodeMapper.IsValidationError(result.ResponseCode))
{
    // Validate data before sending
    var validation = EtisalatValidator.ValidateSmsRequest(request);
    
    if (!validation.IsValid)
    {
        foreach (var error in validation.Errors)
        {
            Console.WriteLine($"❌ {error}");
        }
    }
}
```

---

## 🔧 System Errors

| Code | Description | Error Type |
|------|-------------|-----------|
| `ESB-99999` | Unknown Error Happened | General unspecified error |
| `ESB-9999` | Unknown error | Unknown error |
| `ESB-0030` | Database Error | Database error |
| `ESB-00036` | BW Engine Error | BW engine error |
| `ESB-00031` | Error in BW Process | BW process error |
| `ESB-00037` | Checkpoint Error | Checkpoint error |
| `ESB-00038` | Error in Fault Tolerant Setup | Fault tolerant setup error |
| `ESB-00050` | Connection Time Out Error | Connection timeout |
| `ESB-00051` | Input Data Invalid | Invalid input data |
| `ESB-00052` | Output Data Invalid | Invalid output data |
| `ESB-00053` | Variable name not specified | Variable name not specified |
| `ESB-00054` | Mapping Error | Mapping error |
| `ESB-00042` | Transaction Error | Transaction error |
| `ESB-00041` | Error in Engine command | Engine command error |
| `ESB-00034` | Library Error | Library error |
| `ESB-00035` | Catch Activity Error | Catch activity error |
| `ESB-00043` | Transition condition XPath has an error | XPath error in transition condition |
| `ESB-00039` | Expression Error | Expression error |
| `ESB-00040` | Error in BW Log | BW log error |

### 🔧 How to Handle System Errors:
```csharp
if (ErrorCodeMapper.IsSystemError(result.ResponseCode))
{
    Console.WriteLine("⚠️ System error - try again");
    
    // Retry Logic
    int maxRetries = 3;
    for (int i = 0; i < maxRetries; i++)
    {
        await Task.Delay(2000); // Wait 2 seconds
        var retryResult = await smsService.SendSmsAsync(...);
        
        if (retryResult.IsSuccess)
            break;
    }
}
```

---

## 📊 Quick Reference Table

| Error Type | Codes | Recommended Action |
|------------|-------|-------------------|
| ✅ Success | `ESB-00000` | Continue operation |
| 🔐 Authentication | `69011-69016` | Contact Etisalat |
| 📋 Templates | `69018-69022` | Review template and parameters |
| 💰 Balance | `69023` | Recharge account |
| ❌ Data | `69026-69036` | Review input data |
| 🔧 System | `0030-0054, 9999, 99999` | Retry operation |

---

## 🛠️ Code Usage

### Check Error Type:
```csharp
var result = await smsService.SendSmsAsync(...);

if (ErrorCodeMapper.IsSuccessCode(result.ResponseCode))
{
    // Success
}
else if (ErrorCodeMapper.IsAuthenticationError(result.ResponseCode))
{
    // Authentication error
}
else if (ErrorCodeMapper.IsTemplateError(result.ResponseCode))
{
    // Template error
}
else if (ErrorCodeMapper.IsBalanceError(result.ResponseCode))
{
    // Insufficient balance
}
else if (ErrorCodeMapper.IsValidationError(result.ResponseCode))
{
    // Data error
}
else if (ErrorCodeMapper.IsSystemError(result.ResponseCode))
{
    // System error
}
```

### Get Friendly Message:
```csharp
var friendlyMessage = ErrorCodeMapper.GetErrorMessage(result.ResponseCode);
Console.WriteLine(friendlyMessage);
```

---

## 📞 Support

If problems persist:
1. Check all input data
2. Review Etisalat official documentation
3. Contact Etisalat technical support
4. Open GitHub issue with error details (without sensitive data)

---

**Last Updated:** February 2025  
**Source:** Etisalat APIGateway-SendSMS Interface Specification v0.1