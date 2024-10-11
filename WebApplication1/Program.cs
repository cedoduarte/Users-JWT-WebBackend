using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApplication1.Models;
using WebApplication1.Profiles;
using WebApplication1.Repositories;
using WebApplication1.Services;

namespace WebApplication1
{
    public class Program
    {
        private const string connectionStringName = "ExampleRolePermissions";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            builder.Services.AddSingleton<IConfiguration>(options =>
            {
                var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Local.json", true)
                .AddEnvironmentVariables();
                return configurationBuilder.Build();
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();

            builder.Services.AddSingleton(new MapperConfiguration(configuration =>
            {
                configuration.AddProfile(new UserProfile());
            }).CreateMapper());

            builder.Services.AddDbContext<IAppDbContext, AppDbContext>(options =>
            {
                string dbConnectionString = builder.Configuration.GetConnectionString(connectionStringName)!;
                options.UseSqlServer(dbConnectionString, dbOptions =>
                {
                    dbOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name);
                });
            });

            var app = builder.Build();
            app.UseCors(options =>
            {
                options.AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowAnyOrigin();
            });
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