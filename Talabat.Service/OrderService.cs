using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Core.IRepositories;
using Talabat.Core.IServices;
using Talabat.Core.IUnitOfWork;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepo;

        public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepo)
        {
            _unitOfWork = unitOfWork;
            _basketRepo = basketRepo;
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
            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItmes, subtotal);
            await _unitOfWork.GetRepository<Order>().AddAsync(order);

            // 6- save changes to Database
            var rowsAffected = await _unitOfWork.SaveChangesAsync();

            return rowsAffected <= 0 ? null : order;
        }

        public Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
