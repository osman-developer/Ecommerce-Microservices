using ECommerce.Common.Entities;


namespace ECommerce.Order.Domain.Entities
{
    public class OrderLine : BaseEntity
    {
        public int PurchaseOrderId { get; set; }
        public int ProductId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Navigation
        public PurchaseOrder PurchaseOrder { get; set; }
    }
}
