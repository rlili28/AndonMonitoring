
namespace AndonMonitoring.TimedService
{
    public class TimedHostedStatService : BackgroundService
    {
        public TimedHostedStatService(IServiceProvider services)
        {
            Services = services;
        }
        private IServiceProvider Services { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken token)
        {
            using (var scope = Services.CreateScope())
            {
                var StatService = scope.ServiceProvider.GetRequiredService<IScopedStatService>();
                await StatService.DoWork(token);
            }
        }
    }
}
