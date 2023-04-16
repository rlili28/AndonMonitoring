using AndonMonitoring.Data;

namespace AndonMonitoring.Repositories.Interface
{
    public interface IEventRepository
    {
        public List<EventDto> GetEvents();
        public List<EventDto> GetEventsAsc();
        public EventDto GetLatestEvent(int andonId);
        public int AddEvent(EventDto andonEvent);
    }
}
