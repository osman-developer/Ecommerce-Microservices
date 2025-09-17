using ECommerce.Common.Response;
using ECommerce.Product.Domain.DTOs.Clients;
using System.Threading.Tasks;

namespace ECommerce.Product.Domain.Interfaces.Clients
{
    public interface IProductClientService
    {
        Task<Response<IEnumerable<ProductOrderDTO>>> GetProductsByIdsAsync(IEnumerable<int> productIds);
    }
}
