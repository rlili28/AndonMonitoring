using AndonMonitoring.QueryBuilder;
using AndonMonitoring.Services.Interfaces;
using AndonMonitoring.AndonExceptions;
using AndonMonitoring.Repositories.Interface;
using AndonMonitoring.Data;
using System.Data.SqlTypes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Diagnostics.Eventing.Reader;

namespace AndonMonitoring.Services
{
    public class StatsService : IStatsService
    {
        private readonly IStatRepository statRepository;
        private readonly IEventRepository eventRepository;
        public StatsService(IStatRepository repository, IEventRepository eventRepository)
        {
            statRepository = repository;
            this.eventRepository = eventRepository;
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

        public void createStat()
        {
            createDaily();
            createMonthlyStat();
        }


        /// <summary>
        ///  - kene bejegyzes azokrol a lampakrol, amiknek esemenytelen napja volt
        ///  - csak befejezett napokat akarok menteni egy ilyen alkalmaval
        ///  - kene a kovetkezo mentes szamara egy last state-t eltarolni valahol
        /// </summary>
        private void createDaily()
        {
            Dictionary<int, EventDto> latests = new Dictionary<int, EventDto>();
            IEnumerable<EventDto> events = eventRepository.GetEventsAsc();

            foreach (EventDto currEvent in events)
            {
                if (statRepository.isAdded(currEvent.StartDate, currEvent.AndonId))
                {
                    if (latests.ContainsKey(currEvent.AndonId))
                    {
                        EventDto lastEvent = latests[currEvent.AndonId];
                        setDaily(currEvent, lastEvent);
                        latests[currEvent.AndonId] = currEvent;
                    }
                    else
                    {
                        //TODO: ha itt vagyunk az baj
                    }
                }
                else
                {
                    if (latests.ContainsKey(currEvent.AndonId))
                    {
                        //TODO: baj
                    }
                    else
                    {
                        latests.Add(currEvent.AndonId, currEvent);
                        addDaily(currEvent);
                    }
                }
                //TODO: valamit kezdeni azokkal a lampakkal, amiknek nem volt bejegyzese
            }
        }

        //TODO: befejezni
        private void setDaily(EventDto curr, EventDto last)
        {
            //same day
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
            //not same day, but same month
            else if(curr.StartDate.Year == last.StartDate.Year && curr.StartDate.Month == last.StartDate.Month)
            {
                
            }
            //not same day or month, but same year
            else if(curr.StartDate.Year == last.StartDate.Year)
            {

            }
            //not even in the same year
            else
            {

            }
        }

        private void addDaily(EventDto currEvent)
        {
            int lastState = statRepository.LastState(currEvent.AndonId);
            int minutes = (currEvent.StartDate - new DateTime(currEvent.StartDate.Year,
                                                             currEvent.StartDate.Month,
                                                             currEvent.StartDate.Day)).Minutes;
            StatQueryBuilder builder = new StatQueryBuilder()
                .WithAndon(currEvent.AndonId)
                .WithState(lastState)
                .WithMinutes(minutes)
                .WithCount(1)
                .OnDay(currEvent.StartDate);
            var stat = builder.Build();
            statRepository.AddDayStat(stat);
        }

        //TODO: egesz metodus
        private void createMonthlyStat()
        {
            IEnumerable<EventDto> events = eventRepository.GetEventsAsc();
            Dictionary<int, EventDto> latest = new Dictionary<int, EventDto>();

            StatQueryBuilder builder = new StatQueryBuilder()
                    .WithAndon(events.First().AndonId)
                    .WithState(events.First().StateId)
                    .WithMinutes(0)
                    .WithCount(0);
        }
    }
}
