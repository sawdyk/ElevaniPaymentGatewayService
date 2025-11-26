using ElevaniPaymentGateway.Infrastructure.Interfaces.ProxyClients.Gratip;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.ProxyClients.Gratip
{
    public class GratipServiceProxyClient : IGratipServiceProxyClient
    {
        private HttpClient _client { get; }
        public GratipServiceProxyClient(HttpClient client)
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
