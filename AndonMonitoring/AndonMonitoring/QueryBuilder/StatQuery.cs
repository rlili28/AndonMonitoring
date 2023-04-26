using AndonMonitoring.AndonExceptions;

namespace AndonMonitoring.QueryBuilder
{
    public class StatQuery
    {
        public int Id = -1;
        public int AndonId = -1;
        public int StateId = -1;
        public DateTime Day;
        public DateTime Month;
        public int Count = -1;
        public int Minutes = -1;


        //before adding, getting
        public bool isDayFormat()
        {
            if (AndonId == -1 || AndonId < 0)
                throw new AndonFormatException("andon light id wasn't specified");
            if (StateId == -1 || StateId < 0)
                throw new AndonFormatException("state id wasn't provided");
            if (Day > DateTime.Now)
                throw new AndonFormatException("specified day is not right");
            if(Day == new DateTime())
                throw new AndonFormatException("specified day is not right");
            return true;
        }

        //before adding and getting
        public bool isMonthFormat()
        {
            if (AndonId == -1)
            {
                throw new AndonFormatException("andon light id wasn't specified");
            }
            if (StateId == -1)
            {
                throw new AndonFormatException("state id wasn't provided");
            }
            //if date is in a future month
            if (Month.Month > DateTime.Now.Month && Month.Year >= DateTime.Now.Year)
                throw new AndonFormatException("specified month is not right");
            if (Month == new DateTime())
                throw new AndonFormatException("specified month is not right");
            return true;
        }

        public bool isSetFormat()
        {
            if (Id == -1 || Id < 0 || AndonId == -1 || StateId == -1 || Count == -1 || Minutes == -1)
                throw new AndonFormatException("Stat id was not provided for setting");
            return true;
        }
    }
}
