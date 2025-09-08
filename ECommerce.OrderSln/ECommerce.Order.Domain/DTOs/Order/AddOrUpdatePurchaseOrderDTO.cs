using ECommerce.Order.Domain.DTOs.Orderline;
using System.ComponentModel.DataAnnotations;


namespace ECommerce.Order.Domain.DTOs.Order
{
    public record AddOrUpdatePurchaseOrderDTO
     (
         int? Id,
         [Required] int AppUserId,
         [Required] decimal TotalAmount,
         [Required] List<AddOrUpdateOrderLineDTO> OrderLines
     );
}
