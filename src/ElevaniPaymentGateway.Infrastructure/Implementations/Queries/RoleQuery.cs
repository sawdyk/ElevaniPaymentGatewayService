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
    public class RoleQuery : IRoleQuery
    {
        private readonly ILogger<RoleQuery> _logger;
        private readonly AppDbContext _dbContext;
        public RoleQuery(ILogger<RoleQuery> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<Role> GetByAsync(Expression<Func<Role, bool>> predicate, bool loadNavigationProps)
        {
            try
            {
                Role? Role = null;
                if (loadNavigationProps)
                    Role = await _dbContext.Set<Role>().Where(predicate)
                        .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();
                else
                    Role = await _dbContext.Set<Role>().Where(predicate)
                      .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();

                return Role;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<IQueryable<Role>> ListAllAsync(bool loadNavigationProps)
        {
            try
            {
                IQueryable<Role>? Role = null;
                if (loadNavigationProps)
                    Role = _dbContext.Set<Role>()
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();
                else
                    Role = _dbContext.Set<Role>()
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();

                return Role;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                  $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<IQueryable<Role>> ListAsync(Expression<Func<Role, bool>> predicate, bool loadNavigationProps)
        {
            try
            {
                IQueryable<Role>? Role = null;
                if (loadNavigationProps)
                    Role = _dbContext.Set<Role>().Where(predicate)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();
                else
                    Role = _dbContext.Set<Role>().Where(predicate)
                        .OrderByDescending(x => x.CreatedAt).AsNoTracking().AsSplitQuery();

                return Role;
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
