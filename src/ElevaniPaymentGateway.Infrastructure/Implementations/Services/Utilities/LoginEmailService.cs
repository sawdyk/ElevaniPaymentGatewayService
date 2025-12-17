using ElevaniPaymentGateway.Core.Configs;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Helpers;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services.Utilities
{
    public class LoginEmailService : ILoginEmailService
    {
        private readonly ILogger<LoginEmailService> _logger;
        private readonly IEmailService _emailService;
        private string _emailSubject = "Login Notification";
        private readonly AppSettingsConfig _appSettingsConfig;
        public LoginEmailService(ILogger<LoginEmailService> logger, IEmailService emailService,
            IOptions<AppSettingsConfig> appSettingsConfig)
        {
            _logger = logger;
            _emailService = emailService;
            _appSettingsConfig = appSettingsConfig.Value;
        }

        public async Task SendLoginMailNotificationAsync(User user)
        {
            try
            {
                string emailTemplate = File.ReadAllText("wwwroot/MailTemplates/Account/LoginTemp.html");
                emailTemplate = emailTemplate.Replace("{name}", $"{user.FirstName.FirstCharToUpper()}");
                emailTemplate = emailTemplate.Replace("{date}", DateTime.Now.ToString("dddd, MMMM yyyy, hh:mm:ss tt"));

                EmailRequest msg = new()
                {
                    RecipientTo = new List<string> { user.Email },
                    Subject = _emailSubject,
                    Body = emailTemplate,
                };

                await _emailService.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to send login notification email >> {ex.Message}" +
                    $" | stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
            }
        }
    }
}
