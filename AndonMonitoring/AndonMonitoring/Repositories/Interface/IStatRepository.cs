using AndonMonitoring.Data;
using AndonMonitoring.QueryBuilder;

namespace AndonMonitoring.Repositories.Interface
{
    public interface IStatRepository
    {
        public int GetAndonStateMinutesByDay(StatQuery query);
        public int GetAndonStateMinutesByMonth(StatQuery query);
        public int GetAndonStateCountByDay(StatQuery query);
        public int GetAndonStateCountByMonth(StatQuery query);

        public void AddDayStats(List<StatQuery> queries);
        public void AddMonthStats(List<StatQuery> queries);

        public List<DayStatDto> GetStatsOnDay(DateTime day);
        public List<DayStatDto> GetDailyStatsOnMonth(DateTime month);

        public void DeleteOnDay(DateTime day);
        public void DeleteOnMonth(DateTime month);

        public int AddMonthStat(StatQuery query);
        public void SetMonthStat(StatQuery query);
    }
}
