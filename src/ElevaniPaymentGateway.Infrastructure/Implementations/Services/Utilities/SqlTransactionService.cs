using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Utilities;
using ElevaniPaymentGateway.Persistence;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services.Utilities
{
    public class SqlTransactionService : ISqlTransactionService
    {
        private readonly ILogger<SqlTransactionService> _logger;
        private readonly AppDbContext _dbContext;
        public SqlTransactionService(ILogger<SqlTransactionService> logger,
            AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            try
            {
                return _dbContext.Database.BeginTransaction();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to perfrom sql transaction {ex.Message}");
                throw;
            }
        }

        public async Task CommitAndDisposeTransactionAsync(IDbContextTransaction transaction)
        {
            try
            {
                await transaction.CommitAsync();
                await transaction.DisposeAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to perfrom sql transaction {ex.Message}");
            }
        }

        public async Task RollBackTransactionAsync(IDbContextTransaction transaction)
        {
            try
            {
                await transaction.RollbackAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to perfrom sql transaction {ex.Message}");
            }
        }
    }
}
