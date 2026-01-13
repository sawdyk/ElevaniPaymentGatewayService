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
    public class TransactionQuery : ITransactionQuery
    {
        private readonly ILogger<TransactionQuery> _logger;
        private readonly AppDbContext _dbContext;
        public TransactionQuery(ILogger<TransactionQuery> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<Transaction> GetByAsync(Expression<Func<Transaction, bool>> predicate, bool loadNavigationProps = false)
        {
            try
            {
                Transaction? Transaction = null;
                if (loadNavigationProps)
                    Transaction = await _dbContext.Set<Transaction>().Where(predicate)
                         .Include(x => x.Merchant).AsNoTracking()
                         .Include(x => x.GratipTransaction).AsNoTracking()
                        .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();
                else
                    Transaction = await _dbContext.Set<Transaction>().Where(predicate)
                      .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();

                return Transaction;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<IQueryable<Transaction>> ListAllAsync(bool loadNavigationProps = false)
        {
            try
            {
                IQueryable<Transaction>? Transaction = null;
                if (loadNavigationProps)
                    Transaction = _dbContext.Set<Transaction>()
                          .Include(x => x.Merchant).AsNoTracking()
                         .Include(x => x.GratipTransaction).AsNoTracking()
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();
                else
                    Transaction = _dbContext.Set<Transaction>()
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();

                return Transaction;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<IQueryable<Transaction>> ListAsync(Expression<Func<Transaction, bool>> predicate, bool loadNavigationProps = false)
        {
            try
            {
                IQueryable<Transaction>? Transaction = null;
                if (loadNavigationProps)
                    Transaction = _dbContext.Set<Transaction>().Where(predicate)
                           .Include(x => x.Merchant).AsNoTracking()
                         .Include(x => x.GratipTransaction).AsNoTracking()
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();
                else
                    Transaction = _dbContext.Set<Transaction>().Where(predicate)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();

                return Transaction;
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
