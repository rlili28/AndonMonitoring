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

        public List<EventDto> GetEvents()
        {
            try
            {
                var events = db.Event.ToList();
                return events.Select(e => new EventDto(e.Id, e.AndonId, e.StateId, e.StartDate)).ToList();
            }
            catch(Exception)
            {
                throw;
            }
        }

        public List<EventDto> GetEventsAsc()
        {
            try
            {
                var events = db.Event
                    .OrderBy(e => e.StartDate)
                    .ToList();
                return events.Select(e => new EventDto(e.Id, e.AndonId, e.StateId, e.StartDate.ToLocalTime())).ToList();

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
                if(latestEvent == null)     
                    return null;
       
                var eventDto = new EventDto(latestEvent.Id, latestEvent.AndonId, latestEvent.StateId, latestEvent.StartDate);
                return eventDto;

            }
            catch (Exception)
            {
                throw;
            }
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

        public void deleteEvents()
        {
            try
            {
                var lastEvents = db.Event
                    .GroupBy(e => e.AndonId)
                    .Select(g => g.OrderByDescending(e => e.StartDate).FirstOrDefault())
                    .ToList();

                var sql = "DELETE FROM Event";
                db.Database.ExecuteSqlRaw(sql);

                foreach(Event e in lastEvents)
                {
                    db.Event.Add(e);
                }

                db.SaveChanges();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
