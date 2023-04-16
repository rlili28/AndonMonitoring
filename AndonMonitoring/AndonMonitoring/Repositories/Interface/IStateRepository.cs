using AndonMonitoring.Data;

namespace AndonMonitoring.Repositories.Interface
{
    public interface IStateRepository
    {
        public int AddState(StateDto state);
    }
}
