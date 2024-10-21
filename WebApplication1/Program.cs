using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApplication1.Models;
using WebApplication1.Models.Interfaces;
using WebApplication1.Profiles;
using WebApplication1.Repositories;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Services;
using WebApplication1.Services.Interfaces;

namespace WebApplication1
{
    public class Program
    {
        private const string connectionStringName = "RolesDB";

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

            // repositories
            builder.Services.AddTransient<IRepository<User>, Repository<User>>();
            builder.Services.AddTransient<IRepository<Role>, Repository<Role>>();
            builder.Services.AddTransient<IRepository<Permission>, Repository<Permission>>();
            builder.Services.AddTransient<IUserRoleRepository, UserRoleRepository>();
            builder.Services.AddTransient<IRepository<RolePermission>, Repository<RolePermission>>();
            builder.Services.AddTransient<IRepository<Authentication>, Repository<Authentication>>();

            // services
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IRoleService, RoleService>();
            builder.Services.AddTransient<IPermissionService, PermissionService>();
            builder.Services.AddTransient<IUserRoleService, UserRoleService>();
            builder.Services.AddTransient<IRolePermissionService, RolePermissionService>();
            builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();

            builder.Services.AddSingleton(new MapperConfiguration(configuration =>
            {
                configuration.AddProfile(new UserProfile());
                configuration.AddProfile(new RoleProfile());
                configuration.AddProfile(new PermissionProfile());
                configuration.AddProfile(new UserRoleProfile());
                configuration.AddProfile(new RolePermissionProfile());
                configuration.AddProfile(new AuthenticationProfile());
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