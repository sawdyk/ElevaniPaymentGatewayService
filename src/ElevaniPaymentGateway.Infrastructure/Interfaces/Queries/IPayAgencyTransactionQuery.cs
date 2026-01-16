using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Infrastructure.Autofac;
using System.Linq.Expressions;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Queries
{
    public interface IPayAgencyTransactionQuery : IAutoDependencyServices
    {
        Task<IQueryable<PayAgencyTransaction>> ListAllAsync(bool loadNavigationProps = false);
        Task<IQueryable<PayAgencyTransaction>> ListAsync(Expression<Func<PayAgencyTransaction, bool>> predicate, bool loadNavigationProps = false);
        Task<PayAgencyTransaction> GetByAsync(Expression<Func<PayAgencyTransaction, bool>> predicate, bool loadNavigationProps = false);
    }
}
