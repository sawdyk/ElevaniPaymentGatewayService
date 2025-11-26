using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services
{
    public interface IMerchantCredentialService : IAutoDependencyServices
    {
        //generate new merchant authentication token
        Task<GenericResponse<JwtResponse>> GenerateAuthenticationTokenAsync(MerchantAuthTokenRequest request);

        //generate new credentials for merchant
        Task<GenericResponse<MerchantCredential>> GenerateCredentialsAsync(MerchantCredentialRequest request);
    }
}
