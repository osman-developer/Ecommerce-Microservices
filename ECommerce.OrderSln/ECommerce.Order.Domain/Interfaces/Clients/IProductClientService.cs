using ECommerce.Common.Response;
using ECommerce.Order.Domain.DTOs.Clients;

namespace ECommerce.Order.Domain.Interfaces.Clients
{
    public interface IProductClientService
    {
        Task<Response<List<ProductDTO>>> GetProductsByIdsAsync(IEnumerable<int> productIds);
    }
}
