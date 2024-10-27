using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Mapping_Profile;
using Talabat.Core.IRepositories;
using Talabat.Respository.Data;
using Talabat.Respository.Repositories;

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

            // Dynamically Register the service of the injected GenericRepo with its IGenericRepo
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Register the AutoMapper Profile
            //builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile())); => Readable Way
            builder.Services.AddAutoMapper(typeof(MappingProfile)); // => Easy Way

            #endregion

            var app = builder.Build();

            #region Automatically Updating Database
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            // Explicitly asking an object of DbContext from the CLR using a scope of the configured services
            var dbContext = services.GetRequiredService<StoreDbContext>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await dbContext.Database.MigrateAsync();
                await StoreDataSeed.SeedAsync(dbContext);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "Error Updating Database");
            }
            #endregion

            #region Configure Middlewares
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
