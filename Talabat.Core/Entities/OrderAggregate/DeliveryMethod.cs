namespace Talabat.Core.Entities.OrderAggregate
{
    public class DeliveryMethod : BaseEntity
    {
        // Accessable empty parameterless ctor for EF Core migration
        public DeliveryMethod() { }
        // Parameter ctor for my usage
        public DeliveryMethod(string shortName, string description, decimal cost, string deliveryTime)
        {
            ShortName = shortName;
            Description = description;
            Cost = cost;
            DeliveryTime = deliveryTime;
        }

        public string ShortName { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string DeliveryTime { get; set; }
    }
}
