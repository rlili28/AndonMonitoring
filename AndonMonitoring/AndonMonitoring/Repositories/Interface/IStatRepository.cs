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
        public int SetDayStat(StatQuery query);
        public int SetMonthStat(StatQuery query);

        public bool isAdded(DateTime day, int andonId);
        public int LastState(int andonId);
    }
}
