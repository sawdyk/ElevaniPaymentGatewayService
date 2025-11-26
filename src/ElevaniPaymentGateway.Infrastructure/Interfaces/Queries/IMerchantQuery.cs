using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Infrastructure.Autofac;
using System.Linq.Expressions;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Queries
{
    public interface IMerchantQuery : IAutoDependencyServices
    {
        Task<IQueryable<Merchant>> ListAllAsync(bool loadNavigationProps = false);
        Task<IQueryable<Merchant>> ListAsync(Expression<Func<Merchant, bool>> predicate, bool loadNavigationProps = false);
        Task<Merchant> GetByAsync(Expression<Func<Merchant, bool>> predicate, bool loadNavigationProps = false);
    }
}
