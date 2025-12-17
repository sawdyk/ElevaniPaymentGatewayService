using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services
{
    public interface IAccountService : IAutoDependencyServices
    {
        Task<GenericResponse<UserDto>> VerifyOTPAsync(ValidateOTPRequest request);
        Task<GenericResponse<UserDto>> ForgotPasswordAsync(string emailAddress);
        Task<GenericResponse<UserDto>> ChangePasswordAsync(ChangePasswordRequest request);
        Task<GenericResponse<UserDto>> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest);
        Task<GenericResponse> GenerateOTPAsync(GenerateOTPRequest request);
    }
}
