using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Extensions;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities.Identity;
using Talabat.Respository.Data;
using Talabat.Respository.Identity;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register the injected DbContext to the service container
            builder.Services.AddDbContext<StoreDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConStr")));

            // Register the injected IdentityDbContext to the service container
            builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConStr")));

            // Register the redis server database
            builder.Services.AddSingleton((Func<IServiceProvider, IConnectionMultiplexer>)(serviceProvider =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            }));

            // Registering the app Services from extension method
            builder.Services.AddApplicationServices();

            // Add Identity Configurations
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                            .AddEntityFrameworkStores<AppIdentityDbContext>();
            #endregion

            var app = builder.Build();

            #region Automatically Updating Database
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            // Explicitly asking an object of DbContext from the CLR using a scope of the configured services
            var dbContext = services.GetRequiredService<StoreDbContext>();
            var identityDbContext = services.GetRequiredService<AppIdentityDbContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await dbContext.Database.MigrateAsync();
                await StoreDataSeed.SeedAsync(dbContext);

                await identityDbContext.Database.MigrateAsync();
                await AppIdentityDbContextSeed.SeedAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "Error Updating Database");
            }
            #endregion

            #region Configure Middlewares
            // Handling Server Error (Exceptions)
            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Handling Not Found Endpoint Middleware
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            //app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
