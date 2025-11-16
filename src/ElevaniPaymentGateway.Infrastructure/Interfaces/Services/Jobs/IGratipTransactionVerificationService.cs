using ElevaniPaymentGateway.Infrastructure.Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Jobs
{
    public interface IGratipTransactionVerificationService : IAutoDependencyServices
    {
        Task FinalizeAndVerifyTransactions();
    }
}
