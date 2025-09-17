namespace ECommerce.Product.Domain.DTOs.Core.Product
{
    public record GetProductDTO
     (
         int Id,
         string Name,
         int Quantity,
         decimal Price
     );
}
