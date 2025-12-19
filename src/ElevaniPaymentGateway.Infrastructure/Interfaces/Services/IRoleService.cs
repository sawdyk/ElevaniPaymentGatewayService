using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services
{
    public interface IRoleService : IAutoDependencyServices
    {
        Task<GenericResponse<List<RoleDto>>> RolesAsync();
        Task<GenericResponse<RoleDto>> IdAsync(Guid id);
        Task<GenericResponse<List<RoleDto>>> RolesByRoleTypeAsync(RoleTypes roleType);
    }
}
