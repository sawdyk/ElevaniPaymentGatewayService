using ElevaniPaymentGateway.Core.Configs;
using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Models.Response.Gratip;
using ElevaniPaymentGateway.Infrastructure.Interfaces.ProxyClients.Gratip;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.ProxyClients.Gratip
{
    public class GratipCredentialService : IGratipCredentialService
    {
        private readonly ILogger<GratipCredentialService> _logger;
        private readonly IGratipServiceProxyClient _gratipServiceProxyClient;
        private readonly GratipConfig _gratipConfig;
        public GratipCredentialService(IGratipServiceProxyClient gratipServiceProxyClient,
            ILogger<GratipCredentialService> logger, IOptions<GratipConfig> gratipConfig)
        {
            _gratipServiceProxyClient = gratipServiceProxyClient;
            _logger = logger;
            _gratipConfig = gratipConfig.Value;
        }

        public async Task<RotateCredentialResponse> RotateCredentialAsync()
        {
            try
            {
                RotateCredentialResponse rotateCredentialResponse = null;
                var httpResponse = await _gratipServiceProxyClient.PostAsync(_gratipConfig.RotateCredential, null);
                var contentString = await httpResponse.Content.ReadAsStringAsync();

                _logger.LogInformation($"rotate credential response body => {contentString}");
                if (httpResponse.StatusCode.Equals(HttpStatusCode.OK))
                {
                    rotateCredentialResponse = JsonConvert.DeserializeObject<RotateCredentialResponse>(contentString)!;
                    return rotateCredentialResponse;
                }

                return rotateCredentialResponse!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error encountered while trying to rotate Gratip credential - {ex.Message}" +
                    $"| stack trace >> {ex.StackTrace} | inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }
    }
}
