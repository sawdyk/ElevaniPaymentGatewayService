using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services
{
    public interface IMerchantIPAddressService : IAutoDependencyServices
    {
        Task<GenericResponse<MerchantIPAddress>> CreateAsync(MerchantIPAddressRequest request);
        Task<GenericResponse<List<MerchantIPAddress>>> MerchantIdAsync(string merchantId);
        Task<GenericResponse<MerchantIPAddress>> IdAsync(Guid id);
        Task<GenericResponse<MerchantIPAddress>> UpdateAsync(Guid id, MerchantIPAddressRequest request);
        Task<GenericResponse> DeleteAsync(Guid id);
    }
}
