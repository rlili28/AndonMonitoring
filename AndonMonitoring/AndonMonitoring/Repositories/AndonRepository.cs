using AndonMonitoring.Data;

namespace AndonMonitoring.Repositories
{
    public class AndonRepository : IAndonRepository
    {
        AndonDbContext _context;
        
        public AndonRepository(AndonDbContext context)
        {
            _context = context;
        }
    }
}
