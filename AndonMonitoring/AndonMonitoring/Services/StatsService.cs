using AndonMonitoring.QueryBuilder;
using AndonMonitoring.Services.Interfaces;
using AndonMonitoring.Repositories.Interface;
using AndonMonitoring.Data;

namespace AndonMonitoring.Services
{
    public class StatsService : IStatsService
    {
        private readonly IStatRepository statRepository;
        private readonly IEventRepository eventRepository;
        private readonly IAndonService andonService;
        public StatsService(
                IStatRepository statR,
                IEventRepository eventR,
                IAndonService andonS)
        {
            statRepository = statR;
            eventRepository = eventR;
            andonService = andonS;
        }

        public int GetAndonStateCountByDay(StatQuery param)
        {
            int count = -1;
            try
            {
                param.isDayFormat();
                count = statRepository.GetAndonStateCountByDay(param);
            }
            catch (Exception)
            {
                throw;
            }

            if (count == -1)
                throw new Exception(); //TODO
            return count;
        }

        public int GetAndonStateCountByMonth(StatQuery param)
        {
            int count = -1;
            try
            {
                param.isMonthFormat();
                count = statRepository.GetAndonStateCountByMonth(param);
            }
            catch (Exception)
            {
                throw;
            }

            if (count == -1)
                throw new Exception(); //TODO
            return count;
        }

        public int GetAndonStateMinutesByDay(StatQuery param)
        {
            int minutes = -1;
            try
            {
                param.isDayFormat();
                minutes = statRepository.GetAndonStateMinutesByDay(param);
            }
            catch (Exception)
            {
                throw;
            }

            if (minutes == -1)
                throw new Exception();   //TODO
            return minutes;
        }

        public int GetAndonStateMinutesByMonth(StatQuery param)
        {
            int minutes = -1;
            try
            {
                param.isMonthFormat();
                minutes = statRepository.GetAndonStateMinutesByMonth(param);
            }
            catch (Exception)
            {
                throw;
            }

            if (minutes == -1)
                throw new Exception();       //TODO
            return minutes;
        }

        public void createDailyStat(DateTime day)
        {
            statRepository.DeleteOnDay(day);

            Dictionary<int, EventDto> latests = new Dictionary<int, EventDto>();
            IEnumerable<EventDto> events;

            Dictionary<(int,int), StatQuery> statQueries = new Dictionary<(int, int), StatQuery>();

            try
            {
                events = eventRepository.GetEventsFromDay(day);
            }
            catch { throw; }

            foreach (EventDto curr in events)
            {
                try
                {
                    if (latests.ContainsKey(curr.AndonId))      
                    {
                        EventDto lastEvent = latests[curr.AndonId];
                        if (statQueries.ContainsKey((curr.AndonId, lastEvent.StateId)))
                        {
                            setDaily(curr, lastEvent, statQueries); 
                        }
                        else
                        {
                            addDaily(curr, lastEvent, statQueries);
                        }
                        latests[curr.AndonId] = curr;
                    }
                    else
                    {
                        latests.Add(curr.AndonId, curr); 
                        addDaily(curr,statQueries);
                    }
                }
                catch { throw; }
            }
           
            //seeing whether there are any andons that didn't have an event yet that day (but need stats anyways)
            List<int> andonIds = andonService.GetAndonIds();

            foreach(int andonId in andonIds)
            {
                    try
                    {
                        if (!latests.ContainsKey(andonId))
                        {
                            addStatForFullDay(andonId, day, statQueries);
                        }
                    }
                    catch { throw; }
            }

            statRepository.AddDayStats(statQueries.Values.ToList());
        }

        private void setDaily(EventDto curr, EventDto last, Dictionary<(int, int), StatQuery> queries)
        {
            int previousminutes = queries[(curr.AndonId, last.StateId)].Minutes;
            int minutes = (int)(curr.StartDate - last.StartDate).TotalMinutes + previousminutes;

            int count = queries[(curr.AndonId, last.StateId)].Count;
            count++;
            StatQueryBuilder builder = new StatQueryBuilder()
                .WithAndon(curr.AndonId)
                .WithState(last.StateId)
                .WithMinutes(minutes)
                .WithCount(count)
                .OnDay(last.StartDate);
            StatQuery stat = builder.Build();
            queries[(curr.AndonId, last.StateId)] = stat;
 
        }

        private void addDaily(EventDto curr, EventDto last, Dictionary<(int,int),StatQuery> queries)
        {
            int lastStateId = last.StateId;
            int minutes = (int)(curr.StartDate - last.StartDate).TotalMinutes;
            StatQueryBuilder builder = new StatQueryBuilder()
                .WithAndon(curr.AndonId)
                .WithState(lastStateId)
                .WithMinutes(minutes)
                .WithCount(1)
                .OnDay(curr.StartDate);
            var stat = builder.Build();
            queries.Add((curr.AndonId, lastStateId), stat);
        }
        
        private void addDaily(EventDto currEvent, Dictionary<(int,int), StatQuery> list)
        {
            int lastStateId = eventRepository.GetPreviousState(currEvent.AndonId, currEvent.StartDate);

            //will return the time in minutes between midnight, and the time of the event happening
            int minutes = (int)(currEvent.StartDate - currEvent.StartDate.Date).TotalMinutes;
            StatQueryBuilder builder = new StatQueryBuilder()
                .WithAndon(currEvent.AndonId)
                .WithState(lastStateId)
                .WithMinutes(minutes)
                .WithCount(1)
                .OnDay(currEvent.StartDate);
            var stat = builder.Build();
            //statRepository.AddDayStat(stat);
            list.Add((currEvent.AndonId, lastStateId), stat);
        }

        //when the andon didn't have any event on the given day
        private void addStatForFullDay(int andonId, DateTime day, Dictionary<(int,int), StatQuery> list)
        {
            int lastStateId = eventRepository.GetPreviousState(andonId, day);
            int minutes = 0;

            //assign minutes according to whether the given day has already passed or not
            if(day.Date == DateTime.Now.Date)
            {
                //minutes since midnight
                minutes = (int)(DateTime.Now - DateTime.Now.Date).TotalMinutes;
            } else
            {
                minutes = 24 * 60;
            }

            StatQueryBuilder builder = new StatQueryBuilder()
                .WithAndon(andonId)
                .WithState(lastStateId)
                .WithMinutes(minutes)
                .WithCount(1)
                .OnDay(day);

            var stat = builder.Build();
            list.Add((andonId, lastStateId), stat);
        }

        public void createMonthlyStat(DateTime month)
        {
            statRepository.DeleteOnMonth(month);
            List<DayStatDto> stats = statRepository.GetDailyStatsOnMonth(month);

            Dictionary<(int, int), StatQuery> statQueries = new Dictionary<(int, int), StatQuery>();

            foreach ( var stat in stats )
            {
                try
                {
                    if (statQueries.ContainsKey((stat.AndonId, stat.StateId)))
                    {
                        setMonthly(stat, statQueries);
                    }
                    else
                    {
                        addMonthly(stat, statQueries);
                    }
                }
                catch { throw; }
            }

            statRepository.AddMonthStats(statQueries.Values.ToList());
        }

        private void addMonthly(DayStatDto stat, Dictionary<(int, int), StatQuery> statQueries)
        {
            StatQueryBuilder builder = new StatQueryBuilder()
                .WithAndon(stat.AndonId)
                .WithState(stat.StateId)
                .WithMinutes(stat.OverallMinutes)
                .WithCount(stat.OverallCount)
                .OnMonth(stat.Day);
            var query = builder.Build();
            statQueries.Add((stat.AndonId, stat.StateId), query);
        }

        private void setMonthly(DayStatDto stat, Dictionary<(int, int), StatQuery> statQueries)
        {
            int previousminutes = statQueries[(stat.AndonId, stat.StateId)].Minutes;
            int minutes = stat.OverallMinutes + previousminutes;
            int count = statQueries[(stat.AndonId, stat.StateId)].Count;
            count += stat.OverallCount;

            StatQueryBuilder builder = new StatQueryBuilder()
                .WithAndon(stat.AndonId)
                .WithState(stat.StateId)
                .WithMinutes(minutes)
                .WithCount(count)
                .OnMonth(stat.Day);
            var query = builder.Build();
            statQueries[(stat.AndonId, stat.StateId)] = query;
        }
    }
}
