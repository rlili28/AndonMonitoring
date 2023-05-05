using AndonMonitoring.Data;
using AndonMonitoring.Model;
using AndonMonitoring.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace AndonMonitoring.Repositories
{
    public class EventRepository : IEventRepository
    {
        private AndonDbContext db;

        public EventRepository(AndonDbContext context)
        {
            this.db = context;
        }

        public List<EventDto> GetEventsFromDay(DateTime day)
        {
            try
            {
                return db.Event
                    .Where(e => e.StartDate.Year == day.Year && e.StartDate.Month == day.Month && e.StartDate.Day == day.Day)
                    .OrderBy(e => e.StartDate)
                    .Select(e => new EventDto(e.Id, e.AndonId, e.StateId, e.StartDate))
                    .ToList();
            }
            catch( Exception )
            {
                throw;
            }
        }

        public List<EventDto> GetEventsFromMonth(DateTime month)
        {
            try
            {
                return db.Event
                    .Where(e => e.StartDate.Year == month.Year && e.StartDate.Month == month.Month)
                    .OrderBy(e => e.StartDate)
                    .Select(e => new EventDto(e.Id, e.AndonId, e.StateId, e.StartDate))
                    .ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public EventDto GetLatestEvent(int andonId)
        {
            try
            {
                var latestEvent = db.Event.Where(e => e.AndonId == andonId).OrderByDescending(e => e.StartDate).FirstOrDefault();
                if (latestEvent == null)
                    throw new Exception("id doesn't exist");
       
                var eventDto = new EventDto(latestEvent.Id, latestEvent.AndonId, latestEvent.StateId, latestEvent.StartDate);
                return eventDto;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetPreviousState(int andonId, DateTime time)
        {
            try
            {
                var state = db.Event
                    .Where(e => e.AndonId == andonId && e.StartDate < time)
                    .OrderByDescending(e => e.StartDate)
                    .Select(e => e.State)
                    .FirstOrDefault();

                if(state == null)
                {
                    throw new Exception("there's no previous event for the given andon");
                }

                return state.Id;

            }
            catch { throw; }
        }

        public int AddEvent(EventDto andonEvent)
        {
            try
            {
                var ev = new Event
                {
                    StartDate = andonEvent.StartDate,
                    AndonId = andonEvent.AndonId,
                    StateId = andonEvent.StateId
                };
                db.Event.Add(ev);
                db.SaveChanges();

                return ev.Id;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
