using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Infrastructure.Autofac;
using System.Linq.Expressions;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Queries
{
    public interface ITransactionQuery : IAutoDependencyServices
    {
        Task<IQueryable<Transaction>> ListAllAsync(bool loadNavigationProps = false);
        Task<IQueryable<Transaction>> ListAsync(Expression<Func<Transaction, bool>> predicate, bool loadNavigationProps = false);
        Task<Transaction> GetByAsync(Expression<Func<Transaction, bool>> predicate, bool loadNavigationProps = false);
    }
}
