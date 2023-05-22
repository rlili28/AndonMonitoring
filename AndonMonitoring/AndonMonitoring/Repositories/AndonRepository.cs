using AndonMonitoring.Data;
using AndonMonitoring.Model;
using AndonMonitoring.Repositories.Interface;

namespace AndonMonitoring.Repositories
{
    public class AndonRepository : IAndonRepository
    {
        AndonDbContext db;
        
        public AndonRepository(AndonDbContext context)
        {
            db = context;
        }

        /// <summary>
        /// Retrieves the Andon with the specified id from the database and returns it as an AndonDto object.
        /// </summary>
        /// <param name="id">The unique id of the Andon to retrieve.</param>
        /// <returns>Returns the Andon as an AndonDto object if it exists in the database, otherwise returns null.</returns>
        public AndonDto GetAndon(int id)
        {
            try
            {
                var andon = db.Andon.FirstOrDefault(a => a.Id == id);
                if (andon == null) { return null; }
                var dto = new AndonDto(andon.Id, andon.Name, andon.CreatedDate);
                return dto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves all Andons from the database.
        /// </summary>
        /// <returns>Returns the list of all andons as AndonDto objects from the database.</returns>
        public List<int> GetAndonIds()
        {
            try
            {
                return db.Andon
                    .Select(a => a.Id)
                    .ToList();
            }
            catch { throw; }
        }

        /// <summary>
        /// Adds the received Andon to the database.
        /// </summary>
        /// <param name="light">The Andon to add to the database.</param>
        /// <returns>Returns the id of the newly added andon to the database.</returns>
        public int AddAndon(AndonDto light)
        {
            try
            {
                var andon = new Andon
                {
                    Name = light.Name,
                    CreatedDate = light.CreatedTime.ToUniversalTime()
                };

                db.Andon.Add(andon);
                db.SaveChanges();

                return andon.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes the Andon with the specified id from the database.
        /// </summary>
        /// <param name="id">The id of the Andon to delete.</param>
        public void DeleteAndon(int id)
        {
            try
            {
                var andon = db.Andon.FirstOrDefault(a => a.Id == id);
                if (andon != null)
                {
                    db.Andon.Remove(andon);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Checks if the Andon with the specified id exists in the database.
        /// If it does, updates the Andon with the specified id in the database.
        /// If it doesn't, does nothing. //TODO nem kene semmit se csinalnia
        /// </summary>
        /// <param name="light"></param>
        public void UpdateAndon(AndonDto light)
        {
            try
            {
                var andon = db.Andon.FirstOrDefault(a => a.Id == light.Id);
                if (andon != null)
                {
                    andon.Name = light.Name;
                    andon.CreatedDate = light.CreatedTime.ToUniversalTime();
                    db.SaveChanges();
                }
                else
                {
                    //TODO valamit kene csinalni, ha az update-elni kivant andon nem letezik
                }
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
