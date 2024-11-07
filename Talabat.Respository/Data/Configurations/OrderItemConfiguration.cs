using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Respository.Data.Configurations
{
    internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(i => i.Product, p => p.WithOwner());
            builder.Property(i => i.Price).HasColumnType("decimal(18,2)");
        }
    }
}
