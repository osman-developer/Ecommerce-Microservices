using ECommerce.Common.Response;
using ECommerce.Product.Domain.DTOs.Product;

namespace ECommerce.Product.Domain.Interfaces.Services
{
    public interface IProductService
    {
        Task<Response<GetProductDTO>> Save(AddOrUpdateProductDto product);
        Task<Response<Unit>> Delete(int id);
        Task<Response<List<GetProductDTO>>> GetAll();
        Task<Response<GetProductDTO>> Get(int id);
    }
}
