using ElevaniPaymentGateway.Core.Models.Request.TransactionService;
using ElevaniPaymentGateway.Core.Models.Response.TransactionService;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services.PaymentGateway.Gratip
{
    public interface IGratipPaymentService : IAutoDependencyServices
    {
        Task<TransactionResponse> InitiateTransactionAsync(string merchantId, TransactionRequest request);
    }
}
