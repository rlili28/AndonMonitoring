using AndonMonitoring.Data;
using AndonMonitoring.Repositories.Interface;
using AndonMonitoring.Services.Interfaces;
using AndonMonitoring.AndonExceptions;

namespace AndonMonitoring.Services
{
    public class AndonService : IAndonService
    {
        private readonly IAndonRepository andonRepository;
        private readonly IEventRepository eventRepository;
        private readonly IStateService stateService;

        public AndonService(IAndonRepository andonR, IEventRepository eventR, IStateService stateS)
        {
            andonRepository = andonR;
            eventRepository = eventR;
            stateService = stateS;
        }

        public int AddAndon(AndonDto andonLight)
        {
            try
            {
                int newId;
                if (!andonLight.isAddFormat())
                    throw new AndonFormatException("params are not right for adding a new andon light");
                newId = andonRepository.AddAndon(andonLight);
                //this way every andon has at least one event (for stats)
                eventRepository.AddEvent(new EventDto(newId, 1, andonLight.CreatedTime));

                return newId;
            }
            catch (Exception)
            {
                throw;
            }

            
        }

        /// <summary>
        /// Deletes the andon with the specified andonId.
        /// </summary>
        /// <param name="andonId">The id of the Andon to delete.</param>
        /// <exception cref="Exception">Thrown after delete, if the item is still available in the database</exception>
        public void DeleteAndon(int andonId)
        {
            try
            {
                andonRepository.DeleteAndon(andonId);
            }
            catch(Exception)
            {
                throw;
            }

            if(andonRepository.GetAndon(andonId) != null)
            {
                throw new Exception("delete didn't work");
            }
        }

        /// <summary>
        /// Retrieves an AndonDto object for the specified andonId.
        /// </summary>
        /// <param name="andonId">The unique identifier for the Andon.</param>
        /// <returns>Returns the AndonDto object if it exists, otherwise throws an AndonFormatException.</returns>
        public AndonDto GetAndon(int andonId)
        {
            AndonDto andon;

            try
            {
                andon = andonRepository.GetAndon(andonId);
            }
            catch(Exception)
            {
                throw;
            }

            if(andon == null)
            {
                throw new AndonFormatException("no such andon exists in the database");
            }

            return andon;
        }

        public void UpdateAndon(AndonDto andonLight)
        {
            try
            {
                andonLight.isUpdateFormat();
                andonRepository.UpdateAndon(andonLight);
            }
            catch(Exception) { throw; }
        }

        /// <summary>
        /// Updates the state of the andon to the provided state ID by calling the method to add a new event.
        /// </summary>
        /// <param name="andonId">The ID of the Andon to update.</param>
        /// <param name="stateId">The ID of the new state to set.</param>
        /// <returns>Returns the EventDTO object that was added by updating the andon's state</returns>
        public EventDto ChangeState(int andonId, int stateId)
        {
            EventDto newEvent = new EventDto(andonId, stateId, DateTime.Now);

            int newEventId;
            try
            {
                newEventId = eventRepository.AddEvent(newEvent);
            }
            catch (Exception)
            {
                throw;
            }

            newEvent.Id = newEventId;
            return newEvent;

        }

        /// <summary>
        /// Retrieves the latest event for the specified andon using its id. Then retrieves the state information for that event.
        /// </summary>
        /// <param name="andonId">The andon's unique identifier</param>
        /// <returns>Returns a StateDto object with the andon's current state information if successful, otherwise throws an exception.</returns>
        public StateDto GetState(int andonId)
        {
            EventDto latestEvent;
            try
            {
                latestEvent = eventRepository.GetLatestEvent(andonId);
            }
            catch(Exception)
            {
                throw;
            }

            if (latestEvent == null)
                throw new AndonFormatException("light with specified id doesn't exist");

            int stateId = latestEvent.StateId;

            return stateService.GetState(stateId);
        }

        public List<int> GetAndonIds()
        {
            try
            {
                return andonRepository.GetAndonIds();
            }
            catch(Exception) { throw; }
        }
    }
}
