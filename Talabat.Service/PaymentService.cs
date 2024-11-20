using Microsoft.Extensions.Configuration;
using Talabat.Core.Entities;
using Talabat.Core.IRepositories;
using Talabat.Core.IServices;
using Talabat.Core.IUnitOfWork;
using Stripe;
using Product = Talabat.Core.Entities.Product;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Core.Specifications;

namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration, IBasketRepository basketRepo, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];

            var basket = await _basketRepo.GetBasketAsync(basketId);
            if (basket is null)
                return null;

            // Check if items prices is correct
            if (basket?.Items?.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.GetRepository<Product>().GetAsync(item.Id);
                    if (item.Price != product!.Price)
                        item.Price = product!.Price;
                }
            }

            // Assign the shipping cost to basket
            if (basket!.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod>().GetAsync(basket!.DeliveryMethodId!.Value);
                basket.ShippingCost = deliveryMethod!.Cost;
            }

            PaymentIntentService paymentIntentService = new();
            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(basket?.PaymentIntentId)) // Create new Payment Intent
            {
                PaymentIntentCreateOptions createOptions = new()
                {
                    Amount = (long)(basket!.Items.Sum(item => item.Price * 100 * item.Quantity) + basket.ShippingCost * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                paymentIntent = await paymentIntentService.CreateAsync(createOptions);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else // Update the existing Payment Intent
            {
                PaymentIntentUpdateOptions updateOptions = new()
                {
                    Amount = (long)(basket!.Items.Sum(item => item.Price * 100 * item.Quantity) + basket.ShippingCost * 100)
                };

                paymentIntent = await paymentIntentService.UpdateAsync(basket.PaymentIntentId, updateOptions);
            }

            await _basketRepo.UpdateBasketAsync(basket);
            return basket;
        }

        public async Task<Order> UpdatePaymentIntent(string paymentIntentId, bool isSucceeded)
        {
            Specifications<Order> spec = new(o => o.PaymentIntentId == paymentIntentId);
            var order = await _unitOfWork.GetRepository<Order>().GetWithSpecsAsync(spec);

            order!.Status = isSucceeded ? OrderStatus.PaymentSucceeded : OrderStatus.PaymentFailed;

            _unitOfWork.GetRepository<Order>().Update(order);

            await _unitOfWork.SaveChangesAsync();

            return order;
        }
    }
}
