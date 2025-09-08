using ECommerce.Common.Response;
using ECommerce.Order.Domain.DTOs.Order;
using ECommerce.Order.Domain.DTOs.Orderline;
using ECommerce.Order.Domain.Interfaces.Services;

namespace ECommerce.Order.Service.Services
{
    public class OrderService : IOrderService
    {
        public Task<Response<Unit>> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<GetPurchaseOrderDTO>> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<GetPurchaseOrderDTO>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<GetOrderLineDTO>>> GetOrderLinesByOrderId(int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<GetPurchaseOrderDTO>>> GetPurchaseOrdersByClientId(int appUserId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<GetPurchaseOrderDTO>> Save(AddOrUpdatePurchaseOrderDTO product)
        {
            throw new NotImplementedException();
        }
    }
}
