using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ECommerce.Order.Domain.Entities;

namespace ECommerce.Order.Infrastructure.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.OrderDate)
                   .IsRequired();

            builder.Property(o => o.TotalAmount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(o => o.AppUserId)
                   .IsRequired();

            builder.HasMany(o => o.OrderLines)
                   .WithOne(ol => ol.PurchaseOrder)
                   .HasForeignKey(ol => ol.PurchaseOrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
