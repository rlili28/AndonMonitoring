using AndonMonitoring.Data.Interface;
using AndonMonitoring.Model;
using Microsoft.EntityFrameworkCore;

namespace AndonMonitoring.Data
{
    public class AndonDbContext : DbContext, IAndonDbContext
    {
        public AndonDbContext(DbContextOptions<AndonDbContext> options) : base(options) {}

        public DbSet<Andon> Andon { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<DayStat> DayStat { get; set; }
        public DbSet<MonthStat> MonthStat { get; set; }
    }
}
