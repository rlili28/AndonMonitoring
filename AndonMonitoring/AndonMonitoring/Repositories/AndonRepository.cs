using AndonMonitoring.Data;
using AndonMonitoring.Model;
using AndonMonitoring.Repositories.Interface;
using AndonMonitoring.AndonExceptions;
using System.Xml;

namespace AndonMonitoring.Repositories
{
    public class AndonRepository : IAndonRepository
    {
        AndonDbContext db;
        
        public AndonRepository(AndonDbContext context)
        {
            db = context;
        }

        public AndonDto GetAndon(int id)
        {
            if (id <= 0)
                return null;
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
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
