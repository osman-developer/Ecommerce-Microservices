
using ECommerce.Common.Response;
using ECommerce.Product.API.Controllers.Core;
using ECommerce.Product.Domain.DTOs.Core.Product;
using ECommerce.Product.Domain.Interfaces.Services;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Product.Tests.Controllers
{
    public class ProductControllerTest
    {
        private readonly IProductService _productService;
        private readonly ProductController _productController;
        public ProductControllerTest()
        {
            //Set up dependencies
            _productService = A.Fake<IProductService>();
            // Inject fake service into controller
            _productController = new ProductController(_productService);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WhenServiceReturnsSuccess()
        {
            // Arrange
            var fakeProducts = new List<GetProductDTO>
            {
                new GetProductDTO(1, "Keyboard", 20, 49.99m),
                new GetProductDTO(2, "Mouse", 35, 19.99m)
            };

            // Set up fake response for GetAll
            var fakeResponse = new Response<List<GetProductDTO>>
            {
                Success = true,
                Data = fakeProducts,
                Message = "Products retrieved successfully"
            };

            A.CallTo(() => _productService.GetAll())
                .Returns(Task.FromResult(fakeResponse));

            // Act
            var result = await _productController.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var value = okResult.Value as Response<List<GetProductDTO>>;
            value.Should().NotBeNull();
            value!.Success.Should().BeTrue();
            value.Data.Should().HaveCount(2);
            value.Data.First().Name.Should().Be("Keyboard");
        }

    }
}
