namespace AndonMonitoring.Data
{
    public class EventDto
    {
        public int Id { get; set; }
        public int AndonId { get; set; }
        public int StateId { get; set; }
        public DateTime StartDate { get; set; }

        public EventDto(int id, int andonId, int stateId, DateTime startDate)
        {
            Id = id;
            AndonId = andonId;
            StateId = stateId;
            StartDate = startDate;
        }

        public EventDto(int andonId, int stateId, DateTime startDate)
        {
            AndonId = andonId;
            StateId = stateId;
            StartDate = startDate;
        }

        public string ToString()
        {
            return $"event id: {Id}, andon id: {AndonId}, state id: {StateId}, start date: {StartDate.ToString()}";
        }
    }
}
