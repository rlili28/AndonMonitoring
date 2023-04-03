namespace AndonMonitoring.QueryResult
{
    public class QueryResult<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public QueryResult()
        {

        }
    }
}
