using ECommerce.Common.Response;
using ECommerce.Product.Domain.DTOs.Clients;
using ECommerce.Product.Domain.Interfaces.Clients;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Product.API.Controllers.Clients
{
    [Route("api/clients/product")]
    [ApiController]
  
    public class ProductClientController : ControllerBase
    {
        private readonly IProductClientService _productClientService;

        public ProductClientController(IProductClientService productClientService)
        {
            _productClientService = productClientService;
        }

        // POST: api/product/byIds
        [HttpGet("byIds")]
        public async Task<IActionResult> GetProductsByIds([FromQuery] IEnumerable<int> productIds)
        {
            if (productIds == null || !productIds.Any())
                return Ok(Response<IEnumerable<ProductOrderDTO>>.Fail("Product IDs must be provided."));

            var result = await _productClientService.GetProductsByIdsAsync(productIds);

            return Ok(result);
        }
    }
}
