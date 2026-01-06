using AutoMapper;
using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Interfaces.EfRepository;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Queries;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Helpers;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Text;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IActivityLoggerService _activityLoggerService;
        private readonly IAccountNotificationEmailService _accountNotificationEmailService;
        private readonly ISqlTransactionService _sqlTransactionService;
        private readonly IBaseRepository<OTP> _otpRepository;
        private readonly IOTPService _oTPService;
        private readonly IOTPQuery _otpQuery;
        private readonly IUserQuery _userQuery;
        public AccountService(UserManager<User> userManager, ILogger<AccountService> logger,
            IMapper mapper, IActivityLoggerService activityLoggerService, IAccountNotificationEmailService accountNotificationEmailService,
            ISqlTransactionService sqlTransactionService, IBaseRepository<OTP> otpRepository, IOTPService oTPService, IOTPQuery oTPQuery, IUserQuery userQuery)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _activityLoggerService = activityLoggerService;
            _accountNotificationEmailService = accountNotificationEmailService;
            _sqlTransactionService = sqlTransactionService;
            _otpRepository = otpRepository;
            _oTPService = oTPService;
            _otpQuery = oTPQuery;
            _userQuery = userQuery;
        }
        public async Task<GenericResponse<UserDto>> ChangePasswordAsync(ChangePasswordRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.EmailAddress);
                if (user is null) throw new NotFoundException(ErrorMessages.UserNotFoundError);
                if (user.Status.Equals(UserStatus.InActive)) throw new GenericException(RespMsgConstants.InActiveUser);
                if (!request.ConfirmPassword.Equals(request.NewPassword.Trim())) throw new GenericException(RespMsgConstants.PasswordMismatch);

                var passwordChangedResult = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
                if (!passwordChangedResult.Succeeded)
                {
                    _logger.LogInformation("An error occurred; customer change password");
                    StringBuilder sb = new StringBuilder();
                    foreach (var error in passwordChangedResult.Errors)
                        sb.AppendLine(error.Description);

                    _logger.LogInformation($"customer change password errors => {sb.ToString()}");
                    throw new GenericException(sb.ToString());
                }

                user.UpdatedAt = DateTime.UtcNow;
                user.UpdatedBy = user.Id.ToString();
                user.LastPasswordChangeDate = DateTime.Now;

                await _userManager.UpdateAsync(user);

                //send mail for password change
                await _accountNotificationEmailService.SendResetAndChangedPasswordMailAsync(user);
                await _activityLoggerService.LogUserActivityAsync(ActivityTypes.Password, "Change Password");

                var userDto = _mapper.Map<UserDto>(user);

                return GenericResponse<UserDto>.Success(userDto, "Password changed successfully");
            }
            catch (Exception ex)
            when (ex is GenericException || ex is NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericResponse<UserDto>> ForgotPasswordAsync(string emailAddress)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(emailAddress);
                if (user is null) throw new GenericException($"User with email address '{emailAddress}' does not exist.");
                if (user.Status.Equals(UserStatus.InActive)) throw new GenericException($"User account is inactive. Please contact support");

                //email sending for OTP 
                await _accountNotificationEmailService.SendForgotPasswordMailAsync(user);

                var userDto = _mapper.Map<UserDto>(user);

                return GenericResponse<UserDto>.Success(userDto, "OTP for password reset has been sent to your registered email address");
            }
            catch (Exception ex)
            when (ex is GenericException || ex is NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericResponse> GenerateOTPAsync(GenerateOTPRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.EmailAddress);
                if (user is null) throw new NotFoundException(ErrorMessages.UserNotFoundError);

                //send mail for password change
                await _accountNotificationEmailService.SendForgotPasswordMailAsync(user);

                return GenericResponse.Success("New OTP has been generated and sent to your registered email address");
            }
            catch (Exception ex)
            when (ex is GenericException || ex is NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericResponse<UserDto>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.EmailAddress);
                if (user is null) throw new NotFoundException(ErrorMessages.UserNotFoundError);
                if (user.Status.Equals(UserStatus.InActive)) throw new GenericException(RespMsgConstants.InActiveUser);
                if (!request.ConfirmPassword.Equals(request.NewPassword.Trim())) throw new GenericException(RespMsgConstants.PasswordMismatch);

                var userOtp = await _otpQuery.GetByAsync(x => x.UserId == user.Id && x.OTPValue == request.OTP && x.OTPType == OTPTypes.ForgotPassword, false);
                if (userOtp is null) throw new GenericException(RespMsgConstants.InValidOTP);
                if (DateTime.Now > userOtp.ExpiryDateTime) throw new GenericException(RespMsgConstants.ExpiredOTP);
                if (userOtp.IsUsed) throw new GenericException(RespMsgConstants.InValidOTP);
                if (!userOtp.OTPValue.Equals(request.OTP.Trim())) throw new GenericException(RespMsgConstants.InValidOTP);

                var passwordResetResult = await _userManager.ResetPasswordAsync(user, userOtp.TokenValue, request.NewPassword);
                if (!passwordResetResult.Succeeded)
                {
                    _logger.LogInformation("An error occurred; customer reset password");
                    StringBuilder sb = new StringBuilder();
                    foreach (var error in passwordResetResult.Errors)
                        sb.AppendLine(error.Description);

                    _logger.LogInformation($"customer reset password errors => {sb.ToString()}");
                    throw new GenericException("An error occurred");
                }

                var dbTransaction = await _sqlTransactionService.BeginTransactionAsync();

                user.UpdatedAt = DateTime.UtcNow;
                user.UpdatedBy = user.Id.ToString();
                user.LastPasswordResetDate = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);

                userOtp.IsUsed = true;
                userOtp.DateUsed = DateTime.UtcNow;
                _otpRepository.Update(userOtp);
                await _otpRepository.SaveChangesAsync();

                await _sqlTransactionService.CommitAndDisposeTransactionAsync(dbTransaction);

                //send mail for password change
                await _accountNotificationEmailService.SendResetAndChangedPasswordMailAsync(user);

                var userDto = _mapper.Map<UserDto>(user);

                return GenericResponse<UserDto>.Success(userDto, "Successfully reset password");
            }
            catch (Exception ex)
            when (ex is GenericException || ex is NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericResponse<UserDto>> VerifyOTPAsync(ValidateOTPRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.EmailAddress);
                if (user is null) throw new NotFoundException(ErrorMessages.UserNotFoundError);

                if (request.OTPType.Equals(OTPTypes.ForgotPassword))
                    if (user.Status.Equals(UserStatus.InActive)) throw new GenericException(RespMsgConstants.InActiveUser);

                var userOtp = await _otpQuery.GetByAsync(x => x.UserId == user.Id && x.OTPValue == request.OTP && x.OTPType == request.OTPType, false);
                if (userOtp is null) throw new GenericException(RespMsgConstants.InValidOTP);
                if (DateTime.UtcNow > userOtp.ExpiryDateTime) throw new GenericException(RespMsgConstants.ExpiredOTP);
                if (userOtp.IsUsed) throw new GenericException(RespMsgConstants.InValidOTP);
                if (!userOtp.OTPValue.Equals(request.OTP.Trim())) throw new GenericException(RespMsgConstants.InValidOTP);

                var userDto = _mapper.Map<UserDto>(user);

                return GenericResponse<UserDto>.Success(userDto, "OTP verification successful");
            }
            catch (Exception ex)
            when (ex is GenericException || ex is NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }
    }
}
