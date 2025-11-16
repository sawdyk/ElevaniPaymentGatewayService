using ElevaniPaymentGateway.Core.Configs;
using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Models.Request.Gratip;
using ElevaniPaymentGateway.Core.Models.Response.Gratip;
using ElevaniPaymentGateway.Infrastructure.Interfaces.ProxyClients.Gratip;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.ProxyClients.Gratip
{
    public class GratipCollectionService : IGratipCollectionService
    {
        private readonly ILogger<GratipCollectionService> _logger;
        private readonly IGratipServiceProxyClient _gratipServiceProxyClient;
        private readonly GratipConfig _gratipConfig;
        public GratipCollectionService(IGratipServiceProxyClient gratipServiceProxyClient, ILogger<GratipCollectionService> logger,
            IOptions<GratipConfig> gratipConfig)
        {
            _gratipServiceProxyClient = gratipServiceProxyClient;
            _logger = logger;
            _gratipConfig = gratipConfig.Value;
        }
        public async Task<FinalizeTransactionResponse> FinalizeTransactionAsync(FinalizeTransactionRequest request)
        {
            try
            {
                FinalizeTransactionResponse finalizeTransactionResponse = null;
                var messageJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(request), Encoding.UTF8, Application.Json);
                _logger.LogInformation($"finalize transaction request body => {JsonConvert.SerializeObject(request)}");

                var httpResponse = await _gratipServiceProxyClient.PostAsync(_gratipConfig.TransactionsConfig.Finalize, messageJson);
                var contentString = await httpResponse.Content.ReadAsStringAsync();

                _logger.LogInformation($"finalize transaction response body => {contentString}");
                if (httpResponse.StatusCode.Equals(HttpStatusCode.OK))
                {
                    finalizeTransactionResponse = JsonConvert.DeserializeObject<FinalizeTransactionResponse>(contentString)!;
                    return finalizeTransactionResponse;
                }

                return finalizeTransactionResponse!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to finalize transaction collection - {ex.Message}" +
                    $"| stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<InitiateTransctionResponse> InitiateTransactionAsync(InitiateTransactionRequest request)
        {
            try
            {
                InitiateTransctionResponse initiateTransctionResponse = null;
                var messageJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(request), Encoding.UTF8, Application.Json);
                _logger.LogInformation($"initiate transaction request body => {JsonConvert.SerializeObject(request)}");

                var httpResponse = await _gratipServiceProxyClient.PostAsync(_gratipConfig.TransactionsConfig.Initiate, messageJson);
                var contentString = await httpResponse.Content.ReadAsStringAsync();

                _logger.LogInformation($"initiate transaction response body => {contentString}");
                if (httpResponse.StatusCode.Equals(HttpStatusCode.OK))
                {
                    initiateTransctionResponse = JsonConvert.DeserializeObject<InitiateTransctionResponse>(contentString)!;
                    return initiateTransctionResponse;
                }

                return initiateTransctionResponse!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to initiate transaction collection - {ex.Message}" +
                    $"| stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<ListTransactionResponse> ListTransactionsAsync(string status, int limit, int page, DateTime startDate)
        {
            try
            {
                ListTransactionResponse listTransactionResponse = null;
                var httpResponse = await _gratipServiceProxyClient.GetAsync(_gratipConfig.TransactionsConfig.List
                    .Replace("{status}",status)
                    .Replace("{limit}", limit.ToString())
                    .Replace("{page}", page.ToString())
                    .Replace("{startDate}", startDate.ToString()));

                var contentString = await httpResponse.Content.ReadAsStringAsync();

                _logger.LogInformation($"list of transactions response body => {contentString}");
                if (httpResponse.StatusCode.Equals(HttpStatusCode.OK))
                {
                    listTransactionResponse = JsonConvert.DeserializeObject<ListTransactionResponse>(contentString)!;
                    return listTransactionResponse!;
                }

                return listTransactionResponse!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to fetch all transaction collection - {ex.Message}" +
                     $"| stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<TransactionStatusResponse> TransactionStatusAsync(string transactionReference)
        {
            try
            {
                TransactionStatusResponse transactionStatusResponse = null;
                var httpResponse = await _gratipServiceProxyClient.GetAsync(_gratipConfig.TransactionsConfig.Status.Replace("{transactionReference}", transactionReference));
                var contentString = await httpResponse.Content.ReadAsStringAsync();

                _logger.LogInformation($"verify transaction reference = {transactionReference} | response status => {httpResponse.StatusCode}");
                if (httpResponse.StatusCode.Equals(HttpStatusCode.OK))
                {
                    transactionStatusResponse = JsonConvert.DeserializeObject<TransactionStatusResponse>(contentString)!;
                    return transactionStatusResponse!;
                }

                return transactionStatusResponse!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to get transaction collection status - {ex.Message}" +
                     $"| stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }
    }
}
