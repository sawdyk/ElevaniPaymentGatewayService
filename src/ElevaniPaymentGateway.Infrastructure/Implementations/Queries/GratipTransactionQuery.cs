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
    public class GratipTransactionQuery : IGratipTransactionQuery
    {
        private readonly ILogger<GratipTransactionQuery> _logger;
        private readonly AppDbContext _dbContext;
        public GratipTransactionQuery(ILogger<GratipTransactionQuery> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<GratipTransaction> GetByAsync(Expression<Func<GratipTransaction, bool>> predicate, bool loadNavigationProps = false)
        {
            try
            {
                GratipTransaction? GratipTransaction = null;
                if (loadNavigationProps)
                    GratipTransaction = await _dbContext.Set<GratipTransaction>().Where(predicate)
                         .Include(x => x.Transaction)
                        .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();
                else
                    GratipTransaction = await _dbContext.Set<GratipTransaction>().Where(predicate)
                      .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();

                return GratipTransaction;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<IQueryable<GratipTransaction>> ListAllAsync(bool loadNavigationProps = false)
        {
            try
            {
                IQueryable<GratipTransaction>? GratipTransaction = null;
                if (loadNavigationProps)
                    GratipTransaction = _dbContext.Set<GratipTransaction>()
                          .Include(x => x.Transaction)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();
                else
                    GratipTransaction = _dbContext.Set<GratipTransaction>()
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();

                return GratipTransaction;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<IQueryable<GratipTransaction>> ListAsync(Expression<Func<GratipTransaction, bool>> predicate, bool loadNavigationProps = false)
        {
            try
            {
                IQueryable<GratipTransaction>? GratipTransaction = null;
                if (loadNavigationProps)
                    GratipTransaction = _dbContext.Set<GratipTransaction>().Where(predicate)
                           .Include(x => x.Transaction)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();
                else
                    GratipTransaction = _dbContext.Set<GratipTransaction>().Where(predicate)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();

                return GratipTransaction;
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
