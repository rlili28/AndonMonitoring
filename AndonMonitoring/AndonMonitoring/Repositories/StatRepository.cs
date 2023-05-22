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


        //methods for getting stats

        /// <summary>
        /// Gets the total number of minutes that the provided andon light was in the provided state on the given day from the database.
        /// </summary>
        /// <param name="query">The query params. (Andon id, State id, Day)</param>
        /// <returns>The total number of minutes that the Andon light was in the specified state on the specified day</returns>
        /// <exception cref="AndonFormatException">Thrown when the there is no record in the database that matches the query params.</exception>
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
                    throw new Exception("stats with the given specification don't exist");

                int minutes = result.FirstOrDefault();
                return minutes;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the total number of minutes that the provided andon light was in the provided state on the given month from the database.
        /// </summary>
        /// <param name="query">The query params. (Andon id, State id, Month)</param>
        /// <returns>The total number of minutes that the Andon light was in the specified state on the specified month</returns>
        /// <exception cref="AndonFormatException">Thrown when the there is no record in the database that matches the query params.</exception>
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
                        m.Month.Month == query.Month.Month &&
                        m.Month.Year == query.Month.Year)
                    .Select(d => d.OverallMinutes);

                if (result == null)
                    throw new Exception("stats with the given specification don't exist");

                int minutes = result.FirstOrDefault();
                return minutes;

            }
            catch (Exception)
            {
                throw;
            }
        }
        
        /// <summary>
        /// Gets the total number of times that the provided andon light was in the provided state on the given day from the database.
        /// </summary>
        /// <param name="query">The query params. (Andon id, State id, Day)</param>
        /// <returns>The total number of times that the Andon light was in the specified state on the specified day</returns>
        /// <exception cref="AndonFormatException">Thrown when the there is no record in the database that matches the query params.</exception>
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
                    throw new Exception("stats with the given specification don't exist");

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
                        m.Month.Month == query.Month.Month &&
                        m.Month.Year == query.Month.Year)
                    .Select(d => d.StateCount);

                if (result == null)
                    throw new Exception("stats with the given specification don't exist");
                

                int minutes = result.FirstOrDefault();
                return minutes;

            }
            catch (Exception)
            {
                throw;
            }
        }


        //methods for creating stats

        public void AddDayStats(List<StatQuery> queries)
        {
            if (queries != null)
            {
                foreach( var query in queries)
                {
                    try
                    {
                        query.isDayFormat();

                        var dayStat = new DayStat
                        {
                            Day = query.Day,
                            AndonId = query.AndonId,
                            StateId = query.StateId,
                            OverallMinutes = query.Minutes,
                            StateCount = query.Count
                        };

                        db.DayStat.Add(dayStat);
                        db.SaveChanges();
                    }
                    catch { throw; }
                }
            }
        }

        public void AddMonthStats(List<StatQuery> queries)
        {
            if(queries != null)
            {
                foreach (var query in queries)
                {
                    try
                    {
                        query.isMonthFormat();

                        var monthStat = new MonthStat
                        {
                            Month = query.Month,
                            AndonId = query.AndonId,
                            StateId = query.StateId,
                            OverallMinutes = query.Minutes,
                            StateCount = query.Count
                        };
                        db.MonthStat.Add(monthStat);
                        db.SaveChanges();
                    }
                    catch { throw; }
                }
            }
        }

        public List<DayStatDto> GetStatsOnDay(DateTime day)
        {
            try
            {
                return db.DayStat
                    .Where(d => d.Day.Date == day.Date)
                    .Select(d => new DayStatDto(d.Id, d.AndonId, d.StateId, d.Day, d.OverallMinutes, d.StateCount))
                    .ToList();

            }
            catch(Exception)
            {
                throw;
            }
        }

        public List<DayStatDto> GetDailyStatsOnMonth(DateTime month)
        {
            try
            {
                return db.DayStat
                    .Where(m => m.Day.Month == month.Month && m.Day.Year == month.Year)
                    .Select(m => new DayStatDto(m.Id, m.AndonId, m.StateId, m.Day, m.OverallMinutes, m.StateCount))
                    .ToList();
            }
            catch (Exception)
            {
                throw;
            }
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
                Month = query.Month,
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

    }
}
