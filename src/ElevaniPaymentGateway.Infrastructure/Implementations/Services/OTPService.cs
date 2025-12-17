using ElevaniPaymentGateway.Core.Configs;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Infrastructure.Interfaces.EfRepository;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Queries;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services
{
    public class OTPService : IOTPService
    {
        private readonly IBaseRepository<OTP> _OTPRepository;
        private readonly ILogger<OTPService> _logger;
        private readonly UserManager<User> _userManager;
        private readonly AppSettingsConfig _appSettingsConfig;
        private readonly IOTPQuery _oTPQuery;
        public OTPService(ILogger<OTPService> logger, IBaseRepository<OTP> OTPRepository,
            UserManager<User> userManager, IOptions<AppSettingsConfig> appSettingsConfig,
            IOTPQuery oTPQuery)
        {
            _logger = logger;
            _OTPRepository = OTPRepository;
            _userManager = userManager;
            _appSettingsConfig = appSettingsConfig.Value;
            _oTPQuery = oTPQuery;
        }
        public async Task<OTP> GenerateOTPAsync(User user, OTPTypes oTPType)
        {
            try
            {
                Random generator = new Random();
                string otpValue = generator.Next(0, 10000).ToString("D4");
                var tokenValue = await _userManager.GeneratePasswordResetTokenAsync(user);

                var userOtp = await _oTPQuery.GetByAsync(x => x.UserId == user.Id && x.OTPType == oTPType, false);
                if (userOtp is null)
                {
                    var newOtp = new OTP
                    {
                        UserId = user.Id,
                        OTPValue = otpValue,
                        TokenValue = tokenValue,
                        OTPType = oTPType,
                        ExpiryDateTime = DateTime.Now.AddMinutes(_appSettingsConfig.OTPExpiry),
                        CreatedBy = "Sytem"
                    };

                    _OTPRepository.Add(newOtp);
                    await _OTPRepository.SaveChangesAsync();

                    return newOtp;
                }
                else
                {
                    userOtp.UserId = user.Id;
                    userOtp.OTPValue = otpValue;
                    userOtp.TokenValue = tokenValue;
                    userOtp.OTPType = oTPType;
                    userOtp.DateGenerated = DateTime.Now;
                    userOtp.IsUsed = false;
                    userOtp.DateUsed = null;
                    userOtp.ExpiryDateTime = DateTime.Now.AddMinutes(_appSettingsConfig.OTPExpiry);
                    userOtp.UpdatedBy = "Sytem";
                    userOtp.UpdatedAt = DateTime.Now;

                    _OTPRepository.Update(userOtp);
                    await _OTPRepository.SaveChangesAsync();

                    return userOtp;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to generate and save new OTP - {ex.Message}");
                throw;
            }
        }
    }
}
