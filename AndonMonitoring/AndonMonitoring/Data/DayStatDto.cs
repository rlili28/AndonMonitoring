namespace AndonMonitoring.Data
{
    public class DayStatDto
    {
        public int Id { get; set; }
        public int AndonId { get; set; }
        public int StateId { get; set; }
        public DateTime Day { get; set; }
        public int OverallMinutes { get; set; }
        public int OverallCount { get; set; }

        //constructor
        public DayStatDto(int id, int andonId, int stateId, DateTime day, int overallMinutes, int overallCount)
        {
            Id = id;
            AndonId = andonId;
            StateId = stateId;
            Day = day;
            OverallMinutes = overallMinutes;
            OverallCount = overallCount;
        }
    }
}
