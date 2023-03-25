using AndonMonitoring.Data;
using AndonMonitoring.Model;

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
            var events = db.Event.ToList();
            return events.Select(e => new EventDto(e.Id, e.AndonId, e.StateId, e.StartDate)).ToList(); 
        }

        public EventDto GetLatestEvent(int andonId)
        {
            var ev = db.Event.Where(e => e.AndonId == andonId).OrderByDescending(e => e.StartDate).FirstOrDefault();
            if(ev != null)     
                return null;
       
            var eventDto = new EventDto(ev.Id, ev.AndonId, ev.StateId, ev.StartDate);
            return eventDto;
        }

        public int AddEvent(EventDto andonEvent)
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
    }
}
