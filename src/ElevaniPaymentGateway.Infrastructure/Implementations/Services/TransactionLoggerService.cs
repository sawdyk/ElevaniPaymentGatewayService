using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Models.Request.TransactionService;
using ElevaniPaymentGateway.Infrastructure.Interfaces.EfRepository;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services
{
    public class TransactionLoggerService : ITransactionLoggerService
    {
        private readonly ILogger<TransactionService> _logger;
        private readonly IBaseRepository<Transaction> _transactionRepository;
        public TransactionLoggerService(ILogger<TransactionService> logger, IBaseRepository<Transaction> transactionRepository)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
        }

        public async Task<Transaction> LogTransactionAsync(string merchantId, string reference, PaymentGateways paymentGateway, TransactionRequest request)
        {
            try
            {
                var transaction = new Transaction();
                transaction.MerchantId = merchantId;
                transaction.Reference = reference;
                transaction.Currency = request.Currency;
                transaction.Amount = request.Amount;
                transaction.CountryCode = request.CountryCode;
                transaction.Narration = request.Description;
                transaction.CustomerFirstName = request.CustomerFirstName;
                transaction.CustomerLastName = request.CustomerLastName;
                transaction.CustomerEmail = request.CustomerEmail;
                transaction.CustomerPhoneNumber = request.CustomerPhoneNumber;
                transaction.PaymentGateway = paymentGateway;
                transaction.CreatedBy = "System";

                _transactionRepository.Add(transaction);
                await _transactionRepository.SaveChangesAsync();

                return transaction;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to log transactions >> " +
                    $"{ex.Message} | stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

    }
}
