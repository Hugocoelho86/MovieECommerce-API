using ECommerceMovies.API.Configurations;
using ECommerceMovies.API.Dto;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace ECommerceMovies.API.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailService(IOptions<EmailConfiguration> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public async Task SendEmailAsync(MessageDto message)
        {
            MimeMessage emailMessage = CreateEmailMessage(message);
            await SendEmail(emailMessage);
        }

        private MimeMessage CreateEmailMessage(MessageDto message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From, _emailConfig.Username));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(TextFormat.Html) { Text = message.Body };

            return emailMessage;
        }

        private async Task SendEmail(MimeMessage emailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_emailConfig.Username, _emailConfig.Password);
                    await client.SendAsync(emailMessage);

                }
                catch (Exception)
                {
                    //log an error message or throw and exception or both
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }

            }


        }
    }
}
