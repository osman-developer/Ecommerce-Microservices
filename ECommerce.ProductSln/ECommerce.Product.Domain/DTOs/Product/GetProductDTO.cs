
namespace ECommerce.Product.Domain.DTOs.Product
{
    public record GetProductDTO
     (
         int Id,
         string Name,
         int Quantity,
         decimal Price
     );
}
