using System.Net.Http.Json;
using ECommerce.Common.Response;
using ECommerce.Order.Domain.DTOs.Clients;
using ECommerce.Order.Domain.Interfaces.Clients;

namespace ECommerce.Order.Service.Clients
{
    public class ProductClientService : IProductClientService
    {
        private readonly HttpClient _httpClient;

        public ProductClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Response<List<ProductDTO>>> GetProductsByIdsAsync(IEnumerable<int> productIds)
        {
            try
            {
                var ids = productIds.Distinct().ToList();
                if (!ids.Any())
                    return Response<List<ProductDTO>>.Ok(new List<ProductDTO>(), "No products requested.");

                var query = string.Join("&", ids.Select(id => $"productIds={id}"));
                //should later on make it on api gteway
                var response = await _httpClient.GetAsync($"/api/clients/product/byIds?{query}");

                if (!response.IsSuccessStatusCode)
                    return Response<List<ProductDTO>>.Fail("Failed to fetch products from Product service.");

                var result = await response.Content.ReadFromJsonAsync<Response<List<ProductDTO>>>();

                if (result == null || !result.Success || result.Data == null)
                    return Response<List<ProductDTO>>.Fail("No products returned from Product service.");

                return Response<List<ProductDTO>>.Ok(result.Data);


            }
            catch (Exception ex)
            {
                return Response<List<ProductDTO>>.Fail(ex.Message);
            }
        }
    }
}
