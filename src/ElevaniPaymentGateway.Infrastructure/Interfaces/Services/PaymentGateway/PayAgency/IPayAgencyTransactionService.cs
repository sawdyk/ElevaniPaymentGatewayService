using ElevaniPaymentGateway.Core.Models.Request.TransactionService;
using ElevaniPaymentGateway.Core.Models.Response.PayAgency;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services.PaymentGateway.PayAgency
{
    public interface IPayAgencyTransactionService : IAutoDependencyServices
    {
        Task LogPayAgencyTransactionsAsync(Guid transactionId, PATransactionRequest request, PayAgencyTransactionResponse response);
    }
}
