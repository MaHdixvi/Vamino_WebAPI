using Core.Application.Contracts.Messaging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Messaging
{
    /// <summary>
    /// پیاده‌سازی واقعی ارسال ایمیل با استفاده از SMTP
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _username;
        private readonly string _password;
        private readonly bool _enableSsl;

        public EmailSender(
            string smtpServer = "smtp.gmail.com",
            int smtpPort = 587,
            string username = "your-email@gmail.com",
            string password = "your-app-password",
            bool enableSsl = true)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _username = username;
            _password = password;
            _enableSsl = enableSsl;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                using var client = new System.Net.Mail.SmtpClient(_smtpServer, _smtpPort)
                {
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(_username, _password),
                    EnableSsl = _enableSsl
                };

                var mailMessage = new System.Net.Mail.MailMessage
                {
                    From = new System.Net.Mail.MailAddress(_username, "پشتیبانی وامینو"),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);

                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // در عمل، این خطا باید در لاگ ثبت شود
                throw new Exception($"ارسال ایمیل به {email} با خطا مواجه شد: {ex.Message}", ex);
            }
        }
    }
}