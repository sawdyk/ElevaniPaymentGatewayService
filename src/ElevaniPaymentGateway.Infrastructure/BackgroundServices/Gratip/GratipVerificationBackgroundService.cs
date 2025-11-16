using ElevaniPaymentGateway.Core.Configs;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ElevaniPaymentGateway.Infrastructure.BackgroundServices.Gratip
{
    public class GratipVerificationBackgroundJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly BackgroundJobConfig _backgroundJobConfig;
        private readonly ILogger<GratipVerificationBackgroundJob> _logger;
        public GratipVerificationBackgroundJob(IServiceProvider serviceProvider,
            IOptions<BackgroundJobConfig> backgroundJobConfig, ILogger<GratipVerificationBackgroundJob> logger)
        {
            _serviceProvider = serviceProvider;
            _backgroundJobConfig = backgroundJobConfig.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation($"Gratip transaction finialization and verification started at {DateTime.Now}");

                while (true)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var transactionVerificationService = scope.ServiceProvider.GetService<IGratipTransactionVerificationService>();
                        await transactionVerificationService!.FinalizeAndVerifyTransactions();
                    }

                    await Task.Delay(TimeSpan.FromSeconds(_backgroundJobConfig.GratipTransactionVerificationTaskDelay), stoppingToken);
                    _logger.LogInformation($"{Environment.NewLine}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while executing Gratip transaction verification service - {ex.Message}");
                _logger.LogError($"stack trace >>> {ex.StackTrace} | innver exception >>> {ex.InnerException} | source >>> {ex.Source}");
            }
        }
    }
}
