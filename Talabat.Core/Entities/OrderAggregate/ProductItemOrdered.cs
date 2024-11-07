namespace Talabat.Core.Entities.OrderAggregate
{
    public class ProductItemOrdered
    {
        // Accessable empty parameterless ctor for EF Core migration
        public ProductItemOrdered() { }
        // Parameter ctor for my usage
        public ProductItemOrdered(int productId, string productName, string pictureUrl)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
    }
}
