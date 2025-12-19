using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services
{
    public interface IReportDataService : IAutoDependencyServices
    {
        Task<IQueryable<Transaction>> TransactionsReportAsync(TransactionReportRequest request);
    }
}
