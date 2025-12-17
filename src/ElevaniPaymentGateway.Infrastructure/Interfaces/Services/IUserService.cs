using ElevaniPaymentGateway.Core.Helpers.Pagination;
using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services
{
    public interface IUserService : IAutoDependencyServices
    {
        Task<GenericResponse<UserDto>> CreateMerchantUserAsync(MerchantUserRequest request);
        Task<GenericResponse<UserDto>> UpdateMerchantUserAsync(UpdateMerchantUserRequest request);

        Task<GenericResponse<UserDto>> CreateAdminUserAsync(AdminUserRequest request);
        Task<GenericResponse<UserDto>> UpdateAdminUserAsync(UpdateUserRequest request);

        Task<GenericResponse<UserRoleDto>> UserByIdAsync(Guid id);
        Task<GenericPagedResponse<UserRoleDto>> MerchantUsersAsync(PaginationParams paginationParams);
        Task<GenericPagedResponse<UserDto>> MerchantUsersByMerchantIdAsync(string merchantId, PaginationParams paginationParams);
        Task<GenericPagedResponse<UserRoleDto>> AdminUsersAsync(PaginationParams paginationParams);
        Task<GenericResponse<UserDto>> SetStatusAsync(UserStatusRequest request);
        Task<GenericResponse> DeleteAsync(Guid id);
    }
}
