using ElevaniPaymentGateway.Core.Models.Response.Gratip;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services.PaymentGateway.Gratip
{
    public interface IGratipWebhookService : IAutoDependencyServices
    {
        Task<bool> VerifyWebhookSignature(string payload, string signature);
        Task HandleWebhookNotification(string payload);
        Task HandleCollectionCompleted(GratipWebhookData payload);
    }
}
