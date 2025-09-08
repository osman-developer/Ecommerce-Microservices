using ECommerce.Order.Domain.DTOs.Orderline;

namespace ECommerce.Order.Domain.DTOs.Order
{
    public record GetPurchaseOrderDTO
    (
        int Id,
        int AppUserId,
        DateTime OrderDate,
        decimal TotalAmount,
        List<GetOrderLineDTO> OrderLines
    );
}
