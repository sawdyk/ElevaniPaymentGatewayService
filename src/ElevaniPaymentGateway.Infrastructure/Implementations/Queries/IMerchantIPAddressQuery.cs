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
    public class MerchantIPAddressQuery : IMerchantIPAddressQuery
    {
        private readonly ILogger<MerchantIPAddressQuery> _logger;
        private readonly AppDbContext _dbContext;
        public MerchantIPAddressQuery(ILogger<MerchantIPAddressQuery> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<MerchantIPAddress> GetByAsync(Expression<Func<MerchantIPAddress, bool>> predicate, bool loadNavigationProps = false)
        {
            try
            {
                MerchantIPAddress? MerchantIPAddress = null;
                if (loadNavigationProps)
                    MerchantIPAddress = await _dbContext.Set<MerchantIPAddress>().Where(predicate)
                         .Include(x => x.Merchant)
                        .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();
                else
                    MerchantIPAddress = await _dbContext.Set<MerchantIPAddress>().Where(predicate)
                      .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();

                return MerchantIPAddress;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<IQueryable<MerchantIPAddress>> ListAllAsync(bool loadNavigationProps = false)
        {
            try
            {
                IQueryable<MerchantIPAddress>? MerchantIPAddress = null;
                if (loadNavigationProps)
                    MerchantIPAddress = _dbContext.Set<MerchantIPAddress>()
                          .Include(x => x.Merchant)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();
                else
                    MerchantIPAddress = _dbContext.Set<MerchantIPAddress>()
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();

                return MerchantIPAddress;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<IQueryable<MerchantIPAddress>> ListAsync(Expression<Func<MerchantIPAddress, bool>> predicate, bool loadNavigationProps = false)
        {
            try
            {
                IQueryable<MerchantIPAddress>? MerchantIPAddress = null;
                if (loadNavigationProps)
                    MerchantIPAddress = _dbContext.Set<MerchantIPAddress>().Where(predicate)
                           .Include(x => x.Merchant)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();
                else
                    MerchantIPAddress = _dbContext.Set<MerchantIPAddress>().Where(predicate)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();

                return MerchantIPAddress;
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
