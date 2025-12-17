using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Utilities
{
    public interface ILoginEmailService : IAutoDependencyServices
    {
        Task SendLoginMailNotificationAsync(User user);
    }
}
