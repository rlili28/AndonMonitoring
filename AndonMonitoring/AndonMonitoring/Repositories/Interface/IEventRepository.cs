using AndonMonitoring.Data;

namespace AndonMonitoring.Repositories.Interface
{
    public interface IEventRepository
    {
        public List<EventDto> GetEventsFromDay(DateTime day);
        public List<EventDto> GetEventsFromMonth(DateTime month);
        public int GetPreviousState(int andonId, DateTime day);
        public EventDto GetLatestEvent(int andonId);
        public int AddEvent(EventDto andonEvent);
    }
}
