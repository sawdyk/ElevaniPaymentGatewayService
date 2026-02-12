using ElevaniPaymentGateway.Core.Models.Request.TransactionService;
using ElevaniPaymentGateway.Core.Models.Response.TransactionService;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services.PaymentGateway.PayAgency
{
    public interface IPayAgencyPaymentService : IAutoDependencyServices
    {
        Task<PATransactionResponse> ServerToServerAsync(PAEncryptedTransactionRequest encryptedRequest);
    }
}
