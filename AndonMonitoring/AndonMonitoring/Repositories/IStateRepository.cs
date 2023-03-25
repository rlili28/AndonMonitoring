using AndonMonitoring.Data;

namespace AndonMonitoring.Repositories
{
    public interface IStateRepository
    {
        public int AddState(StateDto state);
    }
}
