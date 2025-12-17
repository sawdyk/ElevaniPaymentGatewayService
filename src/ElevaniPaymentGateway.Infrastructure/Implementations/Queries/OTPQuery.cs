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
    public class OTPQuery : IOTPQuery
    {
        private readonly ILogger<OTPQuery> _logger;
        private readonly AppDbContext _dbContext;
        public OTPQuery(ILogger<OTPQuery> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<OTP> GetByAsync(Expression<Func<OTP, bool>> predicate, bool loadNavigationProps)
        {
            try
            {
                OTP? OTP = null;
                if (loadNavigationProps)
                    OTP = await _dbContext.Set<OTP>().Where(predicate)
                        .Include(x => x.User)
                        .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();
                else
                    OTP = await _dbContext.Set<OTP>().Where(predicate)
                      .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();

                return OTP;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<IQueryable<OTP>> ListAllAsync(bool loadNavigationProps)
        {
            try
            {
                IQueryable<OTP>? OTP = null;
                if (loadNavigationProps)
                    OTP = _dbContext.Set<OTP>()
                        .Include(x => x.User)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();
                else
                    OTP = _dbContext.Set<OTP>()
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();

                return OTP;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<IQueryable<OTP>> ListAsync(Expression<Func<OTP, bool>> predicate, bool loadNavigationProps)
        {
            try
            {
                IQueryable<OTP>? OTP = null;
                if (loadNavigationProps)
                    OTP = _dbContext.Set<OTP>().Where(predicate)
                        .Include(x => x.User)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();
                else
                    OTP = _dbContext.Set<OTP>().Where(predicate)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();

                return OTP;
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
