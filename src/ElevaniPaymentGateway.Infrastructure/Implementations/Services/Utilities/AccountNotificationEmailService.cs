using ElevaniPaymentGateway.Core.Configs;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Helpers;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services.Utilities
{
    public class AccountNotificationEmailService : IAccountNotificationEmailService
    {
        private readonly ILogger<AccountNotificationEmailService> _logger;
        private readonly IEmailService _emailService;
        private readonly AppSettingsConfig _appSettingsConfig;
        private readonly IOTPService _oTPService;
        public AccountNotificationEmailService(ILogger<AccountNotificationEmailService> logger, IEmailService emailService,
           IOptions<AppSettingsConfig> appSettingsConfig, IOTPService oTPService)
        {
            _logger = logger;
            _emailService = emailService;
            _appSettingsConfig = appSettingsConfig.Value;
            _oTPService = oTPService;
        }

        public async Task SendForgotPasswordMailAsync(User user)
        {
            try
            {
                string emailTemplate = File.ReadAllText("wwwroot/MailTemplates/Account/ForgotPasswordTemp.html");
                string firstName = user.FirstName;
                string emailAddress = user.Email;
                string expiryTime = _appSettingsConfig.OTPExpiry.ToString();

                List<string> recipient = new List<string> { emailAddress };
                var otp = await _oTPService.GenerateOTPAsync(user, OTPTypes.ForgotPassword);
                if (otp is null)
                {
                    _logger.LogInformation("OTP generation failed. An error occuured while generating OTP");
                    throw new GenericException("An error occuured");
                }

                emailTemplate = emailTemplate.Replace("{name}", firstName.FirstCharToUpper());
                emailTemplate = emailTemplate.Replace("{OTP}", otp.OTPValue);
                emailTemplate = emailTemplate.Replace("{expiryTime}", expiryTime);

                EmailRequest msg = new()
                {
                    RecipientTo = recipient,
                    Subject = "Reset Password",
                    Body = emailTemplate
                };

                await _emailService.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to send forgot password email >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
            }
        }

        public async Task SendNewAdminUserMailAsync(User user, string defaultPassword)
        {
            try
            {
                string emailTemplate = File.ReadAllText("wwwroot/MailTemplates/Account/NewUserTemp.html");
                string firstName = user.FirstName;
                string emailAddress = user.Email;

                emailTemplate = emailTemplate.Replace("{name}", firstName.FirstCharToUpper());
                emailTemplate = emailTemplate.Replace("{defaultPassword}", defaultPassword);

                EmailRequest msg = new()
                {
                    RecipientTo = new List<string> { emailAddress },
                    Subject = "Onboarding",
                    Body = emailTemplate
                };

                await _emailService.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to send new admin user email >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
            }
        }

        public async Task SendResetAndChangedPasswordMailAsync(User user)
        {
            try
            {
                string emailTemplate = File.ReadAllText("wwwroot/MailTemplates/Account/ChangedPasswordTemp.html");
                emailTemplate = emailTemplate.Replace("{name}", $"{user.FirstName.FirstCharToUpper()}");
                emailTemplate = emailTemplate.Replace("{date}", DateTime.Now.ToString("dddd, MMMM yyyy, hh:mm:ss tt"));

                EmailRequest msg = new()
                {
                    RecipientTo = new List<string> { user.Email },
                    Subject = "Changed Password",
                    Body = emailTemplate,
                };

                await _emailService.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to send reset/change password email >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
            }
        }
    }
}
