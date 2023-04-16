namespace AndonMonitoring.Services.Interfaces
{
    public interface IStatsService
    {
        public int GetAndonStateMinutesByDay(QueryBuilder.StatQuery param);
        public int GetAndonStateMinutesByMonth(QueryBuilder.StatQuery param);
        public int GetAndonStateCountByDay(QueryBuilder.StatQuery param);
        public int GetAndonStateCountByMonth(QueryBuilder.StatQuery param);
    }
}
