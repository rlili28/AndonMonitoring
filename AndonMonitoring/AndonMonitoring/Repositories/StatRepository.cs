using AndonMonitoring.QueryBuilder;
using AndonMonitoring.AndonExceptions;
using AndonMonitoring.Model;
using AndonMonitoring.Repositories.Interface;
using AndonMonitoring.Data;
using System;

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
                        d.Day.Date == query.Day.Date)
                    .Select(d => d.OverallMinutes);

                if (result == null)
                    throw new Exception("id doesn't exist");

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

            //query.Month = query.Month.ToUniversalTime();
            try
            {
                var result = db.MonthStat
                    .Where(m =>
                        m.AndonId == query.AndonId &&
                        m.StateId == query.StateId &&
                        m.Month.Month == query.Month.Month &&
                        m.Month.Year == query.Month.Year)
                    .Select(d => d.OverallMinutes);

                if (result == null)
                    throw new Exception("id doesn't exist");

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
                        m.Day.Date == query.Day.Date)
                    .Select(d => d.StateCount);

                if (result == null)
                    throw new Exception("id doesn't exist");

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

            //query.Month = query.Month.ToUniversalTime();

            try
            {
                var result = db.MonthStat
                    .Where(m =>
                        m.AndonId == query.AndonId &&
                        m.StateId == query.StateId &&
                        m.Month.Month == query.Month.Month &&
                        m.Month.Year == query.Month.Year)
                    .Select(d => d.StateCount);

                if (result == null)
                    throw new Exception("id doesn't exist");
                

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
            day = day.ToUniversalTime();
            try
            {
                return db.DayStat.Any(
                    d => d.Day.Date == day.Date &&
                    d.AndonId == andonId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool isMonthAdded(DateTime month, int andonId)
        {
            try
            {
                return db.MonthStat.Any(
                    d => d.Month.Year == month.Year &&
                    d.Month.Month == month.Month &&
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
            catch { throw; }


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

        public void SetDayStat(StatQuery query)
        {
            query.Day = query.Day.ToUniversalTime();
            try
            {
                query.isSetFormat();
                var dayStat = db.DayStat.FirstOrDefault(s => s.Id == query.Id);
                if(dayStat != null)
                {
                    if(query.Day.Date != dayStat.Day.Date)
                    {
                        dayStat.Day = query.Day;
                    }
                    dayStat.AndonId = query.AndonId;
                    dayStat.StateId = query.StateId;
                    dayStat.OverallMinutes = query.Minutes;
                    dayStat.StateCount = query.Count;
                    db.SaveChanges();
                    
                }
            }
            catch(Exception) { throw; }
        }

        public void SetMonthStat(StatQuery query)
        {
            try
            {
                query.isSetFormat();
                var monthStat = db.MonthStat.FirstOrDefault(s => s.Id == query.Id);
                if (monthStat != null)
                {
                    if (query.Month.Date != DateTime.Now.Date)
                    {
                        monthStat.Month = query.Month;
                    }
                    monthStat.AndonId = query.AndonId;
                    monthStat.StateId = query.StateId;
                    monthStat.OverallMinutes = query.Minutes;
                    monthStat.StateCount = query.Count;
                    db.SaveChanges();

                }
            }
            catch (Exception) { throw; }
        }

        public int AddMonthStat(StatQuery query)
        {
            if (query == null)
                throw new AndonFormatException("params weren't specified");

            try
            {
                query.isMonthFormat();
            }
            catch { throw; }

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
            catch (Exception) { throw; }

            return monthStat.Id;
        }

        public void DeleteOnDay(DateTime day)
        {
            try
            {
                var statsFromOneDay = db.DayStat
                    .Where(s => s.Day.Day == day.Day && s.Day.Month == day.Month && s.Day.Year == day.Year)
                    .ToList();

                foreach (var stat in statsFromOneDay)
                {
                    db.DayStat.Remove(stat);
                }

                db.SaveChanges();
            }
            catch (Exception) { throw; }
        }

        public void DeleteOnMonth(DateTime month)
        {
            try
            {
                var statsFromOneMonth = db.MonthStat
                    .Where(s => s.Month.Month == month.Month && s.Month.Year == month.Year)
                    .ToList();

                foreach(var stat in statsFromOneMonth)
                {
                    db.MonthStat.Remove(stat);
                }

                db.SaveChanges();
            }
            catch(Exception) { throw; }
        }
    }
}
