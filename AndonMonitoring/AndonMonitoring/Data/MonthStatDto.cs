namespace AndonMonitoring.Data
{
    public class MonthStatDto
    {
        public int Id { get; set; }
        public int AndonId { get; set; }
        public int StateId { get; set; }
        public DateTime Month { get; set; }
        public int OverallMinutes { get; set; }

        public MonthStatDto(int id, int andonId, int stateId, DateTime month, int overallMinutes)
        {
            Id = id;
            AndonId = andonId;
            StateId = stateId;
            Month = month;
            OverallMinutes = overallMinutes;
        }
    }
}
