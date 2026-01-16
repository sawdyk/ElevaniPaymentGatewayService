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
        Task<GenericResponse<TransactionResponse>> InitiateTransactionViaPaymentGatewayAsync(TransactionRequest request);
        Task<GenericResponse<TransactionDto>> StatusAsync(string reference);
        Task<GenericPagedResponse<TransactionDto>> MerchantAsync(PaginationParams paginationParams);
        Task<GenericPagedResponse<TransactionDto>> MerchantIdAsync(string merchantId, PaginationParams paginationParams);


        Task<GenericResponse<PATransactionResponse>> InitiateTransactionViaServerAsync(string encryptedRequest);
    }
}
