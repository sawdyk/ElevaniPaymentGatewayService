using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services
{
    public interface IAuthenticationService : IAutoDependencyServices
    {
        Task<GenericResponse<UserLoginResponse>> LoginAsync(LoginRequest request);
        Task<GenericResponse<LoginResponse>> RefreshToken(RefreshTokenRequest request);
    }
}
