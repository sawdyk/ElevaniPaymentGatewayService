using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Utilities
{
    public interface IAccountNotificationEmailService : IAutoDependencyServices
    {
        Task SendResetAndChangedPasswordMailAsync(User user);
        Task SendForgotPasswordMailAsync(User user);
        Task SendNewAdminUserMailAsync(User user, string defaultPassword);
    }
}
