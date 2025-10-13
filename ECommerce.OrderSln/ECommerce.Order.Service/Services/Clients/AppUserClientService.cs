using ECommerce.Common.DTOs.AppUser;
using ECommerce.Common.Response;
using ECommerce.Order.Domain.Interfaces.Clients;
using System.Net.Http.Json;

namespace ECommerce.Order.Service.Services.Clients
{
    public class AppUserClientService : IAppUserClientService
    {
        private readonly HttpClient _httpClient;

        public AppUserClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Response<GetAppUserDTO>> GetAppUserByIdAsync(int id)
        {
            try
            {   //should later on make it on api gteway
                // Call the Auth service 
                var response = await _httpClient.GetAsync($"/api/Identity/{id}");

                if (!response.IsSuccessStatusCode)
                    return Response<GetAppUserDTO>.Fail("Failed to fetch AppUser from Auth service.");

                var result = await response.Content.ReadFromJsonAsync<Response<GetAppUserDTO>>();

                if (result == null || !result.Success || result.Data == null)
                    return Response<GetAppUserDTO>.Fail("No AppUser returned from Auth service.");

                return Response<GetAppUserDTO>.Ok(result.Data);
            }
            catch (Exception ex)
            {
                return Response<GetAppUserDTO>.Fail(ex.Message);
            }
        }
    }
}
