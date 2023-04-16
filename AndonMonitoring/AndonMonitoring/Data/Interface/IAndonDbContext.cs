using AndonMonitoring.Model;
using Microsoft.EntityFrameworkCore;

namespace AndonMonitoring.Data.Interface
{
    public interface IAndonDbContext
    {
        DbSet<Andon> Andon { get; set; }
        DbSet<State> State { get; set; }
        DbSet<Event> Event { get; set; }
        DbSet<DayStat> DayStat { get; set; }
        DbSet<MonthStat> MonthStat { get; set; }
    }
}
