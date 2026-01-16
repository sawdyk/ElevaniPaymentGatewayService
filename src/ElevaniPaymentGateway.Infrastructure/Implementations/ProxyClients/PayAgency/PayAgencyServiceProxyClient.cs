using ElevaniPaymentGateway.Infrastructure.Interfaces.ProxyClients.PayAgency;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.ProxyClients.PayAgency
{
    public class PayAgencyServiceProxyClient : IPayAgencyServiceProxyClient
    {
        private HttpClient _client { get; }
        public PayAgencyServiceProxyClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> GetAsync(string? requestUri)
        {
            return await _client.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> PostAsync(string? requestUri, HttpContent? content)
        {
            return await _client.PostAsync(requestUri, content);
        }

        public async Task<HttpResponseMessage> PutAsync(string? requestUri, HttpContent? content)
        {
            return await _client.PutAsync(requestUri, content);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string? requestUri)
        {
            return await _client.DeleteAsync(requestUri);
        }
    }
}
