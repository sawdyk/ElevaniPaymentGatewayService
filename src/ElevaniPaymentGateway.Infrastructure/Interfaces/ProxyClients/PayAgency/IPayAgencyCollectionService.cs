using ElevaniPaymentGateway.Core.Models.Request.PayAgency;
using ElevaniPaymentGateway.Core.Models.Response.PayAgency;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.ProxyClients.PayAgency
{
    public interface IPayAgencyCollectionService
    {
        Task<PayAgencyTransactionResponse> InitiateTransactionAsync(PayAgencyEncryptedRequest request);
        Task<PayAgencyTransactionResponse> TransactionStatusAsync(string transactionReference);
    }
}
