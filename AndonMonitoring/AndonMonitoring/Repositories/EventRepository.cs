using AndonMonitoring.Data;
using AndonMonitoring.Model;
using AndonMonitoring.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace AndonMonitoring.Repositories
{
    public class EventRepository : IEventRepository
    {
        private AndonDbContext db;

        public EventRepository(AndonDbContext context)
        {
            this.db = context;
        }

        /// <summary>
        /// Retrieves the events from the specified day from the database and converts them to EventDto objects.
        /// </summary>
        /// <param name="day">The day to retrieve the events from.</param>
        /// <returns>Returns a list of EventDto objects if successful, otherwise throws an exception.</returns>
        public List<EventDto> GetEventsFromDay(DateTime day)
        {
            try
            {
                return db.Event
                    .Where(e => e.StartDate.Year == day.Year && e.StartDate.Month == day.Month && e.StartDate.Day == day.Day)
                    .OrderBy(e => e.StartDate)
                    .Select(e => new EventDto(e.Id, e.AndonId, e.StateId, e.StartDate))
                    .ToList();
            }
            catch( Exception )
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves the events from the specified month from the database and converts them to EventDto objects.
        /// </summary>
        /// <param name="month">The month to retrieve the events from.</param>
        /// <returns>Returns a list of EventDto objects if successful, otherwise throws an exception.</returns>
        public List<EventDto> GetEventsFromMonth(DateTime month)
        {
            try
            {
                return db.Event
                    .Where(e => e.StartDate.Year == month.Year && e.StartDate.Month == month.Month)
                    .OrderBy(e => e.StartDate)
                    .Select(e => new EventDto(e.Id, e.AndonId, e.StateId, e.StartDate))
                    .ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves the latest event for the specified andon using its id. Then converts it to an EventDto object.
        /// </summary>
        /// <param name="andonId">The andon's unique identifier</param>
        /// <returns>Returns an EventDto object with the latest event information if successful, otherwise throws an exception.</returns>
        public EventDto GetLatestEvent(int andonId)
        {
            try
            {
                var latestEvent = db.Event.Where(e => e.AndonId == andonId).OrderByDescending(e => e.StartDate).FirstOrDefault();
                if (latestEvent == null)
                    throw new Exception("id doesn't exist");
       
                var eventDto = new EventDto(latestEvent.Id, latestEvent.AndonId, latestEvent.StateId, latestEvent.StartDate);
                return eventDto;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves the latest event of the specified andon using its id
        /// before the specified date from the database, and gets the event's state's id.
        /// </summary>
        /// <param name="andonId">The andon's id which event's state id will be retrieved.</param>
        /// <param name="time">The date to get the event's state id before.</param>
        /// <returns>Returns the id of the event's state if successful, otherwise throws an exception.</returns>
        /// <exception cref="Exception">Throws an exception if there's no previous event found in the database for the given andon.</exception>
        public int GetPreviousState(int andonId, DateTime time)
        {
            try
            {
                var state = db.Event
                    .Where(e => e.AndonId == andonId && e.StartDate < time)
                    .OrderByDescending(e => e.StartDate)
                    .Select(e => e.State)
                    .FirstOrDefault();

                if(state == null)
                {
                    throw new Exception("there's no previous event for the given andon");
                }

                return state.Id;

            }
            catch { throw; }
        }

        /// <summary>
        /// Adds a new event to the database representing a change in an Andon's state.
        /// </summary>
        /// <param name="andonEvent">The EventDto object representing the event to add.</param>
        /// <returns>Returns the ID of the new event that was added.</returns>
        public int AddEvent(EventDto andonEvent)
        {
            try
            {
                var ev = new Event
                {
                    StartDate = andonEvent.StartDate,
                    AndonId = andonEvent.AndonId,
                    StateId = andonEvent.StateId
                };
                db.Event.Add(ev);
                db.SaveChanges();

                return ev.Id;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
