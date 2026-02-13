using ElevaniPaymentGateway.Core.Configs;
using Microsoft.Extensions.Options;

namespace ElevaniPaymentGateway.Worker.PayAgency
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly BackgroundJobConfig _backgroundJobConfig;
        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider,
            IOptions<BackgroundJobConfig> backgroundJobConfig)
        {
            _serviceProvider = serviceProvider;
            _backgroundJobConfig = backgroundJobConfig.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Pay agency worker service running at: {time}", DateTimeOffset.Now);
            }

            while (true)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var transactionVerificationService = scope.ServiceProvider.GetService<Handler>();
                    await transactionVerificationService.VerifyTransaction();
                }

                await Task.Delay(TimeSpan.FromSeconds(_backgroundJobConfig.PayAgencyTransactionVerificationTaskDelay), stoppingToken);
                _logger.LogInformation($"{Environment.NewLine}");
            }
        }
    }
}
