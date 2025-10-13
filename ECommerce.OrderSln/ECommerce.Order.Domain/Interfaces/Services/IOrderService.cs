
using ECommerce.Common.Response;
using ECommerce.Order.Domain.DTOs.Core.Order;
using ECommerce.Order.Domain.DTOs.Core.Orderline;

namespace ECommerce.Order.Domain.Interfaces.Services
{
    public interface IOrderService
    {
        Task<Response<GetPurchaseOrderDTO>> Save(AddOrUpdatePurchaseOrderDTO product);
        Task<Response<Unit>> Delete(int id);
        Task<Response<List<GetPurchaseOrderDTO>>> GetAll();
        Task<Response<List<GetPurchaseOrderDTO>>> GetPurchaseOrdersByClientId(string appUserId);
        Task<Response<List<GetOrderLineDTO>>> GetOrderLinesByOrderId(int orderId);
        Task<Response<GetPurchaseOrderDTO>> Get(int id);
    }
}
