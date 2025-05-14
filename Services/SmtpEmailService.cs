using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace KitabhChautari.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<SmtpEmailService> _logger;

        public SmtpEmailService(IConfiguration config, ILogger<SmtpEmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendStaffCredentialsAsync(string email, string password)
        {
            var smtpConfig = _config.GetSection("Smtp");
            var host = smtpConfig["Host"];
            var port = smtpConfig.GetValue<int>("Port");
            var username = smtpConfig["Username"];
            var smtpPassword = smtpConfig["Password"];
            var fromEmail = smtpConfig["FromEmail"];
            var fromName = smtpConfig["FromName"];

            if (string.IsNullOrEmpty(host) || port == 0 || string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(smtpPassword) || string.IsNullOrEmpty(fromEmail) || string.IsNullOrEmpty(fromName))
            {
                _logger.LogError("SMTP configuration is incomplete.");
                throw new ApplicationException("SMTP configuration is incomplete.");
            }

            try
            {
                using var message = new MimeMessage();
                message.From.Add(new MailboxAddress(fromName, fromEmail));
                message.To.Add(MailboxAddress.Parse(email));
                message.Subject = "Your Staff Credentials";
                message.Body = new TextPart("plain")
                {
                    Text = $"Email: {email}\nTemporary Password: {password}\n\nImportant: Please change your password immediately after logging in.\n\nSincerely,\nThe Kitabh Chautari Team"
                };

                using var client = new SmtpClient();
                try
                {
                    await client.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(username, smtpPassword);
                    await client.SendAsync(message);
                    _logger.LogInformation($"Credentials email sent successfully to {email}.");
                }
                catch (SmtpCommandException smtpEx)
                {
                    _logger.LogError(smtpEx, $"SMTP error sending email to {email}: {smtpEx.Message}");
                    throw new ApplicationException("Failed to send email due to an SMTP error.", smtpEx);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email to {email}: {ex.Message}");
                throw new ApplicationException("An error occurred while sending the email.", ex);
            }
        }
    }
}