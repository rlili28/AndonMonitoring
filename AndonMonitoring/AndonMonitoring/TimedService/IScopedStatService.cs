namespace AndonMonitoring.TimedService
{
    public interface IScopedStatService
    {
        public Task DoWork(CancellationToken token);
    }
}
