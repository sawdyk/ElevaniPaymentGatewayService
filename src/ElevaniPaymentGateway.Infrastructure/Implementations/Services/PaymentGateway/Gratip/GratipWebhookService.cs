using ElevaniPaymentGateway.Core.Configs;
using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Models.Response.Gratip;
using ElevaniPaymentGateway.Infrastructure.Interfaces.EfRepository;
using ElevaniPaymentGateway.Infrastructure.Interfaces.ProxyClients.Gratip;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Queries;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.PaymentGateway.Gratip;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services.PaymentGateway.Gratip
{
    public class GratipWebhookService : IGratipWebhookService
    {
        private readonly ILogger<GratipWebhookService> _logger;
        private readonly GratipConfig _gratipConfig;
        private readonly IBaseRepository<Transaction> _transactionRepository;
        private readonly IBaseRepository<GratipTransaction> _gratipTransactionRepository;
        private readonly ITransactionQuery _transactionQuery;
        private readonly IGratipTransactionQuery _gratipTransactionQuery;
        private readonly ISqlTransactionService _sqlTransactionService;
        public GratipWebhookService(ILogger<GratipWebhookService> logger, IOptions<GratipConfig> gratipConfig,
            IBaseRepository<Transaction> transactionRepository, IBaseRepository<GratipTransaction> gratipTransactionRepository,
            ITransactionQuery transactionQuery, IGratipTransactionQuery gratipTransactionQuery, ISqlTransactionService sqlTransactionService)
        {
            _logger = logger;
            _gratipConfig = gratipConfig.Value;
            _transactionRepository = transactionRepository;
            _gratipTransactionRepository = gratipTransactionRepository;
            _transactionQuery = transactionQuery;
            _gratipTransactionQuery = gratipTransactionQuery;
            _sqlTransactionService = sqlTransactionService;
        }

        public async Task<bool> VerifyWebhookSignature(string payload, string signature)
        {
            try
            {
                using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_gratipConfig.WebhookSecret));
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                var expectedSignature = Convert.ToHexString(hash).ToLower();

                var signatureBytes = Encoding.UTF8.GetBytes(signature);
                var expectedBytes = Encoding.UTF8.GetBytes(expectedSignature);

                if (signatureBytes.Length != expectedBytes.Length)
                    return false;

                // Timing-safe comparison (equivalent to crypto.timingSafeEqual)
                return CryptographicOperations.FixedTimeEquals(signatureBytes, expectedBytes);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to verify Gratip webhook signature >> " +
                    $"{ex.Message} | stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task HandleWebhookNotification(string payload)
        {
            try
            {
                //deserialize payload here
                var webhookData = JsonConvert.DeserializeObject<GratipWebhookResponse>(payload);
                var deserializedData = JsonConvert.SerializeObject(webhookData);
                switch (webhookData?.@event)
                {
                    case "COLLECTION_COMPLETED":
                        // Handle successful collection
                        await HandleCollectionCompleted(webhookData.data);
                        break;

                    case "FEE_CREATED":
                        // Handle fee allocation (pending settlement)
                        _logger.LogInformation($"FEE_CREATED >>> {deserializedData}");
                        break;

                    case "PAYOUT_CREATED":
                        // Handle payout creation (pending settlement)
                        _logger.LogInformation($"PAYOUT_CREATED >>> {deserializedData}");
                        break;

                    case "BULK_FEE_SETTLEMENT_UPDATES":
                        // Handle bulk fee settlements (array of fees)
                        _logger.LogInformation($"BULK_FEE_SETTLEMENT_UPDATES >>> {deserializedData}");
                        break;

                    case "BULK_PAYOUT_UPDATES":
                        // Handle bulk payout updates (array of payouts)
                        // Note: All recipient types receive bulk webhooks for settlement operations
                        // Even single item settlements are sent as bulk webhooks with one item
                        _logger.LogInformation($"BULK_PAYOUT_UPDATES >>> {deserializedData}");
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to handle Gratip webhook notification >> " +
                    $"{ex.Message} | stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task HandleCollectionCompleted(GratipWebhookData finalizeTransactionResp)
        {
            try
            {
                if (finalizeTransactionResp is null)
                    throw new GenericException($"An error occurred");

                var gratipTransaction = await _gratipTransactionQuery.GetByAsync(x => x.TransactionReference == finalizeTransactionResp.transaction_reference);
                if (gratipTransaction is null)
                {
                    _logger.LogInformation($"Transaction with reference {finalizeTransactionResp.transaction_reference} could not be found");
                    throw new GenericException($"An error occurred. Transaction {finalizeTransactionResp.transaction_reference} could not be found");
                }
                var transaction = await _transactionQuery.GetByAsync(x => x.Reference == gratipTransaction.ExternalReference);
                if (transaction is null)
                {
                    _logger.LogInformation($"Transaction with reference {finalizeTransactionResp.transaction_reference} could not be found");
                    throw new GenericException($"An error occurred. Transaction {finalizeTransactionResp.transaction_reference} could not be found");
                }
               
                //check transaction status
                if (finalizeTransactionResp.status.Equals("successful"))
                {
                    gratipTransaction.IsVerified = true;
                    gratipTransaction.Status = TransactionStatus.Completed;

                    //Update the main transaction table
                    transaction.Status = TransactionStatus.Completed;
                }
                else
                {
                    if (finalizeTransactionResp.status.Equals("failed") || finalizeTransactionResp.status.Equals("declined") ||
                        finalizeTransactionResp.status.Equals("cancelled") || finalizeTransactionResp.status.Equals("expired"))
                    {
                        gratipTransaction.IsVerified = false;
                        if (finalizeTransactionResp.status.Equals("cancelled"))
                            gratipTransaction.Status = TransactionStatus.Cancelled;
                        if (finalizeTransactionResp.status.Equals("declined"))
                            gratipTransaction.Status = TransactionStatus.Declined;
                        if (finalizeTransactionResp.status.Equals("failed"))
                            gratipTransaction.Status = TransactionStatus.Failed;
                        if (finalizeTransactionResp.status.Equals("expired"))
                            gratipTransaction.Status = TransactionStatus.Failed;

                        //Update the main transaction table
                        if (finalizeTransactionResp.status.Equals("cancelled"))
                            transaction.Status = TransactionStatus.Cancelled;
                        if (finalizeTransactionResp.status.Equals("declined"))
                            transaction.Status = TransactionStatus.Declined;
                        if (finalizeTransactionResp.status.Equals("failed"))
                            transaction.Status = TransactionStatus.Failed;
                        if (finalizeTransactionResp.status.Equals("expired"))
                            transaction.Status = TransactionStatus.Failed;
                    }
                }

                var sqlTransaction = await _sqlTransactionService.BeginTransactionAsync();

                gratipTransaction.CustomerFirstName = string.IsNullOrEmpty(finalizeTransactionResp.billing_name) ? "" : finalizeTransactionResp.billing_name;
                gratipTransaction.CustomerLastName = string.IsNullOrEmpty(finalizeTransactionResp.billing_name) ? "" : finalizeTransactionResp.billing_name;
                gratipTransaction.UpdatedAt = DateTime.UtcNow;
                gratipTransaction.DateVerified = DateTime.UtcNow;
                gratipTransaction.UpdatedBy = "Job";

                _gratipTransactionRepository.Update(gratipTransaction);
                await _gratipTransactionRepository.SaveChangesAsync();

                transaction.FirstName = string.IsNullOrEmpty(finalizeTransactionResp.billing_name) ? "" : finalizeTransactionResp.billing_name;
                transaction.LastName = string.IsNullOrEmpty(finalizeTransactionResp.billing_name) ? "" : finalizeTransactionResp.billing_name;
                transaction.CardNumber = string.IsNullOrEmpty(finalizeTransactionResp.masked_pan) ? "" : finalizeTransactionResp.masked_pan;
                transaction.UpdatedAt = DateTime.UtcNow;
                transaction.UpdatedBy = "Job";

                _transactionRepository.Update(transaction);
                await _transactionRepository.SaveChangesAsync();

                await _sqlTransactionService.CommitAndDisposeTransactionAsync(sqlTransaction);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to handle collection completed event >> " +
                    $"{ex.Message} | stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }
    }
}
