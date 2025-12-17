using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Infrastructure.Autofac;
using System.Linq.Expressions;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Queries
{
    public interface IUserQuery : IAutoDependencyServices
    {
        Task<IQueryable<User>> ListAllAsync(bool loadNavigationProps = false);
        Task<IQueryable<User>> ListAsync(Expression<Func<User, bool>> predicate, bool loadNavigationProps = false);
        Task<User> GetByAsync(Expression<Func<User, bool>> predicate, bool loadNavigationProps = false);
    }
}
