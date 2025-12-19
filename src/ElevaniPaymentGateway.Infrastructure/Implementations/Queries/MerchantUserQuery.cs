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
    public class MerchantUserQuery : IMerchantUserQuery
    {
        private readonly ILogger<MerchantUserQuery> _logger;
        private readonly AppDbContext _dbContext;
        public MerchantUserQuery(ILogger<MerchantUserQuery> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<MerchantUser> GetByAsync(Expression<Func<MerchantUser, bool>> predicate, bool loadNavigationProps = false)
        {
            try
            {
                MerchantUser? MerchantUser = null;
                if (loadNavigationProps)
                    MerchantUser = await _dbContext.Set<MerchantUser>().Where(predicate)
                         .Include(x => x.User).Include(x => x.Merchant)
                        .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();
                else
                    MerchantUser = await _dbContext.Set<MerchantUser>().Where(predicate)
                      .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();

                return MerchantUser;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<IQueryable<MerchantUser>> ListAllAsync(bool loadNavigationProps = false)
        {
            try
            {
                IQueryable<MerchantUser>? MerchantUser = null;
                if (loadNavigationProps)
                    MerchantUser = _dbContext.Set<MerchantUser>()
                         .Include(x => x.User).Include(x => x.Merchant)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();
                else
                    MerchantUser = _dbContext.Set<MerchantUser>()
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();

                return MerchantUser;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<IQueryable<MerchantUser>> ListAsync(Expression<Func<MerchantUser, bool>> predicate, bool loadNavigationProps = false)
        {
            try
            {
                IQueryable<MerchantUser>? MerchantUser = null;
                if (loadNavigationProps)
                    MerchantUser = _dbContext.Set<MerchantUser>().Where(predicate)
                           .Include(x => x.User).Include(x => x.Merchant)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();
                else
                    MerchantUser = _dbContext.Set<MerchantUser>().Where(predicate)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();

                return MerchantUser;
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
