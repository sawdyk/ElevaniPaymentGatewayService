namespace ElevaniPaymentGateway.Infrastructure.Interfaces.ProxyClients.PayAgency
{
    public interface IPayAgencyServiceProxyClient
    {
        Task<HttpResponseMessage> PostAsync(string? requestUri, HttpContent? content);
        Task<HttpResponseMessage> PutAsync(string? requestUri, HttpContent? content);
        Task<HttpResponseMessage> GetAsync(string? requestUri);
        Task<HttpResponseMessage> DeleteAsync(string? requestUri);
    }
}
