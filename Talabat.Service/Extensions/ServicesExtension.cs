using Microsoft.Extensions.DependencyInjection;
using Talabat.Core.IServices;

namespace Talabat.Service.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddServiceLayerServices(this IServiceCollection services)
        {
            // Register to the business logic services
            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped(typeof(IProductService), typeof(ProductService));
            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            
            // Register to the AuthService for JWT
            services.AddScoped(typeof(IAuthService), typeof(AuthService));

            return services;
        }
    }
}
