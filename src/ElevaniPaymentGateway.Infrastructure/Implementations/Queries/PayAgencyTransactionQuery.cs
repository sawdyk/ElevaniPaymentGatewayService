using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Queries;
using ElevaniPaymentGateway.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Queries
{
    public class PayAgencyTransactionQuery : IPayAgencyTransactionQuery
    {
        private readonly ILogger<PayAgencyTransactionQuery> _logger;
        private readonly AppDbContext _dbContext;
        public PayAgencyTransactionQuery(ILogger<PayAgencyTransactionQuery> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<PayAgencyTransaction> GetByAsync(Expression<Func<PayAgencyTransaction, bool>> predicate, bool loadNavigationProps = false)
        {
            try
            {
                PayAgencyTransaction? PayAgencyTransaction = null;
                if (loadNavigationProps)
                    PayAgencyTransaction = await _dbContext.Set<PayAgencyTransaction>().Where(predicate)
                          .Include(x => x.Transaction).AsNoTracking()
                        .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();
                else
                    PayAgencyTransaction = await _dbContext.Set<PayAgencyTransaction>().Where(predicate)
                      .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();

                return PayAgencyTransaction;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<IQueryable<PayAgencyTransaction>> ListAllAsync(bool loadNavigationProps = false)
        {
            try
            {
                IQueryable<PayAgencyTransaction>? PayAgencyTransaction = null;
                if (loadNavigationProps)
                    PayAgencyTransaction = _dbContext.Set<PayAgencyTransaction>()
                          .Include(x => x.Transaction).AsNoTracking()
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();
                else
                    PayAgencyTransaction = _dbContext.Set<PayAgencyTransaction>()
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();

                return PayAgencyTransaction;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<IQueryable<PayAgencyTransaction>> ListAsync(Expression<Func<PayAgencyTransaction, bool>> predicate, bool loadNavigationProps = false)
        {
            try
            {
                IQueryable<PayAgencyTransaction>? PayAgencyTransaction = null;
                if (loadNavigationProps)
                    PayAgencyTransaction = _dbContext.Set<PayAgencyTransaction>().Where(predicate)
                            .Include(x => x.Transaction).AsNoTracking()
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();
                else
                    PayAgencyTransaction = _dbContext.Set<PayAgencyTransaction>().Where(predicate)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();

                return PayAgencyTransaction;
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
