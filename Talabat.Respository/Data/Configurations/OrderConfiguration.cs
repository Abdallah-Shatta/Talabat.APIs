using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Respository.Data.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, s => s.WithOwner());
            builder.HasOne(o => o.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.SetNull);
            builder.Property(o => o.Subtotal).HasColumnType("decimal(18,2)");
            builder.Property(o => o.Status).HasConversion(
                    o => o.ToString(),
                    o => (OrderStatus) Enum.Parse(typeof(OrderStatus), o)
                );
        }
    }
}
