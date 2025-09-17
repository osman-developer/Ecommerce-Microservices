namespace ECommerce.Product.Domain.DTOs.Clients
{
  
    public record ProductOrderDTO(
        int Id,
        string Name,
        decimal Price,
        int Quantity
    );
}
