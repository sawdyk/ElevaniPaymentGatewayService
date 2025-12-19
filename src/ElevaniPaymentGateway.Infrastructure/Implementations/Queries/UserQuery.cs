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
    public class UserQuery : IUserQuery
    {
        private readonly ILogger<UserQuery> _logger;
        private readonly AppDbContext _dbContext;
        public UserQuery(ILogger<UserQuery> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<User> GetByAsync(Expression<Func<User, bool>> predicate, bool loadNavigationProps = false)
        {
            try
            {
                User? User = null;
                if (loadNavigationProps)
                    User = await _dbContext.Set<User>().Where(predicate)
                        .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();
                else
                    User = await _dbContext.Set<User>().Where(predicate)
                      .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();

                return User;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<IQueryable<User>> ListAllAsync(bool loadNavigationProps = false)
        {
            try
            {
                IQueryable<User>? User = null;
                if (loadNavigationProps)
                    User = _dbContext.Set<User>()
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();
                else
                    User = _dbContext.Set<User>()
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();

                return User;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<IQueryable<User>> ListAsync(Expression<Func<User, bool>> predicate, bool loadNavigationProps = false)
        {
            try
            {
                IQueryable<User>? User = null;
                if (loadNavigationProps)
                    User = _dbContext.Set<User>().Where(predicate)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();
                else
                    User = _dbContext.Set<User>().Where(predicate)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();

                return User;
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
