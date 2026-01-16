using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Core.Models.Request.Gratip;
using ElevaniPaymentGateway.Infrastructure.Interfaces.EfRepository;
using ElevaniPaymentGateway.Infrastructure.Interfaces.ProxyClients.PayAgency;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Queries;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Utilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ElevaniPaymentGateway.Worker.PayAgency
{
    public class Handler
    {
        private readonly ILogger<Handler> _logger;
        private readonly IBaseRepository<Transaction> _transactionRepository;
        private readonly IBaseRepository<PayAgencyTransaction> _payAgencyTransactionRepository;
        private readonly ITransactionQuery _transactionQuery;
        private readonly IPayAgencyTransactionQuery _payAgencyTransactionQuery;
        private readonly IPayAgencyCollectionService _payAgencyCollectionService;
        private readonly ISqlTransactionService _sqlTransactionService;
        public Handler(ILogger<Handler> logger, IBaseRepository<Transaction> transactionRepository, IBaseRepository<PayAgencyTransaction> payAgencyTransactionRepository,
            ITransactionQuery transactionQuery, IPayAgencyTransactionQuery payAgencyTransactionQuery, IPayAgencyCollectionService payAgencyCollectionService,
            ISqlTransactionService sqlTransactionService)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
            _payAgencyTransactionRepository = payAgencyTransactionRepository;
            _transactionQuery = transactionQuery;
            _payAgencyTransactionQuery = payAgencyTransactionQuery;
            _payAgencyCollectionService = payAgencyCollectionService;
            _sqlTransactionService = sqlTransactionService;
        }

        public async Task VerifyTransaction()
        {
            try
            {
                var pendingTransactions = await (await _payAgencyTransactionQuery
                    .ListAsync(x => x.IsVerified == false 
                    && (x.Status == TransactionStatus.Init) || (x.Status == TransactionStatus.Pending) || (x.Status == TransactionStatus.Redirect)))
                    .ToListAsync();

                if (!pendingTransactions.Any())
                {
                    _logger.LogInformation($"No pending transaction for status verification as at {DateTime.Now}");
                    return;
                }

                //foreach (var payAgencyTransaction in pendingTransactions)
                //{
                //    var transaction = await _transactionQuery.GetByAsync(x => x.Reference == gratipTransaction.ExternalReference);
                //    if (transaction is null)
                //    {
                //        _logger.LogInformation($"Transaction with reference {gratipTransaction.ExternalReference} could not be found");
                //        continue;
                //    }
                //    var finalizeTransactionResp = await _gratipCollectionService
                //        .FinalizeTransactionAsync(new FinalizeTransactionRequest { transactionReference = gratipTransaction.TransactionReference });
                //    if (finalizeTransactionResp is not null)
                //    {
                //        if (finalizeTransactionResp.data is not null)
                //        {
                //            _logger.LogInformation($"tranaction reference {gratipTransaction.TransactionReference} " +
                //                $"| finalize data status => {finalizeTransactionResp.data.status}");

                //            //check transaction status
                //            if (finalizeTransactionResp.data.status.Equals("successful"))
                //            {
                //                gratipTransaction.IsVerified = true;
                //                gratipTransaction.Status = TransactionStatus.Completed;

                //                //Update the main transaction table
                //                transaction.Status = TransactionStatus.Completed;
                //            }
                //            else
                //            {
                //                if (finalizeTransactionResp.data.status.Equals("failed") ||
                //                    finalizeTransactionResp.data.status.Equals("cancelled") || finalizeTransactionResp.data.status.Equals("declined"))
                //                {
                //                    gratipTransaction.IsVerified = false;
                //                    if (finalizeTransactionResp.data.status.Equals("cancelled"))
                //                        gratipTransaction.Status = TransactionStatus.Cancelled;
                //                    if (finalizeTransactionResp.data.status.Equals("declined"))
                //                        gratipTransaction.Status = TransactionStatus.Declined;
                //                    if (finalizeTransactionResp.data.status.Equals("failed"))
                //                        gratipTransaction.Status = TransactionStatus.Failed;

                //                    //Update the main transaction table
                //                    if (finalizeTransactionResp.data.status.Equals("cancelled"))
                //                        transaction.Status = TransactionStatus.Cancelled;
                //                    if (finalizeTransactionResp.data.status.Equals("declined"))
                //                        transaction.Status = TransactionStatus.Declined;
                //                    if (finalizeTransactionResp.data.status.Equals("failed"))
                //                        transaction.Status = TransactionStatus.Failed;
                //                }
                //            }

                //            gratipTransaction.UpdatedAt = DateTime.UtcNow;
                //            gratipTransaction.DateVerified = DateTime.UtcNow;
                //            gratipTransaction.UpdatedBy = "Job";

                //            var sqlTransaction = await _sqlTransactionService.BeginTransactionAsync();

                //            _gratipTransactionRepository.Update(gratipTransaction);
                //            await _gratipTransactionRepository.SaveChangesAsync();

                //            transaction.UpdatedAt = DateTime.UtcNow;
                //            transaction.UpdatedBy = "Job";

                //            _transactionRepository.Update(transaction);
                //            await _transactionRepository.SaveChangesAsync();

                //            await _sqlTransactionService.CommitAndDisposeTransactionAsync(sqlTransaction);
                //        }
                //        else
                //            _logger.LogInformation($"Verification data => {JsonConvert.SerializeObject(finalizeTransactionResp.data)}");
                //    }
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while executing pay agency transaction verification service - {ex.Message}");
                _logger.LogError($"stack trace >>> {ex.StackTrace} | inner exception >>> {ex.InnerException} | source >>> {ex.Source}");
            }
        }
    }
}
