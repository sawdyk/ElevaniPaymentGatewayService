using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Infrastructure.Autofac;
using System.Linq.Expressions;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Queries
{
    public interface IUserRoleQuery : IAutoDependencyServices
    {
        Task<IQueryable<UserRole>> ListAllAsync(bool loadNavigationProps = false);
        Task<IQueryable<UserRole>> ListAsync(Expression<Func<UserRole, bool>> predicate, bool loadNavigationProps = false);
        Task<UserRole> GetByAsync(Expression<Func<UserRole, bool>> predicate, bool loadNavigationProps = false);
    }
}
