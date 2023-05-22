using AndonMonitoring.Data;
using AndonMonitoring.Data.Interface;
using AndonMonitoring.Repositories;
using AndonMonitoring.Repositories.Interface;
using AndonMonitoring.Services;
using AndonMonitoring.Services.Interfaces;
using AndonMonitoring.TimedService;
using Microsoft.EntityFrameworkCore;

namespace AndonMonitoring
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string conn;
            //Add connection string
            if(Environment.GetEnvironmentVariable("PGL__HOST")==null)
            {
                conn = builder.Configuration.GetConnectionString("DefaultConnection");
            }
            else
            {
                string host = Environment.GetEnvironmentVariable("PGQL__HOST");
                string port = Environment.GetEnvironmentVariable("PGQL__PORT");
                string user = Environment.GetEnvironmentVariable("PGQL__USER");
                string password = Environment.GetEnvironmentVariable("PGQL__PW");
                string database = Environment.GetEnvironmentVariable("PGQL__DB");
                conn = $"UserID={user};Password={password};Host={host};Port={port};Database={database}";
            }

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

            //timed stat service

            builder.Services.AddHostedService<TimedHostedStatService>();
            builder.Services.AddScoped<IScopedStatService, ScopedStatService>();

            //Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            //Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}