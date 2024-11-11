using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Mapping_Profile;
using Talabat.APIs.Middlewares;
using Talabat.Core.IRepositories;
using Talabat.Core.IUnitOfWork;
using Talabat.Respository.Repositories;
using Talabat.Respository.UnitOfWork;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            /// Dynamically Register the service of the injected GenericRepo with its IGenericRepo
            /// services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            /// After Implementing the unit of work i don't need to register the generic repo service
            /// because it will be injected in a higher layer (Unit of work)

            // Register to the unit of work service
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            // Register to BasketRepository service
            services.AddScoped<IBasketRepository, BasketRepository>();

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
