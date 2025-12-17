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
    public class UserRoleQuery : IUserRoleQuery
    {
        private readonly ILogger<UserRoleQuery> _logger;
        private readonly AppDbContext _dbContext;
        public UserRoleQuery(ILogger<UserRoleQuery> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<UserRole> GetByAsync(Expression<Func<UserRole, bool>> predicate, bool loadNavigationProps)
        {
            try
            {
                UserRole? UserRole = null;
                if (loadNavigationProps)
                    UserRole = await _dbContext.Set<UserRole>().Where(predicate)
                        .Include(x => x.User).Include(x => x.Role)
                        .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();
                else
                    UserRole = await _dbContext.Set<UserRole>().Where(predicate)
                      .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();

                return UserRole;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<IQueryable<UserRole>> ListAllAsync(bool loadNavigationProps)
        {
            try
            {
                IQueryable<UserRole>? UserRole = null;
                if (loadNavigationProps)
                    UserRole = _dbContext.Set<UserRole>()
                        .Include(x => x.User).Include(x => x.Role)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();
                else
                    UserRole = _dbContext.Set<UserRole>()
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();

                return UserRole;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<IQueryable<UserRole>> ListAsync(Expression<Func<UserRole, bool>> predicate, bool loadNavigationProps)
        {
            try
            {
                IQueryable<UserRole>? UserRole = null;
                if (loadNavigationProps)
                    UserRole = _dbContext.Set<UserRole>().Where(predicate)
                        .Include(x => x.User).Include(x => x.Role)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();
                else
                    UserRole = _dbContext.Set<UserRole>().Where(predicate)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();

                return UserRole;
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
