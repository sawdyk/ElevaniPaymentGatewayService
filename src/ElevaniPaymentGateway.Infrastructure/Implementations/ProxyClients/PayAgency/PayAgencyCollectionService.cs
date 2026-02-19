using ElevaniPaymentGateway.Core.Configs;
using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Models.Request.PayAgency;
using ElevaniPaymentGateway.Core.Models.Response.PayAgency;
using ElevaniPaymentGateway.Infrastructure.Interfaces.ProxyClients.PayAgency;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.ProxyClients.PayAgency
{
    public class PayAgencyCollectionService : IPayAgencyCollectionService
    {
        private readonly ILogger<PayAgencyCollectionService> _logger;
        private readonly IPayAgencyServiceProxyClient _payAgencyServiceProxyClient;
        private readonly PayAgencyConfig _payAgencyConfig;
        public PayAgencyCollectionService(IPayAgencyServiceProxyClient payAgencyServiceProxyClient, 
            ILogger<PayAgencyCollectionService> logger, IOptions<PayAgencyConfig> payAgencyConfig)
        {
            _payAgencyServiceProxyClient = payAgencyServiceProxyClient;
            _logger = logger;
            _payAgencyConfig = payAgencyConfig.Value;
        }

        public async Task<PayAgencyTransactionResponse> InitiateTransactionAsync(PayAgencyEncryptedRequest request)
        {
            try
            {
                PayAgencyTransactionResponse payAgencyTransactionResponse = null;
                var messageJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(request), Encoding.UTF8, Application.Json);
                _logger.LogInformation($"initiate transaction request body => {JsonConvert.SerializeObject(request)}");

                var httpResponse = await _payAgencyServiceProxyClient.PostAsync(_payAgencyConfig.Initiate, messageJson);
                var contentString = await httpResponse.Content.ReadAsStringAsync();

                _logger.LogInformation($"initiate transaction response body => {contentString}");
                if (!httpResponse.StatusCode.Equals(HttpStatusCode.InternalServerError))
                {
                    payAgencyTransactionResponse = JsonConvert.DeserializeObject<PayAgencyTransactionResponse>(contentString)!;
                    return payAgencyTransactionResponse;
                }

                return payAgencyTransactionResponse!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to initiate card transaction - {ex.Message}" +
                    $"| stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<PayAgencyTransactionResponse> TransactionStatusAsync(string transactionReference)
        {
            try
            {
                PayAgencyTransactionResponse payAgencyTransactionResponse = null;
                var httpResponse = await _payAgencyServiceProxyClient.GetAsync(_payAgencyConfig.Status.Replace("{id}", transactionReference));
                var contentString = await httpResponse.Content.ReadAsStringAsync();

                _logger.LogInformation($"verify transaction reference = {transactionReference} | response status => {httpResponse.StatusCode}");
                if (httpResponse.StatusCode.Equals(HttpStatusCode.OK))
                {
                    payAgencyTransactionResponse = JsonConvert.DeserializeObject<PayAgencyTransactionResponse>(contentString)!;
                    return payAgencyTransactionResponse!;
                }

                return payAgencyTransactionResponse!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to get transaction status - {ex.Message}" +
                     $"| stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }
    }
}
