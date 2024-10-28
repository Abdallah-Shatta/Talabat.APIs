using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Mapping_Profile;
using Talabat.APIs.Middlewares;
using Talabat.Core.IRepositories;
using Talabat.Respository.Repositories;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Dynamically Register the service of the injected GenericRepo with its IGenericRepo
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Register the AutoMapper Profile
            // builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile())); => Readable Way
            services.AddAutoMapper(typeof(MappingProfile)); // => Easy Way

            // Handling Validation Error Response by configuring the api behavior options by its InvalidModelStateResponseFactory
            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                                   .SelectMany(p => p.Value.Errors)
                                                   .Select(e => e.ErrorMessage)
                                                   .ToList();

                    var validationResponse = new ApiValidationErrorResponse() { Errors = errors };

                    return new BadRequestObjectResult(validationResponse);
                };
            });

            // Registering the service of my custom middleware (ExceptionMiddleware)
            services.AddTransient<ExceptionMiddleware>();

            return services;
        }
    }
}
