using ElevaniPaymentGateway.Core.Configs;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Infrastructure.Implementations.Services.Helpers;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using Newtonsoft.Json;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services.Utilities
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfig _emailConfig;
        private readonly ILogger<EmailService> _logger;
        public EmailService(IOptions<EmailConfig> emailConfig, ILogger<EmailService> logger)
        {
            _emailConfig = emailConfig.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(EmailRequest req)
        {
            try
            {
                _logger.LogInformation($"Attempting to send email");
                _logger.LogInformation($"From >>> {_emailConfig.From}");
                _logger.LogInformation($"To >>> {JsonConvert.SerializeObject(req.RecipientTo)} Cc >>> {JsonConvert.SerializeObject(req.RecipientCC)}");
                _logger.LogInformation($"SMTP >>> {_emailConfig.SMTPAddress} | port >>> {_emailConfig.SMTPPort} | username >>> {_emailConfig.Username}");

                var builder = EmailResourceHelpers.mimeKitCommonLinkedResource();
                builder.HtmlBody = req.Body;

                //Add attachment here
                if (req.Attachments != null)
                    if (req.Attachments.Any())
                        foreach (var attachment in req.Attachments)
                            builder.Attachments.Add(attachment.FileName, attachment.Data, attachment.ContentType);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Eletranz", _emailConfig.From));
                message.Subject = req.Subject;
                message.Body = builder.ToMessageBody();

                if (req.RecipientTo.Count == 0)
                    throw new GenericException("No email recipients");

                foreach (string toAddress in req.RecipientTo)
                    message.To.Add(new MailboxAddress(toAddress, toAddress));

                if (req.RecipientCC != null)
                    if (req.RecipientCC.Count > 0)
                        foreach (string copyAddress in req.RecipientCC)
                            message.Cc.Add(new MailboxAddress(copyAddress, copyAddress));

                using (var client = new SmtpClient())
                {
                    client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                    await client.ConnectAsync(_emailConfig.SMTPAddress, _emailConfig.SMTPPort, _emailConfig.EnableSSL);
                    await client.AuthenticateAsync(_emailConfig.Username, _emailConfig.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                //throw;
            }
        }
    }
}
