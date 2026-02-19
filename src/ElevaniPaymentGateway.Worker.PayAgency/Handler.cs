using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Core.Helpers;
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
        public Handler(ILogger<Handler> logger, IBaseRepository<Transaction> transactionRepository, 
            IBaseRepository<PayAgencyTransaction> payAgencyTransactionRepository, ITransactionQuery transactionQuery, 
            IPayAgencyTransactionQuery payAgencyTransactionQuery, IPayAgencyCollectionService payAgencyCollectionService,
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
                    && (x.Status == TransactionStatus.Init) || (x.Status == TransactionStatus.Pending) || (x.Status == TransactionStatus.Redirect))).ToListAsync();

                if (!pendingTransactions.Any())
                {
                    _logger.LogInformation($"No pending transaction for status verification as at {DateTime.Now}");
                    return;
                }

                foreach (var payAgencyTransaction in pendingTransactions)
                {
                    var transaction = await _transactionQuery.GetByAsync(x => x.Reference == payAgencyTransaction.Reference);
                    if (transaction is null)
                    {
                        _logger.LogInformation($"Transaction with reference {payAgencyTransaction.Reference} could not be found");
                        continue;
                    }
                    var transactionStatusResp = await _payAgencyCollectionService.TransactionStatusAsync(payAgencyTransaction.Reference);
                    if (transactionStatusResp is not null)
                    {
                        if (transactionStatusResp.data is not null)
                        {
                            _logger.LogInformation($"Transaction reference >>> {payAgencyTransaction.Reference} " +
                                $"| Pay agency reference >>> {payAgencyTransaction.TransactionReference} | Transaction status >>> {transactionStatusResp.status}");

                            //check transaction status
                            if (transactionStatusResp.status.ToLower().Equals("success"))
                            {
                                //Pay agency transaction table
                                payAgencyTransaction.IsVerified = true;
                                payAgencyTransaction.Message = transactionStatusResp.message;
                                payAgencyTransaction.Status = TransactionStatus.Completed;

                                //Main transaction table
                                transaction.Status = TransactionStatus.Completed;
                                transaction.Message = transactionStatusResp.message;
                            }
                            else
                            {
                                //Pay agency transaction table
                                payAgencyTransaction.IsVerified = false;
                                payAgencyTransaction.Message = transactionStatusResp.message;
                                payAgencyTransaction.Status = StringHelpers.FormatPayAgencyStatus(transactionStatusResp.status);

                                //Main transaction table
                                transaction.Message = transactionStatusResp.message;
                                transaction.Status = StringHelpers.FormatPayAgencyStatus(transactionStatusResp.status);
                            }

                            //Update the pay agency transaction table
                            payAgencyTransaction.UpdatedAt = DateTime.UtcNow;
                            payAgencyTransaction.DateVerified = DateTime.UtcNow;
                            payAgencyTransaction.UpdatedBy = "Job";

                            var sqlTransaction = await _sqlTransactionService.BeginTransactionAsync();

                            _payAgencyTransactionRepository.Update(payAgencyTransaction);
                            await _payAgencyTransactionRepository.SaveChangesAsync();

                            //Update the main transaction table
                            transaction.UpdatedAt = DateTime.UtcNow;
                            transaction.UpdatedBy = "Job";

                            _transactionRepository.Update(transaction);
                            await _transactionRepository.SaveChangesAsync();

                            await _sqlTransactionService.CommitAndDisposeTransactionAsync(sqlTransaction);
                        }
                        else
                            _logger.LogInformation($"Verification data >>> {JsonConvert.SerializeObject(transactionStatusResp.data)}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while executing pay agency transaction verification service - {ex.Message}");
                _logger.LogError($"stack trace >>> {ex.StackTrace} | inner exception >>> {ex.InnerException} | source >>> {ex.Source}");
            }
        }
    }
}
