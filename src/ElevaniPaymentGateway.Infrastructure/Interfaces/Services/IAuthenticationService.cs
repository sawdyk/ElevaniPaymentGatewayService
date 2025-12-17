using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services
{
    public interface IAuthenticationService : IAutoDependencyServices
    {
        Task<GenericResponse<MerchantUserLoginResponse>> MerchantLoginAsync(LoginRequest request);
        Task<GenericResponse<AdminUserLoginResponse>> AdminLoginAsync(LoginRequest request);
        Task<GenericResponse<LoginResponse>> RefreshToken(RefreshTokenRequest request);
    }
}
