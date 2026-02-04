using Etisalat.SMS.Integration.Helpers;
using Etisalat.SMS.Integration.Models;
using System;
using System.Web.UI;

namespace Etisalat.SMS.Integration
{
    /// <summary>
    /// Simple example of using Etisalat SMS in Web Form
    /// </summary>
    public partial class SendSmsPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // You can load default data here
            }
        }

        /// <summary>
        /// Send single message
        /// </summary>
        protected async void btnSendSingle_Click(object sender, EventArgs e)
        {
            try
            {
                // Clear previous messages
                lblResult.Text = "";
                lblResult.CssClass = "";

                // Validate input data
                if (string.IsNullOrWhiteSpace(txtPhoneNumber.Text))
                {
                    ShowError("Please enter phone number");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtMessage.Text))
                {
                    ShowError("Please enter message text");
                    return;
                }

                // Create service from Configuration
                using (var smsService = ConfigurationHelper.CreateServiceFromConfiguration())
                {
                    // Send message
                    var result = await smsService.SendCustomSmsAsync(
                        phoneNumber: txtPhoneNumber.Text.Trim(),
                        messageBody: txtMessage.Text.Trim(),
                        authToken: GetAuthToken()
                    );

                    // Display result
                    if (result.IsSuccess)
                    {
                        ShowSuccess($"Sent successfully! TrackingID: {result.TrackingId}");

                        // Clear fields after success
                        txtPhoneNumber.Text = "";
                        txtMessage.Text = "";
                    }
                    else
                    {
                        ShowError($"Send failed: {result.ErrorMessage} (Code: {result.ResponseCode})");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"Unexpected error: {ex.Message}");

                // Log error
                System.Diagnostics.Debug.WriteLine($"Error in SendSms: {ex}");
            }
        }

        /// <summary>
        /// Send message using Template
        /// </summary>
        protected async void btnSendWithTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                lblResult.Text = "";
                lblResult.CssClass = "";

                if (string.IsNullOrWhiteSpace(txtPhoneNumber.Text))
                {
                    ShowError("Please enter phone number");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtTemplateId.Text))
                {
                    ShowError("Please enter Template ID");
                    return;
                }

                using (var smsService = ConfigurationHelper.CreateServiceFromConfiguration())
                {
                    var result = await smsService.SendSmsAsync(
                        phoneNumber: txtPhoneNumber.Text.Trim(),
                        templateId: txtTemplateId.Text.Trim(),
                        authToken: GetAuthToken()
                    );

                    if (result.IsSuccess)
                    {
                        ShowSuccess($"Sent successfully! TrackingID: {result.TrackingId}");
                    }
                    else
                    {
                        ShowError($"Send failed: {result.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Test service connection
        /// </summary>
        protected async void btnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                lblResult.Text = "Testing connection...";
                lblResult.CssClass = "alert alert-info";

                using (var smsService = ConfigurationHelper.CreateServiceFromConfiguration())
                {
                    var healthResult = await smsService.CheckServiceHealthAsync();

                    if (healthResult.IsHealthy)
                    {
                        ShowSuccess($"✅ Service is working! Response time: {healthResult.ResponseTime.TotalMilliseconds:F0} ms");
                    }
                    else
                    {
                        ShowError($"❌ Service not working: {healthResult.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"Connection error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get Auth Token
        /// Can be read from Session or Database based on system
        /// </summary>
        private string GetAuthToken()
        {
            // Example: Read from TextBox
            if (!string.IsNullOrWhiteSpace(txtAuthToken.Text))
            {
                return txtAuthToken.Text.Trim();
            }

            // Or read from Session
            if (Session["AuthToken"] != null)
            {
                return Session["AuthToken"].ToString();
            }

            // Or read from Config as Default
            return System.Configuration.ConfigurationManager.AppSettings["DefaultAuthToken"];
        }

        #region Helper Methods

        private void ShowSuccess(string message)
        {
            lblResult.Text = message;
            lblResult.CssClass = "alert alert-success";
        }

        private void ShowError(string message)
        {
            lblResult.Text = message;
            lblResult.CssClass = "alert alert-danger";
        }

        private void ShowInfo(string message)
        {
            lblResult.Text = message;
            lblResult.CssClass = "alert alert-info";
        }

        #endregion
    }
}