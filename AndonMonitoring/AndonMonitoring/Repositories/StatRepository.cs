using AndonMonitoring.QueryBuilder;
using AndonMonitoring.AndonExceptions;
using AndonMonitoring.Model;
using AndonMonitoring.Repositories.Interface;
using AndonMonitoring.Data;

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
            if (query == null)
                throw new AndonFormatException("params weren't specified");

            try
            {
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
            catch (Exception)
            {
                throw;
            }
        }

        public int GetAndonStateMinutesByMonth(StatQuery query)
        {
            if (query == null)
                throw new AndonFormatException("params weren't specified");
            try
            {
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
            catch (Exception)
            {
                throw;
            }
        }

        public int GetAndonStateCountByDay(StatQuery query)
        {
            if (query == null)
                throw new AndonFormatException("params weren't specified");
            
            try
            {
                var result = db.DayStat
                    .Where(m =>
                        m.AndonId == query.AndonId &&
                        m.StateId == query.StateId &&
                        m.Day.Day == query.Day.Day)
                    .Select(d => d.StateCount);

                if (result == null)
                    return -1;      //no such record found

                int minutes = result.FirstOrDefault();
                return minutes;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetAndonStateCountByMonth(StatQuery query)
        {
            if (query == null)
                throw new AndonFormatException("params weren't specified");

            try
            {
                var result = db.MonthStat
                    .Where(m =>
                        m.AndonId == query.AndonId &&
                        m.StateId == query.StateId &&
                        m.Month.Month == query.Month.Month)
                    .Select(d => d.StateCount);

                if (result == null)
                    return -1;      //no such record found

                int minutes = result.FirstOrDefault();
                return minutes;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool isAdded(DateTime day, int andonId)
        {
            try
            {
                return db.DayStat.Any(
                    d => d.Day.Year == day.Year && 
                    d.Day.Month == day.Month && 
                    d.Day.Day == day.Day && 
                    d.AndonId == andonId);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public int AddDayStat(StatQuery query)
        {
            if (query == null)
                throw new AndonFormatException("params weren't specified");
            try
            {
                query.isDayFormat();
            }
            catch { throw; } //TODO


            var dayStat = new DayStat
            {
                Day = query.Day.ToUniversalTime(),
                AndonId = query.AndonId,
                StateId = query.StateId,
                OverallMinutes = query.Minutes,
                StateCount = query.Count
            };

            try
            {
                db.DayStat.Add(dayStat);
                db.SaveChanges();
            }
            catch(Exception)
            {
                throw;
            }

            return dayStat.Id;
        }

        public int AddMonthStat(StatQuery query)
        {
            if(query == null)
                throw new AndonFormatException("params weren't specified");

            var monthStat = new MonthStat
            {
                Month = query.Month.ToUniversalTime(),
                AndonId = query.AndonId,
                StateId = query.StateId,
                OverallMinutes = query.Minutes,
                StateCount = query.Count
            };

            try
            {
                db.MonthStat.Add(monthStat);
                db.SaveChanges();
            }
            catch(Exception) { throw; }

            return monthStat.Id;
        }

        //TODO: egesz metodus
        public int SetDayStat(StatQuery query)
        {
            try
            {
                query.isSetFormat();
            }
            catch(Exception) { throw; }
            return 0;
        }


        //TODO: egesz metodus
        public int SetMonthStat(StatQuery query)
        {
            return 0;
        }

        //TODO: ennek nem igy kell mukodnie majd
        public int LastState(int AndonId)
        {
            return db.DayStat
                .Where(d => d.AndonId == AndonId)
                .OrderByDescending(d => d.Day)
                .Select(d => d.StateId)
                .FirstOrDefault();
        }
    }
}
