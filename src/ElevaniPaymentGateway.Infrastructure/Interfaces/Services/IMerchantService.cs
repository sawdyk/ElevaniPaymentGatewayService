using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Helpers.Pagination;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services
{
    public interface IMerchantService : IAutoDependencyServices
    {
        Task<GenericResponse<Merchant>> CreateAsync(MerchantRequest request);
        Task<GenericPagedResponse<Merchant>> MerchantsAsync(PaginationParams paginationParams);
        Task<GenericResponse<Merchant>> IdAsync(string id);
        Task<GenericResponse<Merchant>> SetStatusAsync(string id, bool isActive);
    }
}
