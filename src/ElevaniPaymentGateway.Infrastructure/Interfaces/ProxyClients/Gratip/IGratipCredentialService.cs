using ElevaniPaymentGateway.Core.Models.Response.Gratip;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.ProxyClients.Gratip
{
    public interface IGratipCredentialService
    {
        Task<RotateCredentialResponse> RotateCredentialAsync();
    }
}
