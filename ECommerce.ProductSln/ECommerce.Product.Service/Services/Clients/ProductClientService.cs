using AutoMapper;
using ECommerce.Common.Interface.Repository;
using ECommerce.Common.Response;
using ECommerce.Product.Domain.DTOs.Clients;
using ECommerce.Product.Domain.Entities;
using ECommerce.Product.Domain.Interfaces.Clients;
using Microsoft.Extensions.Logging;


namespace ECommerce.Product.Service.Services.Clients
{
   
    public class ProductClientService : IProductClientService
    {
        private readonly IGenericRepository<ProductItem> _productRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductClientService> _logger;

        public ProductClientService(
            IGenericRepository<ProductItem> productRepo,
            IMapper mapper,
            ILogger<ProductClientService> logger)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<IEnumerable<ProductOrderDTO>>> GetProductsByIdsAsync(IEnumerable<int> productIds)
        {
            try
            {
                var result = await _productRepo.FindByAsync(p => productIds.Contains(p.Id));
                if (!result.Success || result.Data == null || !result.Data.Any())
                    return Response<IEnumerable<ProductOrderDTO>>.Fail("No products found for the given IDs.");

                var mapped = _mapper.Map<IEnumerable<ProductOrderDTO>>(result.Data);
                return Response<IEnumerable<ProductOrderDTO>>.Ok(mapped);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products by IDs");
                return Response<IEnumerable<ProductOrderDTO>>.Fail("An error occurred while fetching products.");
            }
        }
    }
}
