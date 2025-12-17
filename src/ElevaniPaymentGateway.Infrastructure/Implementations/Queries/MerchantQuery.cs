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
    public class MerchantQuery : IMerchantQuery
    {
        private readonly ILogger<MerchantQuery> _logger;
        private readonly AppDbContext _dbContext;
        public MerchantQuery(ILogger<MerchantQuery> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<Merchant> GetByAsync(Expression<Func<Merchant, bool>> predicate, bool loadNavigationProps = false)
        {
            try
            {
                Merchant? Merchant = null;
                if (loadNavigationProps)
                    Merchant = await _dbContext.Set<Merchant>().Where(predicate)
                         .Include(x => x.MerchantCredential).Include(x => x.MerchantIPAddresses)
                        .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();
                else
                    Merchant = await _dbContext.Set<Merchant>().Where(predicate)
                      .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();

                return Merchant;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<IQueryable<Merchant>> ListAllAsync(bool loadNavigationProps = false)
        {
            try
            {
                IQueryable<Merchant>? Merchant = null;
                if (loadNavigationProps)
                    Merchant = _dbContext.Set<Merchant>()
                          .Include(x => x.MerchantCredential).Include(x => x.MerchantIPAddresses)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();
                else
                    Merchant = _dbContext.Set<Merchant>()
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();

                return Merchant;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<IQueryable<Merchant>> ListAsync(Expression<Func<Merchant, bool>> predicate, bool loadNavigationProps = false)
        {
            try
            {
                IQueryable<Merchant>? Merchant = null;
                if (loadNavigationProps)
                    Merchant = _dbContext.Set<Merchant>().Where(predicate)
                           .Include(x => x.MerchantCredential).Include(x => x.MerchantIPAddresses)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();
                else
                    Merchant = _dbContext.Set<Merchant>().Where(predicate)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();

                return Merchant;
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
