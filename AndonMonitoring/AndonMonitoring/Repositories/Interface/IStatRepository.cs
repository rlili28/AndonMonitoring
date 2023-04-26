using AndonMonitoring.QueryBuilder;

namespace AndonMonitoring.Repositories.Interface
{
    public interface IStatRepository
    {
        public int GetAndonStateMinutesByDay(StatQuery query);
        public int GetAndonStateMinutesByMonth(StatQuery query);
        public int GetAndonStateCountByDay(StatQuery query);
        public int GetAndonStateCountByMonth(StatQuery query);
        public int AddDayStat(StatQuery query);
        public int AddMonthStat(StatQuery query);
        public void SetDayStat(StatQuery query);
        public void SetMonthStat(StatQuery query);

        public bool isAdded(DateTime day, int andonId);
        public void DeleteOnDay(DateTime day);
        public void DeleteOnMonth(DateTime month);
    }
}
