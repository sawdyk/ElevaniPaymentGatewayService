using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Models.Request.Gratip;
using ElevaniPaymentGateway.Core.Models.Response.Gratip;
using ElevaniPaymentGateway.Infrastructure.Interfaces.EfRepository;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.PaymentGateway.Gratip;
using Microsoft.Extensions.Logging;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services.PaymentGateway.Gratip
{
    public class GratipTransactionService : IGratipTransactionService
    {
        private readonly ILogger<GratipTransactionService> _logger;
        private readonly IBaseRepository<GratipTransaction> _gratipTransactionRepository;
        public GratipTransactionService(ILogger<GratipTransactionService> logger, IBaseRepository<GratipTransaction> gratipTransactionRepository)
        {
            _logger = logger;
            _gratipTransactionRepository = gratipTransactionRepository;
        }

        public async Task LogGratipTransactionsAsync(Guid transactionId, InitiateTransactionRequest request, InitiateTransctionResponse response)
        {
            try
            {
                _gratipTransactionRepository.Add(new GratipTransaction
                {
                    TransactionId = transactionId,
                    ExternalReference = request.external_reference,
                    Currency = request.currency,
                    Method = request.method,
                    Amount = request.amount,
                    CountryCode = request.countryCode,
                    //MerchantID = request.merchant_id,
                    Description = request.description,
                    PaymentURL = response.data.payment_url,
                    CollectionId = response.data.collection_id.ToString(),
                    TransactionReference = response.data.transaction_reference,
                    RequestReference = response.data.request_reference,
                    //CustomerFirstName = request.customer_info is null ? "" : request.customer_info.firstName,
                    //CustomerLastName = request.customer_info is null ? "" : request.customer_info.lastName,
                    //CustomerEmail = request.customer_info is null ? "" : request.customer_info.email,
                });

                await _gratipTransactionRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to log Gratip transactions >> " +
                    $"{ex.Message} | stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }
    }
}
