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
            try
            {
                events = eventRepository.GetEventsFromDay(day);
            } catch { throw; }

            foreach (EventDto curr in events)
            {
                try
                {
                    if (statRepository.isAdded(curr.StartDate, curr.AndonId))       //TODO: lehet ez is felesleges
                    {
                        if (latests.ContainsKey(curr.AndonId))
                        {
                            EventDto lastEvent = latests[curr.AndonId];
                            setDaily(curr, lastEvent);
                            latests[curr.AndonId] = curr;
                        }
                        else
                        {
                            //TODO: nem is biztos hogy valaha megtortenhet
                        }
                    }
                    else
                    {
                        if (!latests.ContainsKey(curr.AndonId))
                        {
                            latests.Add(curr.AndonId, curr);
                            addDaily(curr);
                        }
                        else
                        {
                            //TODO: nem is biztos hogy valaha megtortenhet
                        }
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
                        if (statRepository.isAdded(day, andonId))
                        { 
                            //TODO lehet hogy nem is kene ellenoriznem
                        }
                        else
                        {
                            addStatForFullDay(andonId, day);
                        }
                    }
                }
                catch { throw; }
            }
        }

        private void setDaily(EventDto curr, EventDto last)
        {
            if (curr.StartDate.Year == last.StartDate.Year && curr.StartDate.Month == last.StartDate.Month && curr.StartDate.Day == last.StartDate.Day)
            {
                int minutes = (curr.StartDate - last.StartDate).Minutes;
                StatQueryBuilder builder = new StatQueryBuilder()
                    .WithAndon(curr.AndonId)
                    .WithState(last.StateId)
                    .WithMinutes(minutes)
                    .WithCount(1)
                    .OnDay(last.StartDate);
                StatQuery stat = builder.Build();
                statRepository.SetDayStat(stat);
            }
            else
            {
                throw new Exception("previous event wasn't on the same day, yet there's already a record for the day with this andon?");
            }
        }

        private void addDaily(EventDto currEvent)
        {
            int lastStateId = eventRepository.GetPreviousState(currEvent.AndonId, currEvent.StartDate);

            //will return the time in minutes between midnight, and the time of the event happening
            int minutes = (currEvent.StartDate - currEvent.StartDate.Date).Minutes;
            StatQueryBuilder builder = new StatQueryBuilder()
                .WithAndon(currEvent.AndonId)
                .WithState(lastStateId)
                .WithMinutes(minutes)
                .WithCount(1)
                .OnDay(currEvent.StartDate);
            var stat = builder.Build();
            statRepository.AddDayStat(stat);
        }

        //when the andon didn't have any event on the given day
        private void addStatForFullDay(int andonId, DateTime day)
        {
            int lastStateId = eventRepository.GetPreviousState(andonId, day);
            int minutes = 0;

            //assign minutes according to whether the given day has already passed or not
            if(day.Date == DateTime.Now.Date)
            {
                //minutes since midnight
                minutes = (DateTime.Now - DateTime.Now.Date).Minutes;
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
            statRepository.AddDayStat(stat);
        }

        //TODO: egesz metodus
        public void createMonthlyStat(DateTime month)
        {
            

        }
    }
}
