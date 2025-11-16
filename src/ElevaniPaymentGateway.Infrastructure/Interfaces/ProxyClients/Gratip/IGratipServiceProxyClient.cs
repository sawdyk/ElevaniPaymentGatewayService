namespace ElevaniPaymentGateway.Infrastructure.Interfaces.ProxyClients.Gratip
{
    public interface IGratipServiceProxyClient
    {
        Task<HttpResponseMessage> PostAsync(string? requestUri, HttpContent? content);
        Task<HttpResponseMessage> PutAsync(string? requestUri, HttpContent? content);
        Task<HttpResponseMessage> GetAsync(string? requestUri);
        Task<HttpResponseMessage> DeleteAsync(string? requestUri);
    }
}
