using ElevaniPaymentGateway.Core.Helpers.Pagination;
using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Core.Models.Request.TransactionService;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Core.Models.Response.TransactionService;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services
{
    public interface ITransactionService : IAutoDependencyServices
    {
        Task<GenericResponse<TransactionResponse>> InitiateGratipPaymentCollectionAsync(TransactionRequest request);
        Task<GenericResponse<PATransactionResponse>> InitiatePayAgencyServerToServerAsync(PAEncryptedTransactionRequest encryptedRequest);

        Task<GenericResponse<MerchantTransactionDto>> StatusAsync(string reference);
        Task<GenericPagedResponse<MerchantTransactionDto>> MerchantAsync(PaginationParams paginationParams);
        Task<GenericPagedResponse<TransactionDto>> MerchantIdAsync(string merchantId, PaginationParams paginationParams);
    }
}
