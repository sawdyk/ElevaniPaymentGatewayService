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
    public class MerchantCredentialQuery : IMerchantCredentialQuery
    {
        private readonly ILogger<MerchantCredentialQuery> _logger;
        private readonly AppDbContext _dbContext;
        public MerchantCredentialQuery(ILogger<MerchantCredentialQuery> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<MerchantCredential> GetByAsync(Expression<Func<MerchantCredential, bool>> predicate, bool loadNavigationProps = false)
        {
            try
            {
                MerchantCredential? MerchantCredential = null;
                if (loadNavigationProps)
                    MerchantCredential = await _dbContext.Set<MerchantCredential>().Where(predicate)
                         .Include(x => x.Merchant)
                        .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();
                else
                    MerchantCredential = await _dbContext.Set<MerchantCredential>().Where(predicate)
                      .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();

                return MerchantCredential;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<IQueryable<MerchantCredential>> ListAllAsync(bool loadNavigationProps = false)
        {
            try
            {
                IQueryable<MerchantCredential>? MerchantCredential = null;
                if (loadNavigationProps)
                    MerchantCredential = _dbContext.Set<MerchantCredential>()
                          .Include(x => x.Merchant)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();
                else
                    MerchantCredential = _dbContext.Set<MerchantCredential>()
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();

                return MerchantCredential;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<IQueryable<MerchantCredential>> ListAsync(Expression<Func<MerchantCredential, bool>> predicate, bool loadNavigationProps = false)
        {
            try
            {
                IQueryable<MerchantCredential>? MerchantCredential = null;
                if (loadNavigationProps)
                    MerchantCredential = _dbContext.Set<MerchantCredential>().Where(predicate)
                           .Include(x => x.Merchant)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();
                else
                    MerchantCredential = _dbContext.Set<MerchantCredential>().Where(predicate)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();

                return MerchantCredential;
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
