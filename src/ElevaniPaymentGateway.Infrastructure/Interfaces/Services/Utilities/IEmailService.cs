using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Utilities
{
    public interface IEmailService : IAutoDependencyServices
    {
        Task SendEmailAsync(EmailRequest req);
    }
}
