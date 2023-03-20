using AndonMonitoring.Data;

namespace AndonMonitoring.Repositories
{
    public interface IEventRepository
    {
        public List<EventDTO> GetEvents();
        public EventDTO GetLatestEvent(int andonId);
        public int AddEvent(EventDTO andonEvent);
    }
}
