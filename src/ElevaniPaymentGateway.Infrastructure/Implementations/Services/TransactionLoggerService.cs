using ElevaniPaymentGateway.Core.Configs;
using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Helpers;
using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Core.Models.Request.TransactionService;
using ElevaniPaymentGateway.Core.Models.Response.PayAgency;
using ElevaniPaymentGateway.Infrastructure.Interfaces.EfRepository;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services
{
    public class TransactionLoggerService : ITransactionLoggerService
    {
        private readonly ILogger<TransactionService> _logger;
        private readonly IBaseRepository<Transaction> _transactionRepository;
        private MerchantContextDto _merchantContext;
        private readonly IPaymentHttpContextHelperService _paymentHttpContextHelperService;
        private readonly PayAgencyConfig _payAgencyConfig;
        public TransactionLoggerService(ILogger<TransactionService> logger, IBaseRepository<Transaction> transactionRepository,
            IPaymentHttpContextHelperService paymentHttpContextHelperService, IOptions<PayAgencyConfig> payAgencyConfig)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
            _paymentHttpContextHelperService = paymentHttpContextHelperService;
            _merchantContext = _paymentHttpContextHelperService.MerchantContext();
            _payAgencyConfig = payAgencyConfig.Value;
        }

        public async Task<Transaction> LogPayAgencyTransactionAsync(PATransactionRequest request, PayAgencyTransactionResponse response)
        {
            try
            {
                var transaction = new Transaction();
                transaction.MerchantId = _merchantContext.MerchantId;
                transaction.Reference = request.Reference;
                transaction.Currency = request.Currency;
                transaction.Amount = request.Amount;
                transaction.CountryCode = request.Country;
                transaction.Narration = request.Description;
                transaction.FirstName = request.FirstName;
                transaction.LastName = request.LastName;
                transaction.Email = request.Email;
                transaction.PhoneNumber = request.PhoneNumber;
                transaction.Message = response.message;
                transaction.City = request.City;
                transaction.State = request.State;
                transaction.Zip = request.Zip;
                transaction.IPAddress = _payAgencyConfig.IPAddress;
                transaction.CardNumber = $"{request.CardNumber.Substring(0, 6)}**********";
                transaction.CardExpiryMonth = "**"; //request.CardExpiryMonth;
                transaction.CardExpiryYear = "****"; //request.CardExpiryYear;
                transaction.CardCVV = "***"; //request.CardCVV
                transaction.RedirectUrl = _payAgencyConfig.RedirectUrl;
                transaction.WebHookUrl = "";
                transaction.Status = StringHelpers.FormatPayAgencyStatus(response.status); 
                transaction.PaymentGateway = PaymentGateways.PAYAGENCY;
                transaction.CreatedBy = "System";

                _transactionRepository.Add(transaction);
                await _transactionRepository.SaveChangesAsync();

                return transaction;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to log pay agency transactions >> " +
                    $"{ex.Message} | stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<Transaction> LogGratipTransactionAsync(TransactionRequest request)
        {
            try
            {
                var transaction = new Transaction();
                transaction.MerchantId = _merchantContext.MerchantId;
                transaction.Reference = request.Reference;
                transaction.Currency = request.Currency;
                transaction.Amount = request.Amount;
                transaction.CountryCode = request.CountryCode;
                transaction.Narration = request.Description;
                transaction.FirstName = "";
                transaction.LastName = "";
                transaction.Email = "";
                transaction.PhoneNumber = "";
                transaction.PaymentGateway = PaymentGateways.GRATIP;
                transaction.Status = TransactionStatus.Pending;
                transaction.CreatedBy = "System";

                _transactionRepository.Add(transaction);
                await _transactionRepository.SaveChangesAsync();

                return transaction;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to log gratip transactions >> " +
                    $"{ex.Message} | stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }
    }
}
