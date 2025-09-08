namespace ECommerce.Order.Domain.DTOs.Orderline
{
    public record GetOrderLineDTO
  (
      int Id,
      int ProductId,
      int PurchaseOrderId,
      int Quantity,
      decimal UnitPrice
  );
}
