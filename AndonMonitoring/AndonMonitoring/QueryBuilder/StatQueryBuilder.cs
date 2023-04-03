namespace AndonMonitoring.QueryBuilder
{
    public class StatQueryBuilder
    {
        private readonly StatQuery query;

        public StatQueryBuilder()
        {
            this.query = new StatQuery();
        }

        public StatQueryBuilder WithAndon(int id)
        {
            query.AndonId = id;
            return this;
        }

        public StatQueryBuilder WithState(int id)
        {
            query.StateId = id;
            return this;
        }

        public StatQueryBuilder OnDay(DateTime day)
        {
            query.Day = day;
            return this;
        }

        public StatQueryBuilder OnMonth(DateTime month)
        {
            query.Month = month;
            return this;
        }

        public StatQueryBuilder WithCount(int count)
        {
            query.Count = count;
            return this;
        }

        public StatQueryBuilder WithMinutes(int minutes)
        {
            query.Minutes = minutes;
            return this;
        }

        public StatQuery Build()
        {
            return this.query;
        }
    }
}
