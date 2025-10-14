using ECommerce.Common.DTOs.AppUser;
using ECommerce.Common.Response;
using ECommerce.Order.Domain.Interfaces.Clients;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace ECommerce.Order.Service.Services.Clients
{
    public class AppUserClientService : IAppUserClientService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppUserClientService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
        }

        public async Task<Response<GetAppUserDTO>> GetAppUserByIdAsync(string id)
        {
            try
            {
                var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(token))
                    return Response<GetAppUserDTO>.Fail("Authorization token is missing.");

                // Call the Auth service 
                var request = new HttpRequestMessage(HttpMethod.Get, $"/api/identity/{id}");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));

                var response = await _httpClient.SendAsync(request);


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
