using AndonMonitoring.Data;
using AndonMonitoring.Model;

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
            var andon = db.Andon.FirstOrDefault(a => a.Id == id);
            if(andon == null) { return null; }
            var dto = new AndonDto(andon.Id, andon.Name, andon.CreatedDate);
            return dto;
        }

        public int AddAndon(AndonDto light)
        {
            var andon = new Andon
            {
                Name = light.Name,
                CreatedDate = light.CreatedTime
            };
            db.Andon.Add(andon);
            db.SaveChanges();
            return andon.Id;
        }

        public void DeleteAndon(int id)
        {
            var andon = db.Andon.Find(id);
            if (andon != null)
            {
                db.Andon.Remove(andon);
                db.SaveChanges();
            }
        }

        public void UpdateAndon(AndonDto light)
        {
            var andon = db.Andon.Find(light.Id);
            if(andon != null)
            {
                andon.Name = light.Name;
                andon.CreatedDate = light.CreatedTime;
                db.SaveChanges();
            }
        }
    }
}
