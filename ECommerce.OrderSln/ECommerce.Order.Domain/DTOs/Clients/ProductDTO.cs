namespace ECommerce.Order.Domain.DTOs.Clients
{
    public record ProductDTO(int Id,
        string Name,
        decimal Price,
        int Quantity
        );
}
