using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Infrastructure.Autofac;
using System.Linq.Expressions;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Queries
{
    public interface IMerchantUserQuery : IAutoDependencyServices
    {
        Task<IQueryable<MerchantUser>> ListAllAsync(bool loadNavigationProps = false);
        Task<IQueryable<MerchantUser>> ListAsync(Expression<Func<MerchantUser, bool>> predicate, bool loadNavigationProps = false);
        Task<MerchantUser> GetByAsync(Expression<Func<MerchantUser, bool>> predicate, bool loadNavigationProps = false);
    }
}
