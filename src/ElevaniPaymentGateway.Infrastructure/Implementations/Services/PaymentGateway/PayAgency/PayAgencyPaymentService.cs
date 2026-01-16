using AutoMapper;
using ElevaniPaymentGateway.Core.Configs;
using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Core.Models.Request.PayAgency;
using ElevaniPaymentGateway.Core.Models.Request.TransactionService;
using ElevaniPaymentGateway.Core.Models.Response.TransactionService;
using ElevaniPaymentGateway.Infrastructure.Helpers;
using ElevaniPaymentGateway.Infrastructure.Interfaces.ProxyClients.PayAgency;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Queries;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Helpers;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.PaymentGateway.PayAgency;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services.PaymentGateway.PayAgency
{
    public class PayAgencyPaymentService : IPayAgencyPaymentService
    {
        private readonly ILogger<PayAgencyPaymentService> _logger;
        private readonly ISqlTransactionService _sqlTransactionService;
        private readonly ITransactionLoggerService _transactionLoggerService;
        private readonly PayAgencyConfig _payAgencyConfig;
        private readonly ITransactionQuery _transactionQuery;
        private MerchantContextDto _merchantContext;
        private readonly IPaymentHttpContextHelperService _paymentHttpContextHelperService;
        private readonly IPayAgencyCollectionService _payAgencyCollectionService;
        private readonly IMapper _mapper;
        private readonly IPayAgencyTransactionService _payAgencyTransactionService;
        private readonly ValidationHelper _validationHelper;
        public PayAgencyPaymentService(ILogger<PayAgencyPaymentService> logger, ISqlTransactionService sqlTransactionService, 
            ITransactionLoggerService transactionLoggerService, IOptions<PayAgencyConfig> payAgencyConfig, ITransactionQuery transactionQuery,
            IPaymentHttpContextHelperService paymentHttpContextHelperService, IPayAgencyCollectionService payAgencyCollectionService, IMapper mapper, 
            IPayAgencyTransactionService payAgencyTransactionService, ValidationHelper validationHelper)
        {
            _logger = logger;
            _sqlTransactionService = sqlTransactionService;
            _transactionLoggerService = transactionLoggerService;
            _payAgencyConfig = payAgencyConfig.Value;
            _transactionQuery = transactionQuery;
            _paymentHttpContextHelperService = paymentHttpContextHelperService;
            _merchantContext = _paymentHttpContextHelperService.MerchantContext();
            _payAgencyCollectionService = payAgencyCollectionService;
            _mapper = mapper;
            _payAgencyTransactionService = payAgencyTransactionService;
            _validationHelper = validationHelper;
        }

        public async Task<PATransactionResponse> InitiateTransactionAsync(string encryptedRequest)
        {
            try
            {
                _logger.LogInformation($"merchant encrypted transaction request >>> {encryptedRequest}");
                string merchantDecryptedRequest = PayAgencyEncryptionService.DecryptData(encryptedRequest, _payAgencyConfig.MerchantEncryptionKey);
                _logger.LogInformation($"merchant decrypted transaction request >>> {merchantDecryptedRequest}"); //remove this later

                var merchantRequest = JsonConvert.DeserializeObject<PATransactionRequest>(merchantDecryptedRequest);

                if (merchantRequest is not null)
                    _validationHelper.ValidateRequest(merchantRequest);

                var merhantSlug = merchantRequest.Reference.Substring(0, 3);
                if (!_merchantContext.Slug.Equals(merhantSlug)) throw new GenericException("Invalid reference format");

                var existingReference = await _transactionQuery.GetByAsync(x => x.Reference == merchantRequest.Reference);
                if (existingReference is not null) throw new GenericException("Duplicate reference exist");

                var payAgencytTransRequest = _mapper.Map<PayAgencyTransactionRequest>(merchantRequest);
                payAgencytTransRequest.ip_address = _payAgencyConfig.IPAddress;
                
                var payAgencyRequest = JsonConvert.SerializeObject(payAgencytTransRequest);
                string payAgencyEncryptedRequest = PayAgencyEncryptionService.EncryptData(payAgencyRequest, _payAgencyConfig.EncryptionKey);
                var payAgencyEncryptedJsonRequest = new PayAgencyEncryptedRequest { payload = payAgencyEncryptedRequest };

                var initiateTransactionResponse = await _payAgencyCollectionService.InitiateTransactionAsync(payAgencyEncryptedJsonRequest);
                if (initiateTransactionResponse is null) throw new GenericException(RespMsgConstants.TransactionInitiationError);
                if (initiateTransactionResponse.errors != null && initiateTransactionResponse.errors.Any())
                    return new PATransactionResponse { Errors = initiateTransactionResponse.errors };
                if (initiateTransactionResponse.data is null)
                    return new PATransactionResponse { Message = initiateTransactionResponse.message };

                var sqlTransaction = await _sqlTransactionService.BeginTransactionAsync();

                var transaction = await _transactionLoggerService.LogPayAgencyTransactionAsync(merchantRequest, initiateTransactionResponse);
                await _payAgencyTransactionService.LogPayAgencyTransactionsAsync(transaction.Id, merchantRequest, initiateTransactionResponse);

                await _sqlTransactionService.CommitAndDisposeTransactionAsync(sqlTransaction);

                _logger.LogInformation("logged transaction successfully");

                return new PATransactionResponse
                {
                    MerchantID = _merchantContext.MerchantId,
                    Status = initiateTransactionResponse.status,
                    Message = initiateTransactionResponse.message,
                    TransactionReference = initiateTransactionResponse.data.order_id,
                    TransactionId = initiateTransactionResponse.data.transaction_id,
                    RedirectUrl = initiateTransactionResponse.redirect_url, //for redirecting users for OTP for 3DS card transactions
                    Amount = initiateTransactionResponse.data.amount,
                    Currency = initiateTransactionResponse.data.currency,
                    Customer = new PACustomerDetails
                    {
                        FirstName = initiateTransactionResponse.data.customer.first_name,
                        LastName = initiateTransactionResponse.data.customer.last_name,
                        Email = initiateTransactionResponse.data.customer.email
                    }
                };
            }
            catch (Exception ex)
            when (ex is NotFoundException || ex is GenericException
            || ex is DataValidationException || ex is ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to initiate payment via pay agency server to server service >> " +
                    $"{ex.Message} | stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }
    }
}
