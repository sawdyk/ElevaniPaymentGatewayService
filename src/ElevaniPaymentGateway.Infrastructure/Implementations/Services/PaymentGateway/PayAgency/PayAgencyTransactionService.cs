using ElevaniPaymentGateway.Core.Configs;
using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Helpers;
using ElevaniPaymentGateway.Core.Models.Request.TransactionService;
using ElevaniPaymentGateway.Core.Models.Response.PayAgency;
using ElevaniPaymentGateway.Infrastructure.Interfaces.EfRepository;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.PaymentGateway.PayAgency;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services.PaymentGateway.PayAgency
{
    public class PayAgencyTransactionService : IPayAgencyTransactionService
    {
        private readonly ILogger<PayAgencyTransactionService> _logger;
        private readonly IBaseRepository<PayAgencyTransaction> _payAgencyTransactionRepository;
        private readonly PayAgencyConfig _payAgencyConfig;
        public PayAgencyTransactionService(ILogger<PayAgencyTransactionService> logger, 
            IBaseRepository<PayAgencyTransaction> payAgencyTransactionRepository, IOptions<PayAgencyConfig> payAgencyConfig)
        {
            _logger = logger;
            _payAgencyTransactionRepository = payAgencyTransactionRepository;
            _payAgencyConfig = payAgencyConfig.Value;
        }

        public async Task LogPayAgencyTransactionsAsync(Guid transactionId, PATransactionRequest request, PayAgencyTransactionResponse response)
        {
            try
            {
                var payAgencyTransaction = new PayAgencyTransaction();

                payAgencyTransaction.TransactionId = transactionId;
                payAgencyTransaction.Reference = request.Reference;
                payAgencyTransaction.TransactionReference = response.data.transaction_id;
                payAgencyTransaction.Currency = request.Currency;
                payAgencyTransaction.Amount = request.Amount;
                payAgencyTransaction.Country = request.Country;
                payAgencyTransaction.Description = request.Description;
                payAgencyTransaction.FirstName = request.FirstName;
                payAgencyTransaction.LastName = request.LastName;
                payAgencyTransaction.Email = request.Email;
                payAgencyTransaction.PhoneNumber = request.PhoneNumber;
                payAgencyTransaction.Address = request.Address;
                payAgencyTransaction.City = request.City;
                payAgencyTransaction.State = request.State;
                payAgencyTransaction.Zip = request.Zip;
                payAgencyTransaction.IPAddress = _payAgencyConfig.IPAddress; //use the servers IP address
                payAgencyTransaction.CardNumber = $"{request.CardNumber.Substring(0, 6)}**********";
                payAgencyTransaction.CardExpiryMonth = "**"; //request.CardExpiryMonth;
                payAgencyTransaction.CardExpiryYear = "****"; //request.CardExpiryYear;
                payAgencyTransaction.CardCVV = "***"; //request.CardCVV
                payAgencyTransaction.RedirectUrl = _payAgencyConfig.RedirectUrl;
                payAgencyTransaction.OTPRedirectUrl = response is null ? "" : response.redirect_url;
                payAgencyTransaction.Message = response is null ? "" : response.message;
                payAgencyTransaction.WebHookUrl = "";
                payAgencyTransaction.Status = StringHelpers.FormatPayAgencyStatus(response.status);
                if (payAgencyTransaction.Status == TransactionStatus.Completed)
                {
                    payAgencyTransaction.IsVerified = true;
                    payAgencyTransaction.DateVerified = DateTime.Now;
                }
               
                _payAgencyTransactionRepository.Add(payAgencyTransaction);
                await _payAgencyTransactionRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to log pay agency transactions >> " +
                    $"{ex.Message} | stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }
    }
}
