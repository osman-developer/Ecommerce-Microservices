using System.ComponentModel.DataAnnotations;

namespace ECommerce.Product.Domain.DTOs.Product
{
    public record AddOrUpdateProductDto(
        int? Id,

        [Required(ErrorMessage = "Product name is required")]
        string Name,

        [Required, Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        int Quantity,

        [Required, Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        decimal Price
    );
}
