using AndonMonitoring.QueryBuilder;

namespace AndonMonitoring.Repositories
{
    public interface IStatRepository
    {
        public int GetAndonStateMinutesByDay(StatQuery query);
        public int GetAndonStateMinutesByMonth(StatQuery query);
        public int GetAndonStateCountByDay(int andonId, int stateId, DateTime month);
        public int GetAndonStateCountByMonth(int andonId, int stateId, DateTime month);
        public int AddDayStat(DateTime day, int andonId, int stateId, int count, int minutes);
        public int AddMonthStat(DateTime month, int andonId, int stateId, int count, int minutes);
        public int SetDayStat(DateTime day, int andonId, int stateId, int count, int minutes);
        public int SetMonthStat(DateTime month, int andonId, int stateId, int count, int minutes);
    }
}
