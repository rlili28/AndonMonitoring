using AndonMonitoring.Data;

namespace AndonMonitoring.Repositories
{
    public interface IEventRepository
    {
        public List<EventDto> GetEvents();
        public EventDto GetLatestEvent(int andonId);
        public int AddEvent(EventDto andonEvent);
    }
}
