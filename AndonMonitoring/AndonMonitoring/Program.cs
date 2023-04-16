using AndonMonitoring.Data;
using AndonMonitoring.Data.Interface;
using AndonMonitoring.QueryBuilder;
using AndonMonitoring.Repositories;
using AndonMonitoring.Repositories.Interface;
using AndonMonitoring.Services;
using AndonMonitoring.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;

namespace AndonMonitoring
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Add connection string
            var conn = builder.Configuration.GetConnectionString("DefaultConnection");

            //Add dbcontext
            builder.Services.AddDbContext<AndonDbContext>(options =>
                    options.UseNpgsql(conn));

            builder.Services.AddScoped<IAndonDbContext, AndonDbContext>();
            builder.Services.AddScoped<IAndonService, AndonService>();
            builder.Services.AddScoped<IStateService, StateService>();
            builder.Services.AddScoped<IStatsService, StatsService>();
            builder.Services.AddScoped<IAndonRepository, AndonRepository>();
            builder.Services.AddScoped<IEventRepository, EventRepository>();
            builder.Services.AddScoped<IStateRepository, StateRepository>();
            builder.Services.AddScoped<IStatRepository, StatRepository>();

            var options = new DbContextOptionsBuilder<AndonDbContext>();
            options.UseNpgsql(conn);

            var db = new AndonDbContext(options.Options);
            var andonRepo = new AndonRepository(db);
            var statRepo = new StatRepository(db);

            StatQueryBuilder queryBuilder = new StatQueryBuilder();
            StatQuery query = queryBuilder.OnDay(new DateTime(2023, 4, 8))
                 .WithAndon(2)
                 .WithState(3)
                 .Build();
            int count = statRepo.GetAndonStateCountByDay(query);
            int minutes = statRepo.GetAndonStateMinutesByDay(query);
            Console.WriteLine("count: " + count + ", minutes: " + minutes);

            bool isToday = statRepo.isAdded(new DateTime(2023,4,9));
            if(isToday) { Console.WriteLine("today was already added"); }
            else { Console.WriteLine("today was not yet added"); }

            // Add services to the container.

            //builder.Services.AddControllers();
            //// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            var app = builder.Build();

            //Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            //app.UseHttpsRedirection();

            //app.UseAuthorization();

            //app.MapControllers();

            app.Run();
        }
    }
}