using AndonMonitoring.Data;
using AndonMonitoring.Repositories.Interface;
using AndonMonitoring.Services.Interfaces;

namespace AndonMonitoring.Services
{
    public class StateService : IStateService
    {
        private readonly IStateRepository stateRepository;

        public StateService(IStateRepository state)
        {
            stateRepository = state;
        }
        public StateDto GetState(int id)
        {
            try
            {
                return stateRepository.GetState(id);
            }
            catch { throw; }
        }

        public int AddState(StateDto state)
        {
            try
            {
                return stateRepository.AddState(state);
            }
            catch { throw; }
        }
    }
}
