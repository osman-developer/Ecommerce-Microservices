using ECommerce.Order.Domain.DTOs.Core.Orderline;

namespace ECommerce.Order.Domain.DTOs.Core.Order
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
