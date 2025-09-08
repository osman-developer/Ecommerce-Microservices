using ECommerce.Common.Entities;

namespace ECommerce.Order.Domain.Entities
{
    public class PurchaseOrder : BaseEntity
    {
        public decimal TotalAmount { get; set; }
        public int AppUserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        // Navigation
        public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
    }
}
