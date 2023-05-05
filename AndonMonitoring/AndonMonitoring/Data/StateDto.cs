namespace AndonMonitoring.Data
{
    public class StateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public StateDto()
        {
            
        }

        public StateDto(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
} 
