using ElevaniPaymentGateway.Core.Models.Request.Gratip;
using ElevaniPaymentGateway.Core.Models.Response.Gratip;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services.PaymentGateway.Gratip
{
    public interface IGratipTransactionService : IAutoDependencyServices
    {
        Task LogGratipTransactionsAsync(Guid transactionId, InitiateTransactionRequest request, InitiateTransctionResponse response);
    }
}
