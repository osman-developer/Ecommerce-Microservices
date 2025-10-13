using ECommerce.Common.DTOs.AppUser;
using ECommerce.Common.Response;


namespace ECommerce.Order.Domain.Interfaces.Clients
{
    public interface IAppUserClientService
    {
        Task<Response<GetAppUserDTO>> GetAppUserByIdAsync(string id);
    }
}
