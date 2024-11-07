namespace Talabat.Core.Entities.OrderAggregate
{
    public class OrderItem : BaseEntity
    {
        // Accessable empty parameterless ctor for EF Core migration
        public OrderItem() { }
        // Parameter ctor for my usage
        public OrderItem(ProductItemOrdered product, decimal price, int quantity)
        {
            Product = product;
            Price = price;
            Quantity = quantity;
        }

        public ProductItemOrdered Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
