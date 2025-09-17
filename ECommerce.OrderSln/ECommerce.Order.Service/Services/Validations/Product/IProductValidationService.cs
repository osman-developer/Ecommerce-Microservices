using ECommerce.Common.Response;
using ECommerce.Order.Domain.DTOs.Core.Order;

namespace ECommerce.Order.Service.Services.Validations.Product
{

    public interface IProductValidationService
    {
        Task<Response<Unit>> ValidateAsync(AddOrUpdatePurchaseOrderDTO dto);
    }
}
