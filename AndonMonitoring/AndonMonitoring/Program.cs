using AndonMonitoring.Data;
using AndonMonitoring.Repositories;
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
            builder.Services.AddScoped<IAndonRepository, AndonRepository>();

            var options = new DbContextOptionsBuilder<AndonDbContext>();
            options.UseNpgsql(conn);
            var db = new AndonDbContext(options.Options);
            var state = new StateRepository(db);
            int returnId = state.AddState(new StateDto(2, "detected"));
            int returnId2 = state.AddState(new StateDto(3, "problem"));
            int returnId3 = state.AddState(new StateDto(4, "deffective"));
            Console.WriteLine("db adding called, return ID: " + returnId);

            // Add services to the container.

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