using AndonMonitoring.AndonExceptions;
using System.Text.Json.Serialization;

namespace AndonMonitoring.Data
{
    public class AndonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedTime { get; set; }

        public AndonDto() { }

        public AndonDto(int id, string name, DateTime created)
        {
            Id = id;
            Name = name;
            CreatedTime = created;
        }

        public AndonDto(int id, string name, int year, int month)
        {
            Id = id;
            Name = name;
            CreatedTime = new DateTime(year, month, 1);
        }

        public AndonDto(int id, string name, int year, int month, int day)
        {
            Id = id;
            Name = name;
            CreatedTime = new DateTime(year, month, day);
        }

        public bool isAddFormat()
        {
            if (string.IsNullOrEmpty(Name))
                throw new AndonFormatException("name for andon was not provided");
            if (CreatedTime > DateTime.Now)
                throw new AndonFormatException("andon created time is in the future");
            return true;
        }

        public bool isUpdateFormat()
        {
            if (Id < 0)
                throw new AndonFormatException("provided andon object's id is negative (which it can't be)");
            if (string.IsNullOrEmpty(Name))
                throw new AndonFormatException("name for andon was not provided");
            if (CreatedTime > DateTime.Now)
                throw new AndonFormatException("andon created time is in the future");
            return true;
        }
    }
}
