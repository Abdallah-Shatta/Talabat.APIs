namespace Talabat.Core.Entities.OrderAggregate
{
    public class Order : BaseEntity
    {
        // Accessable empty parameterless ctor for EF Core migration
        public Order() { }
        // Parameter ctor for my usage
        public Order(string buyerEmail,
                     Address shippingAddress,
                     DeliveryMethod? deliveryMethod,
                     ICollection<OrderItem> items,
                     decimal subtotal,
                     string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; }
        public DeliveryMethod? DeliveryMethod { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public decimal Subtotal { get; set; }
        // Derived attribute method دي طريقة الكومبايلر بيفهمها عشان يحسبه
        public decimal GetTotal() => Subtotal + DeliveryMethod.Cost;
        public string PaymentIntentId { get; set; }
    }
}
