using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Infrastructure.Autofac;
using System.Linq.Expressions;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Queries
{
    public interface IGratipTransactionQuery : IAutoDependencyServices
    {
        Task<IQueryable<GratipTransaction>> ListAllAsync(bool loadNavigationProps = false);
        Task<IQueryable<GratipTransaction>> ListAsync(Expression<Func<GratipTransaction, bool>> predicate, bool loadNavigationProps = false);
        Task<GratipTransaction> GetByAsync(Expression<Func<GratipTransaction, bool>> predicate, bool loadNavigationProps = false);
    }
}
