
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Order.Domain.DTOs.Core.Orderline
{
    public record AddOrUpdateOrderLineDTO
  (
      int? Id,
      [Required] int ProductId,
      [Required] int PurchaseOrderId,
      [Required] int Quantity,
      [Required] decimal UnitPrice
  );
}
