using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Queries;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services
{
    public class ReportDataService : IReportDataService
    {
        private readonly ILogger<ReportDataService> _logger;
        private readonly ITransactionQuery _transactionQuery;
        private readonly IUserRoleQuery _userRoleQuery;
        public ReportDataService(ILogger<ReportDataService> logger, ITransactionQuery transactionQuery, IUserRoleQuery userRoleQuery)
        {
            _logger = logger;
            _transactionQuery = transactionQuery;
            _userRoleQuery = userRoleQuery;
        }

        public async Task<IQueryable<Transaction>> TransactionsReportAsync(TransactionReportRequest request)
        {
            try
            {
                IQueryable<Transaction> transactions = null;

                if (request.Status.HasValue && !string.IsNullOrEmpty(request.MerchantId))
                    transactions = (await _transactionQuery.ListAsync(x => x.Status == request.Status && x.MerchantId == request.MerchantId 
                    && x.CreatedAt.Date >= request.StartDate.Date && x.CreatedAt.Date <= request.EndDate.Date, true));
                else if (request.Status.HasValue && string.IsNullOrEmpty(request.MerchantId))
                    transactions = (await _transactionQuery.ListAsync(x => x.Status == request.Status
                    && x.CreatedAt.Date >= request.StartDate.Date && x.CreatedAt.Date <= request.EndDate.Date, true));
                else if (!request.Status.HasValue && !string.IsNullOrEmpty(request.MerchantId))
                    transactions = (await _transactionQuery.ListAsync(x => x.MerchantId == request.MerchantId
                    && x.CreatedAt.Date >= request.StartDate.Date && x.CreatedAt.Date <= request.EndDate.Date, true));
                else
                    transactions = (await _transactionQuery.ListAsync(x => x.CreatedAt.Date >= request.StartDate.Date && x.CreatedAt.Date <= request.EndDate.Date, true));

                return transactions;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                    $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }
    }
}
