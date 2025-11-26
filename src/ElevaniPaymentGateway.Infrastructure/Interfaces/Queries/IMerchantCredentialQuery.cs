using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Infrastructure.Autofac;
using System.Linq.Expressions;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Queries
{
    public interface IMerchantCredentialQuery : IAutoDependencyServices
    {
        Task<IQueryable<MerchantCredential>> ListAllAsync(bool loadNavigationProps = false);
        Task<IQueryable<MerchantCredential>> ListAsync(Expression<Func<MerchantCredential, bool>> predicate, bool loadNavigationProps = false);
        Task<MerchantCredential> GetByAsync(Expression<Func<MerchantCredential, bool>> predicate, bool loadNavigationProps = false);
    }
}
