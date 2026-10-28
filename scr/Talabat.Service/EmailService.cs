using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using Talabat.Core.Service.Contract;

namespace Talabat.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpHost = _configuration["EmailSettings:Host"];
                int.TryParse(_configuration["EmailSettings:Port"], out var smtpPort);
                if (smtpPort == 0) smtpPort = 587;
                var smtpUser = _configuration["EmailSettings:Username"];
                var smtpPass = _configuration["EmailSettings:Password"];
                var fromEmail = _configuration["EmailSettings:FromEmail"] ?? smtpUser ?? "noreply@talabat.com";

                if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpUser))
                {
                    _logger.LogInformation("SMTP not configured. Simulated email to {ToEmail}: Subject: {Subject}", toEmail, subject);
                    return true;
                }

                using var client = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = true
                };

                using var message = new MailMessage
                {
                    From = new MailAddress(fromEmail, "Talabat Store"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                message.To.Add(toEmail);

                await client.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {ToEmail}", toEmail);
                return false;
            }
        }
    }
}
