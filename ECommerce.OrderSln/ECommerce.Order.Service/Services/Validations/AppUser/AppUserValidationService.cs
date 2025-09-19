
using ECommerce.Common.Response;
using ECommerce.Order.Domain.Interfaces.Clients;

namespace ECommerce.Order.Service.Services.Validations.AppUser
{
    public class AppUserValidationService : IAppUserValidationService
    {
        private readonly IAppUserClientService _appUserClientService;

        public AppUserValidationService(IAppUserClientService appUserClientService)
        {
            _appUserClientService = appUserClientService;
        }

        public async Task<Response<Unit>> ValidateAsync(int id)
        {
            // Call Auth service to validate user
            var userResp = await _appUserClientService.GetAppUserByIdAsync(id);
            if (!userResp.Success || userResp.Data == null)
                return Response<Unit>.Fail(userResp.Message ?? "Failed to validate AppUser.");
            
            return Response<Unit>.Ok("");
        }
    }
}
