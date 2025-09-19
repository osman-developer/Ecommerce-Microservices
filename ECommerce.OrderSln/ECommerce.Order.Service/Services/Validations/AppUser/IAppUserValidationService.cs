using ECommerce.Common.DTOs.AppUser;
using ECommerce.Common.Response;

namespace ECommerce.Order.Service.Services.Validations.AppUser
{
    public interface IAppUserValidationService
    {
        Task<Response<Unit>> ValidateAsync(int id);
    }
}
