using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services
{
    public interface IAuthenticationService : IAutoDependencyServices
    {
        Task<GenericResponse<LoginResponse>> MerchantLoginAsync(LoginRequest request);
        Task<GenericResponse<LoginResponse>> AdminLoginAsync(LoginRequest request);
        Task<GenericResponse<LoginResponse>> RefreshToken(RefreshTokenRequest request);
    }
}
