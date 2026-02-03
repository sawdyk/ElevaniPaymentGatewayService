using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Core.Models.Request.Gratip;
using ElevaniPaymentGateway.Infrastructure.Interfaces.EfRepository;
using ElevaniPaymentGateway.Infrastructure.Interfaces.ProxyClients.Gratip;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Queries;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Utilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ElevaniPaymentGateway.Worker.Gratip
{
    public class Handler
    {
        private readonly ILogger<Handler> _logger;
        private readonly IBaseRepository<Transaction> _transactionRepository;
        private readonly IBaseRepository<GratipTransaction> _gratipTransactionRepository;
        private readonly ITransactionQuery _transactionQuery;
        private readonly IGratipTransactionQuery _gratipTransactionQuery;
        private readonly IGratipCollectionService _gratipCollectionService;
        private readonly ISqlTransactionService _sqlTransactionService;
        public Handler(ILogger<Handler> logger, IBaseRepository<Transaction> transactionRepository, IBaseRepository<GratipTransaction> gratipTransactionRepository,
            ITransactionQuery transactionQuery, IGratipTransactionQuery gratipTransactionQuery, IGratipCollectionService gratipCollectionService,
            ISqlTransactionService sqlTransactionService)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
            _gratipTransactionRepository = gratipTransactionRepository;
            _transactionQuery = transactionQuery;
            _gratipTransactionQuery = gratipTransactionQuery;
            _gratipCollectionService = gratipCollectionService;
            _sqlTransactionService = sqlTransactionService;
        }

        public async Task FinalizeAndVerifyTransactions() //transactions verification
        {
            try
            {
                var pendingGratipTransactions = await (await _gratipTransactionQuery
                    .ListAsync(x => x.IsVerified == false && x.Status == TransactionStatus.Pending)).ToListAsync();

                if (!pendingGratipTransactions.Any())
                {
                    _logger.LogInformation($"No pending transaction for finalization as at {DateTime.Now}");
                    return;
                }

                foreach (var gratipTransaction in pendingGratipTransactions)
                {
                    var transaction = await _transactionQuery.GetByAsync(x => x.Reference == gratipTransaction.ExternalReference);
                    if (transaction is null)
                    {
                        _logger.LogInformation($"Transaction with reference {gratipTransaction.ExternalReference} could not be found");
                        continue;
                    }
                    var finalizeTransactionResp = await _gratipCollectionService
                        .FinalizeTransactionAsync(new FinalizeTransactionRequest { transactionReference = gratipTransaction.TransactionReference });
                    if (finalizeTransactionResp is not null)
                    {
                        if (finalizeTransactionResp.data is not null)
                        {
                            _logger.LogInformation($"tranaction reference {gratipTransaction.TransactionReference} " +
                                $"| finalize data status => {finalizeTransactionResp.data.status}");

                            //check transaction status
                            if (finalizeTransactionResp.data.status.Equals("successful"))
                            {
                                gratipTransaction.IsVerified = true;
                                gratipTransaction.Status = TransactionStatus.Completed;

                                //Update the main transaction table
                                transaction.Status = TransactionStatus.Completed;
                            }
                            else
                            {
                                if (finalizeTransactionResp.data.status.Equals("failed") ||
                                    finalizeTransactionResp.data.status.Equals("cancelled") || finalizeTransactionResp.data.status.Equals("declined"))
                                {
                                    gratipTransaction.IsVerified = false;
                                    if (finalizeTransactionResp.data.status.Equals("cancelled"))
                                        gratipTransaction.Status = TransactionStatus.Cancelled;
                                    if (finalizeTransactionResp.data.status.Equals("declined"))
                                        gratipTransaction.Status = TransactionStatus.Declined;
                                    if (finalizeTransactionResp.data.status.Equals("failed"))
                                        gratipTransaction.Status = TransactionStatus.Failed;

                                    //Update the main transaction table
                                    if (finalizeTransactionResp.data.status.Equals("cancelled"))
                                        transaction.Status = TransactionStatus.Cancelled;
                                    if (finalizeTransactionResp.data.status.Equals("declined"))
                                        transaction.Status = TransactionStatus.Declined;
                                    if (finalizeTransactionResp.data.status.Equals("failed"))
                                        transaction.Status = TransactionStatus.Failed;
                                }
                            }

                            gratipTransaction.UpdatedAt = DateTime.UtcNow;
                            gratipTransaction.DateVerified = DateTime.UtcNow;
                            gratipTransaction.UpdatedBy = "Job";

                            var sqlTransaction = await _sqlTransactionService.BeginTransactionAsync();

                            _gratipTransactionRepository.Update(gratipTransaction);
                            await _gratipTransactionRepository.SaveChangesAsync();

                            transaction.UpdatedAt = DateTime.UtcNow;
                            transaction.UpdatedBy = "Job";

                            _transactionRepository.Update(transaction);
                            await _transactionRepository.SaveChangesAsync();

                            await _sqlTransactionService.CommitAndDisposeTransactionAsync(sqlTransaction);
                        }
                        else
                            _logger.LogInformation($"Verification data => {JsonConvert.SerializeObject(finalizeTransactionResp.data)}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while executing gratip transaction verification service - {ex.Message}");
                _logger.LogError($"stack trace >>> {ex.StackTrace} | innver exception >>> {ex.InnerException} | source >>> {ex.Source}");
            }
        }
    }
}
