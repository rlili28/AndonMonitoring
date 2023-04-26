using AndonMonitoring.Data;

namespace AndonMonitoring.Services.Interfaces
{
    public interface IStateService
    {
        public StateDto GetState(int id);
        public int AddState(StateDto state);
    }
}
