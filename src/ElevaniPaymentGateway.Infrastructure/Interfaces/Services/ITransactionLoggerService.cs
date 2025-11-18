using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Core.Models.Request.TransactionService;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services
{
    public interface ITransactionLoggerService : IAutoDependencyServices
    {
        Task<Transaction> LogTransactionAsync(string merchantId, PaymentGateways paymentGateway, TransactionRequest request);
    }
}
