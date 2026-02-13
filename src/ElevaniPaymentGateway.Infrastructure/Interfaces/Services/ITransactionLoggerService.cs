using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Models.Request.TransactionService;
using ElevaniPaymentGateway.Core.Models.Response.PayAgency;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services
{
    public interface ITransactionLoggerService : IAutoDependencyServices
    {
        Task<Transaction> LogGratipTransactionAsync(TransactionRequest request);
        Task<Transaction> LogPayAgencyTransactionAsync(PATransactionRequest request, PayAgencyTransactionResponse response);
    }
}
