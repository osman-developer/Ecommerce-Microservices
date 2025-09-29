
namespace ECommerce.Authentication.Domain.DTOs
{
    public record AuthenticatedUserDTO(
        string UserId,
        string Email,
        string FirstName,
        string LastName,
        string Token,
        int? CustomerId,
        IList<string> Roles
    );
}
