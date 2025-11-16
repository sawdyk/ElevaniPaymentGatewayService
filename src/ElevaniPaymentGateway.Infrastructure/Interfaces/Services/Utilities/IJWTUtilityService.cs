using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Utilities
{
    public interface IJWTUtilityService : IAutoDependencyServices
    {
        Task<JwtResponse> GenerateMerchantAuthenticationAccessToken(Merchant merchant);
        Task<JwtResponse> GenerateAccessToken(UserDto user);
        string GenerateRefreshToken();
        Task<LoginResponse> RefreshToken(RefreshTokenRequest request);
    }
}
