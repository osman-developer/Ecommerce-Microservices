using ECommerce.Order.Domain.DTOs.Core.Orderline;
using System.ComponentModel.DataAnnotations;


namespace ECommerce.Order.Domain.DTOs.Core.Order
{
    public record AddOrUpdatePurchaseOrderDTO
     (
         int? Id,
         [Required] int AppUserId,
         [Required] decimal TotalAmount,
         [Required] List<AddOrUpdateOrderLineDTO> OrderLines
     );
}
