using ElevaniPaymentGateway.Infrastructure.Autofac;
using Microsoft.EntityFrameworkCore.Storage;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Utilities
{
    public interface ISqlTransactionService : IAutoDependencyServices
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitAndDisposeTransactionAsync(IDbContextTransaction transaction);
        Task RollBackTransactionAsync(IDbContextTransaction transaction);
    }
}
