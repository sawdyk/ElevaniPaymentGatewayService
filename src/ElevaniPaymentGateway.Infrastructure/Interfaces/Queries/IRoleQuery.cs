using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Infrastructure.Autofac;
using System.Linq.Expressions;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Queries
{
    public interface IRoleQuery : IAutoDependencyServices
    {
        Task<IQueryable<Role>> ListAllAsync(bool loadNavigationProps = false);
        Task<IQueryable<Role>> ListAsync(Expression<Func<Role, bool>> predicate, bool loadNavigationProps = false);
        Task<Role> GetByAsync(Expression<Func<Role, bool>> predicate, bool loadNavigationProps = false);
    }
}
