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

        public AndonService(IAndonRepository andonRepository, IEventRepository eventRepository)
        {
            this.andonRepository = andonRepository;
            this.eventRepository = eventRepository;
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
                throw new Exception("delete didn't work for some reason?");
            }
        }

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
                throw new AndonFormatException("no such andon exists in the database, probably an andon light with the specified id doesn't exist");
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

        public EventDto ChangeState(int andonId, int stateId)
        {
            EventDto newEvent = new EventDto(andonId, stateId, DateTime.Now);

            int newEventId = -1;
            try
            {
                newEventId = eventRepository.AddEvent(newEvent);
            }
            catch (Exception)
            {
                throw;
            }

            if (newEventId == -1)
                throw new Exception();  //TODO

            newEvent.Id = newEventId;
            return newEvent;

        }

        //which state the specified light is in currently?
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
                throw new AndonFormatException("light with specified id wasn't found");

            int state = latestEvent.StateId;
            //TODO: itt a state id-jat kapom csak meg az eventDtoban. De nekem StateDto kene, szoval talan kene az EventRepository-ban egy olyan metodus erre ami StateDtoval ter vissza.
            throw new NotImplementedException();
        }

        public List<int> GetAndonIds()
        {
            try
            {
                return andonRepository.GetAndonIds();
            } catch(Exception) { throw; }
        }
    }
}
