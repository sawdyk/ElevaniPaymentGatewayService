using ElevaniPaymentGateway.Core.Models.Request.Gratip;
using ElevaniPaymentGateway.Core.Models.Response.Gratip;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.ProxyClients.Gratip
{
    public interface IGratipCollectionService
    {
        Task<InitiateTransctionResponse> InitiateTransactionAsync(InitiateTransactionRequest request);
        Task<TransactionStatusResponse> TransactionStatusAsync(string transactionReference);
        Task<ListTransactionResponse> ListTransactionsAsync(string status, int limit, int page, DateTime startDate);
        Task<FinalizeTransactionResponse> FinalizeTransactionAsync(FinalizeTransactionRequest request);
    }
}
