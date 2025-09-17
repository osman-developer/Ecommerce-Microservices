using ECommerce.Common.Response;
using ECommerce.Order.Domain.DTOs.Core.Order;
using ECommerce.Order.Domain.Interfaces.Clients;


namespace ECommerce.Order.Service.Services.Validations.Product
{
    public class ProductValidationService : IProductValidationService
    {
        private readonly IProductClientService _productClientService;

        public ProductValidationService(IProductClientService productClientService)
        {
            _productClientService = productClientService;
        }

        public async Task<Response<Unit>> ValidateAsync(AddOrUpdatePurchaseOrderDTO dto)
        {
            // 1) Batch-validate products by ids
            var productIds = dto.OrderLines.Select(x => x.ProductId).Distinct().ToList();
            var prodResp = await _productClientService.GetProductsByIdsAsync(productIds);
            if (!prodResp.Success || prodResp.Data == null)
                return Response<Unit>.Fail(prodResp.Message ?? "Failed to validate products.");

            // 2) Dictionary lookup
            var productsDict = prodResp.Data.ToDictionary(p => p.Id);

            // 3) Validate each order line (no db calls, only memory check)
            foreach (var line in dto.OrderLines)
            {
                if (!productsDict.TryGetValue(line.ProductId, out var prod))
                    return Response<Unit>.Fail($"Product {line.ProductId} not found.");

                if (prod.Quantity < line.Quantity)
                    return Response<Unit>.Fail($"Insufficient stock for product {line.ProductId}.");

                if (line.UnitPrice != prod.Price)
                    return Response<Unit>.Fail($"Price mismatch for product {line.ProductId}.");
            }

            return Response<Unit>.Ok("");
        }
    }
}
