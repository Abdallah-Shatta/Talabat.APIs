using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Respository.Data.Configurations
{
    internal class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(m => m.Cost).HasColumnType("decimal(18,2)");
        }
    }
}
