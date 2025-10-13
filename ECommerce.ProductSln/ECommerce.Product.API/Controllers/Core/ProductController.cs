using ECommerce.Common.Response;
using ECommerce.Product.Domain.DTOs.Core.Product;
using ECommerce.Product.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Product.API.Controllers.Core
{
    [Route("api/core/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/product
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productService.GetAll();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // GET: api/product/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _productService.Get(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        // POST: api/product/save
        [HttpPost("save")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Save([FromBody] AddOrUpdateProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(Response<GetProductDTO>.Fail("Invalid input."));

            var result = await _productService.Save(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // DELETE: api/product/{id}
        [HttpDelete("{id:int}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.Delete(id);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
