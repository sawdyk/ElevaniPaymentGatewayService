using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Core.Models.Request.Gratip;
using ElevaniPaymentGateway.Core.Models.Request.TransactionService;
using ElevaniPaymentGateway.Core.Models.Response.TransactionService;
using ElevaniPaymentGateway.Infrastructure.Helpers;
using ElevaniPaymentGateway.Infrastructure.Interfaces.ProxyClients.Gratip;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Queries;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Helpers;
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
        private readonly ITransactionLoggerService _transactionLoggerService;
        private MerchantContextDto _merchantContext;
        private readonly IPaymentHttpContextHelperService _paymentHttpContextHelperService;
        private readonly ITransactionQuery _transactionQuery;
        private readonly ValidationHelper _validationHelper;
        public GratipPaymentService(ILogger<GratipPaymentService> logger,
           IGratipCollectionService gratipCollectionService, IGratipTransactionService gratipTransactionService,
           ISqlTransactionService sqlTransactionService, ITransactionLoggerService transactionLoggerService, 
           IPaymentHttpContextHelperService paymentHttpContextHelperService, ITransactionQuery transactionQuery, 
           ValidationHelper validationHelper)
        {
            _logger = logger;
            _gratipCollectionService = gratipCollectionService;
            _gratipTransactionService = gratipTransactionService;
            _sqlTransactionService = sqlTransactionService;
            _transactionLoggerService = transactionLoggerService;
            _paymentHttpContextHelperService = paymentHttpContextHelperService;
            _merchantContext = _paymentHttpContextHelperService.MerchantContext();
            _transactionQuery = transactionQuery;
            _validationHelper = validationHelper;
        }

        public async Task<TransactionResponse> InitiateTransactionAsync(TransactionRequest request)
        {
            try
            {
                if (request is not null)
                    _validationHelper.ValidateRequest(request);

                var merhantSlug = request.Reference.Substring(0, 3);
                if (!_merchantContext.Slug.Equals(merhantSlug)) throw new GenericException("Invalid reference format");

                var existingReference = await _transactionQuery.GetByAsync(x => x.Reference == request.Reference);
                if (existingReference is not null) throw new GenericException("Duplicate reference exist");

                InitiateTransactionRequest initiateTransactionRequest = new InitiateTransactionRequest();
                initiateTransactionRequest.amount = request.Amount;
                initiateTransactionRequest.currency = request.Currency.ToUpper();
                initiateTransactionRequest.method = "Card Pay"; // Payment method (Google Pay, Apple Pay, Card Pay)
                initiateTransactionRequest.countryCode = request.CountryCode.ToUpper();
                initiateTransactionRequest.external_reference = request.Reference;
                initiateTransactionRequest.description = request.Description; //narration
                
                var initiateTransactionResponse = await _gratipCollectionService.InitiateTransactionAsync(initiateTransactionRequest);
                if (initiateTransactionResponse is null) throw new GenericException(RespMsgConstants.TransactionInitiationError);
                if (!initiateTransactionResponse.success) throw new GenericException(RespMsgConstants.TransactionInitiationError);

                var sqlTransaction = await _sqlTransactionService.BeginTransactionAsync();

                var transaction = await _transactionLoggerService.LogGratipTransactionAsync(request);
                await _gratipTransactionService.LogGratipTransactionsAsync(transaction.Id, initiateTransactionRequest, initiateTransactionResponse);

                await _sqlTransactionService.CommitAndDisposeTransactionAsync(sqlTransaction);

                _logger.LogInformation("logged transaction successfully");

                return new TransactionResponse
                {
                    MerchantID = _merchantContext.MerchantId,
                    TransactionReference = request.Reference,
                    PaymentUrl = initiateTransactionResponse.data.payment_url,
                    Amount = initiateTransactionResponse.data.amount,
                    Currency = initiateTransactionResponse.data.currency,
                    CountryCode = initiateTransactionRequest.countryCode,
                    Description = initiateTransactionRequest.description,
                    Status = initiateTransactionResponse.data.status,
                    CreatedAt = initiateTransactionResponse.data.created_at,
                };
            }
            catch (Exception ex)
            when (ex is NotFoundException || ex is GenericException
            || ex is DataValidationException)
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
