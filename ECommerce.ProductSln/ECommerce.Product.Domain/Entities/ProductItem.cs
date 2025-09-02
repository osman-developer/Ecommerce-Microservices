using ECommerce.Common.Entities;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Product.Domain.Entities
{
    public class ProductItem : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Quantity{ get; set; }
    }
}
