using BillingApplication.Server.Quartz;

namespace BillingApplication.Server.Services.MailService
{
    public class DataSchedulerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public DataSchedulerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                DataScheduler.Start(serviceProvider);
            }
            await Task.CompletedTask;
        }
    }

}
