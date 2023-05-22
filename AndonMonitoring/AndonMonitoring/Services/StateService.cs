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

        /// <summary>
        /// Retrieves a StateDto object with information on the state identified by the specified ID.
        /// </summary>
        /// <param name="id">The ID of the state to retrieve</param>
        /// <returns>A StateDto object with the state information if successful, otherwise an exception is thrown</returns>
        public StateDto GetState(int id)
        {
            try
            {
                return stateRepository.GetState(id);
            }
            catch { throw; }
        }

        /// <summary>
        /// Add a new state item.
        /// </summary>
        /// <param name="state">The StateDto object representing the state to be added.</param>
        /// <returns>Returns the ID of the added state.</returns>
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
