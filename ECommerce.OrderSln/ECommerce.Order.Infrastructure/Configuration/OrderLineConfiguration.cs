using ECommerce.Order.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Order.Infrastructure.Configurations
{
    public class OrderLineConfiguration : IEntityTypeConfiguration<OrderLine>
    {
        public void Configure(EntityTypeBuilder<OrderLine> builder)
        {
          
            builder.HasKey(ol => ol.Id);

            builder.Property(ol => ol.ProductId)
                   .IsRequired();

            builder.Property(ol => ol.Quantity)
                   .IsRequired();

            builder.Property(ol => ol.UnitPrice)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.HasOne(ol => ol.PurchaseOrder)
                      .WithMany(po => po.OrderLines)
                      .HasForeignKey(ol => ol.PurchaseOrderId)
                      .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
