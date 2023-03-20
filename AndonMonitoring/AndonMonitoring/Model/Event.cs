namespace AndonMonitoring.Model
{
    public class Event
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public Andon Andon { get; set; }
        public State State { get; set; }
    }
}
