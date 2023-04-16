namespace AndonMonitoring.AndonExceptions
{
    public class AndonFormatException : Exception
    {
        public AndonFormatException() { }

        public AndonFormatException(string message) : base(message) {}
    }
}
