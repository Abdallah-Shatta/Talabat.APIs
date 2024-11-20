using System.Linq.Expressions;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Core.IRepositories;
using Talabat.Core.IServices;
using Talabat.Core.IUnitOfWork;
using Talabat.Core.Specifications;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepo;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepo, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _basketRepo = basketRepo;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            // 1- Get Basket from BasketRepo
            var basket = await _basketRepo.GetBasketAsync(basketId);

            // 2- Get selected items at basket from ProductRepo
            var orderItmes = new List<OrderItem>();
            if(basket?.Items?.Count > 0)
            {
                var productRepo = _unitOfWork.GetRepository<Product>();
                foreach (var item in basket.Items)
                {
                    var product = await productRepo.GetAsync(item.Id);
                    var productItemOrdered = new ProductItemOrdered(item.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
                    orderItmes.Add(orderItem);
                }
            }

            // 3- Calculate subtotal
            var subtotal = orderItmes.Sum(i => i.Quantity * i.Price);

            // 4- Get DeliveryMethod from its Repo
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod>().GetAsync(deliveryMethodId);

            // 5- Create Order
            var orderRepo = _unitOfWork.GetRepository<Order>();

            // Check if there's an order with an existing payment inetent
            Specifications<Order> spec = new(o => o.PaymentIntentId == basket.PaymentIntentId);
            var existingOrder = await orderRepo.GetWithSpecsAsync(spec);
            if (existingOrder is not null)
            {
                orderRepo.Delete(existingOrder);

                // this method implicitly update the basket with a new payment intent
                await _paymentService.CreateOrUpdatePaymentIntent(basketId); 
            }

            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItmes, subtotal, basket.PaymentIntentId);
            await orderRepo.AddAsync(order);

            // 6- save changes to Database
            var rowsAffected = await _unitOfWork.SaveChangesAsync();

            return rowsAffected <= 0 ? null : order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var orderRepo = _unitOfWork.GetRepository<Order>();

            Expression<Func<Order, bool>> filters = o => o.BuyerEmail == buyerEmail;
            List<Expression<Func<Order, object>>> includes = [o => o.DeliveryMethod, o => o.Items];
            var spec = new Specifications<Order>(filters, includes);
            spec.OrderByDesc = o => o.OrderDate;

            var orders = await orderRepo.GetAllWithSpecsAsync(spec);
            return orders;
        }

        public async Task<Order?> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var orderRepo = _unitOfWork.GetRepository<Order>();

            Expression<Func<Order, bool>> filters = o => o.Id == orderId && o.BuyerEmail == buyerEmail;
            List<Expression<Func<Order, object>>> includes = [o => o.DeliveryMethod, o => o.Items];
            var spec = new Specifications<Order>(filters, includes);

            var order = await orderRepo.GetWithSpecsAsync(spec);
            return order is null ? null : order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
            => await _unitOfWork.GetRepository<DeliveryMethod>().GetAllAsync();
    }
}
