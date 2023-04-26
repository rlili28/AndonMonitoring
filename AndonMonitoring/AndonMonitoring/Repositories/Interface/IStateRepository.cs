using AndonMonitoring.Data;

namespace AndonMonitoring.Repositories.Interface
{
    public interface IStateRepository
    {
        public StateDto GetState(int id);
        public int AddState(StateDto state);
    }
}
