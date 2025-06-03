
using Data;
using Microsoft.EntityFrameworkCore;
using Repository;
using Services;

namespace BookManagement_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ProgramDbContent>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            // Add services to the container.
            builder.Services.AddTransient<Bookservices>();
            builder.Services.AddTransient<SpBookService>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ProgramDbContent>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var allowedOrigins = builder.Configuration.GetValue<string>("allowedOrigins");
            // Allowing the request from different origin 
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            
            app.Run();
        }
    }
}
