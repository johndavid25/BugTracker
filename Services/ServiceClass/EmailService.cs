using Microsoft.AspNetCore.Identity.UI.Services;
using MailKit.Security;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using BugTracker.Models;
using Microsoft.Extensions.Options;

namespace BugTracker.Services.ServiceClass
{
    public class EmailService : IEmailSender
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(string emailTo, string subject, string htmlMessage)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(emailTo));
            email.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = htmlMessage;

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);

            await smtp.SendAsync(email);

            smtp.Disconnect(true);
        }
    }
}
