using AndonMonitoring.Data;
using Microsoft.EntityFrameworkCore;

namespace AndonMonitoring
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Add connection
            var conn = builder.Configuration.GetConnectionString("DefaultConnection");
            //Add dbcontext
            builder.Services.AddDbContext<AndonDbContext>(options =>
                    options.UseNpgsql(conn));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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