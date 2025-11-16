namespace ElevaniPaymentGateway.Worker.Gratip
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly Handler _handler;
        private readonly IServiceProvider _serviceProvider;
        public Worker(ILogger<Worker> logger, Handler handler, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _handler = handler;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    if (_logger.IsEnabled(LogLevel.Information))
                        _logger.LogInformation("Gratip transaction verification worker running at: {time}", DateTimeOffset.Now);

                    await _handler.FinalizeGratipTransactions();
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
