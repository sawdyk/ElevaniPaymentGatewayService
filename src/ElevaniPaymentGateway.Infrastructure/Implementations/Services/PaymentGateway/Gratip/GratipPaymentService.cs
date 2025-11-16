using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Models.Request.Gratip;
using ElevaniPaymentGateway.Core.Models.Request.TransactionService;
using ElevaniPaymentGateway.Core.Models.Response.TransactionService;
using ElevaniPaymentGateway.Infrastructure.Interfaces.EfRepository;
using ElevaniPaymentGateway.Infrastructure.Interfaces.ProxyClients.Gratip;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.PaymentGateway.Gratip;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Utilities;
using Microsoft.Extensions.Logging;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services.PaymentGateway.Gratip
{
    public class GratipPaymentService : IGratipPaymentService
    {
        private readonly ILogger<GratipPaymentService> _logger;
        private readonly IGratipCollectionService _gratipCollectionService;
        private readonly IGratipTransactionService _gratipTransactionService;
        private readonly ISqlTransactionService _sqlTransactionService;
        private readonly IBaseRepository<Transaction> _transactionRepository;
        private readonly ITransactionLoggerService _transactionLoggerService;
        public GratipPaymentService(ILogger<GratipPaymentService> logger,
           IGratipCollectionService gratipCollectionService, IGratipTransactionService gratipTransactionService,
           ISqlTransactionService sqlTransactionService, IBaseRepository<Transaction> transactionRepository, 
           ITransactionLoggerService transactionLoggerService)
        {
            _logger = logger;
            _gratipCollectionService = gratipCollectionService;
            _gratipTransactionService = gratipTransactionService;
            _sqlTransactionService = sqlTransactionService;
            _transactionRepository = transactionRepository;
            _transactionLoggerService = transactionLoggerService;
        }

        public async Task<TransactionResponse> InitiateTransactionAsync(string merchantId, string reference, TransactionRequest request)
        {
            try
            {
                InitiateTransactionRequest initiateTransactionRequest = new InitiateTransactionRequest();
                initiateTransactionRequest.amount = request.Amount;
                initiateTransactionRequest.currency = request.Currency;
                initiateTransactionRequest.method = "Card Pay"; // Payment method (Google Pay, Apple Pay, Card Pay)
                initiateTransactionRequest.countryCode = request.CountryCode;
                initiateTransactionRequest.external_reference = reference;
                initiateTransactionRequest.description = request.Description; //narration
                initiateTransactionRequest.customer_info = new Customer_Info
                {
                    firstName = request.CustomerFirstName,
                    lastName = request.CustomerLastName,
                    email = request.CustomerEmail,
                };

                var initiateTransactionResponse = await _gratipCollectionService.InitiateTransactionAsync(initiateTransactionRequest);
                if (initiateTransactionResponse is null) throw new GenericException(RespMsgConstants.TransactionInitiationError);
                if (!initiateTransactionResponse.success) throw new GenericException(RespMsgConstants.TransactionInitiationError);

                var sqlTransaction = await _sqlTransactionService.BeginTransactionAsync();

                var transaction = await _transactionLoggerService.LogTransactionAsync(merchantId, reference, PaymentGateways.GRATIP, request);
                await _gratipTransactionService.LogGratipTransactionsAsync(transaction.Id, initiateTransactionRequest, initiateTransactionResponse);

                await _sqlTransactionService.CommitAndDisposeTransactionAsync(sqlTransaction);

                _logger.LogInformation("logged transaction successfully");

                return new TransactionResponse
                {
                    MerchantID = merchantId,
                    TransactionReference = reference,
                    PaymentUrl = initiateTransactionResponse.data.payment_url,
                    Amount = initiateTransactionResponse.data.amount,
                    Currency = initiateTransactionResponse.data.currency,
                    CountryCode = initiateTransactionRequest.countryCode,
                    Status = initiateTransactionResponse.data.status,
                    CreatedAt = initiateTransactionResponse.data.created_at,
                };
            }
            catch (Exception ex)
            when (ex is NotFoundException || ex is GenericException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to initiate payment via Gratip payment gateway >> " +
                    $"{ex.Message} | stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }
    }
}
