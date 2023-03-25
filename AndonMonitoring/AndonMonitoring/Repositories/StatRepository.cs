using AndonMonitoring.Data;
using AndonMonitoring.QueryBuilder;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AndonMonitoring.Repositories
{
    public class StatRepository : IStatRepository
    {
        private AndonDbContext db;
        public StatRepository(AndonDbContext context)
        {
            this.db = context;
        }


        public int GetAndonStateMinutesByDay(StatQuery query)
        {
            if (query == null || query.StateId < 0 || query.AndonId < 0 || query.Day == null)
                return -2;      //some argument wasn't provided

            var result = db.DayStat
                .Where(d =>
                d.AndonId == query.AndonId &&
                d.StateId == query.StateId &&
                d.Day.Day == query.Day.Day)
                .Select(d => d.OverallMinutes);

            if (result == null)
                return -1;      //no such record found

            int minutes = result.FirstOrDefault();
            return minutes;
        }

        public int GetAndonStateMinutesByMonth(StatQuery query)
        {
            if (query == null || query.StateId < 0 || query.AndonId < 0 || query.Month == null)
                return -2;      //some argument wasn't provided

            var result = db.MonthStat
                .Where(m =>
                m.AndonId == query.AndonId &&
                m.StateId == query.StateId &&
                m.Month.Month == query.Month.Month)
                .Select(d => d.OverallMinutes);

            if (result == null)
                return -1;      //no such record found

            int minutes = result.FirstOrDefault();
            return minutes;
        }

        public int GetAndonStateCountByDay(int andonId, int stateId, DateTime month)
        {
            return 0;
        }

        public int GetAndonStateCountByMonth(int andonId, int stateId, DateTime month)
        {
            return 0;
        }

        public int AddDayStat(DateTime day, int andonId, int stateId, int count, int minutes)
        {
            return 0;
        }

        public int AddMonthStat(DateTime month, int andonId, int stateId, int count, int minutes)
        {
            return 0;
        }

        public int SetDayStat(DateTime day, int andonId, int stateId, int count, int minutes)
        {
            return 0;
        }

        public int SetMonthStat(DateTime month, int andonId, int stateId, int count, int minutes)
        {
            return 0;
        }
    }
}
