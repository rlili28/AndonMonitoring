using AndonMonitoring.Services.Interfaces;

namespace AndonMonitoring.TimedService
{
    public class ScopedStatService : IScopedStatService
    {
        private Timer? _timer = null;
        private IStatsService _statsService;
        public ScopedStatService(IStatsService statService)
        {
            _statsService = statService;
        }

        public Task DoWork(CancellationToken token)
        {
            TimeSpan period = TimeSpan.FromDays(1);
            TimeSpan dueTime;
            DateTime lastMidnight = DateTime.Now.Date;
            if (DateTime.Now.Hour < 1)
            {
                DateTime _1am = lastMidnight.AddHours(1);
                dueTime = _1am - DateTime.Now;
            }
            else
            {
                DateTime _1am = lastMidnight.AddDays(1).AddHours(1);
                dueTime = _1am - DateTime.Now;
            }

            _timer = new Timer(createStat, null, dueTime, period);
            return Task.CompletedTask;
        }

        private void createStat(object? state)
        {
            _statsService.createDailyStat(DateTime.Now);
            _statsService.createMonthlyStat(DateTime.Now);
        }

        public Task StopWork(CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
   