using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Infrastructure.Autofac;
using System.Linq.Expressions;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Queries
{
    public interface IMerchantIPAddressQuery : IAutoDependencyServices
    {
        Task<IQueryable<MerchantIPAddress>> ListAllAsync(bool loadNavigationProps = false);
        Task<IQueryable<MerchantIPAddress>> ListAsync(Expression<Func<MerchantIPAddress, bool>> predicate, bool loadNavigationProps = false);
        Task<MerchantIPAddress> GetByAsync(Expression<Func<MerchantIPAddress, bool>> predicate, bool loadNavigationProps = false);
    }
}
