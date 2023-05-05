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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //ANDON
            //modelBuilder.Entity<Andon>()
            //    .Property(a => a.CreatedDate)
            //    .HasColumnType("timestamp");

            //EVENT
            modelBuilder.Entity<Event>()
                .Property(e => e.StartDate)
                .HasColumnType("timestamp");

            //STATS
            modelBuilder.Entity<DayStat>()
                .Property(s => s.Day)
                .HasColumnType("date");

            modelBuilder.Entity<MonthStat>()
                .Property(s => s.Month)
                .HasColumnType("date");
        }
    }
}
