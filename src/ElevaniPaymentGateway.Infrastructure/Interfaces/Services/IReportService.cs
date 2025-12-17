using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Helpers.Pagination;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services
{
    public interface IReportService : IAutoDependencyServices
    {
        Task<GenericResponse<ReportResponse<Transaction>>> TransactionsReportAsync(TransactionReportRequest request, PaginationParams paginationParams);
    }
}
