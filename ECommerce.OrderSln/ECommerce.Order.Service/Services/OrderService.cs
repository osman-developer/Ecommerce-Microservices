using AutoMapper;
using ECommerce.Common.Interface.Repository;
using ECommerce.Common.Response;
using ECommerce.Order.Domain.DTOs.Order;
using ECommerce.Order.Domain.DTOs.Orderline;
using ECommerce.Order.Domain.Entities;
using ECommerce.Order.Domain.Interfaces.Services;
using ECommerce.Order.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Order.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<PurchaseOrder> _orderRepo;
        private readonly IGenericRepository<OrderLine> _orderLineRepo;
        private readonly IMapper _mapper;
        private readonly OrderDbContext _context;
        private readonly HttpClient _httpClient;

        public OrderService(
            IGenericRepository<PurchaseOrder> orderRepo,
            IGenericRepository<OrderLine> orderLineRepo,
            IMapper mapper,
            OrderDbContext context, IHttpClientFactory httpClientFactory)
        {
            _orderRepo = orderRepo;
            _orderLineRepo = orderLineRepo;
            _mapper = mapper;
            _context = context;
            _httpClient = httpClientFactory.CreateClient("UserService");
        }

        public async Task<Response<GetPurchaseOrderDTO>> Get(int id)
        {
            // Use repository with includes instead of _context
            var orderResult = await _orderRepo.GetIncludingAsync(o => o.Id == id, o => o.OrderLines);
            if (!orderResult.Success || !orderResult.Data.Any())
                return Response<GetPurchaseOrderDTO>.Fail(orderResult.Message);

            var dto = _mapper.Map<GetPurchaseOrderDTO>(orderResult.Data.First());
            return Response<GetPurchaseOrderDTO>.Ok(dto, orderResult.Message);
        }

        public async Task<Response<List<GetPurchaseOrderDTO>>> GetAll()
        {
            var result = await _orderRepo.GetIncludingAsync(null, o => o.OrderLines);
            if (!result.Success)
                return Response<List<GetPurchaseOrderDTO>>.Fail(result.Message);

            var dtos = _mapper.Map<List<GetPurchaseOrderDTO>>(result.Data.ToList());
            return Response<List<GetPurchaseOrderDTO>>.Ok(dtos, result.Message);
        }

        public async Task<Response<GetPurchaseOrderDTO>> Save(AddOrUpdatePurchaseOrderDTO dto)
        {
            var entity = _mapper.Map<PurchaseOrder>(dto);

            if (dto.Id is null)
            {
                var createResult = await _orderRepo.CreateAsync(entity);
                if (!createResult.Success || createResult.Data == null)
                    return Response<GetPurchaseOrderDTO>.Fail(createResult.Message);

                var createdResult = await _orderRepo.GetIncludingAsync(o => o.Id == createResult.Data.Id, o => o.OrderLines);
                var createdDto = _mapper.Map<GetPurchaseOrderDTO>(createdResult.Data.First());
                return Response<GetPurchaseOrderDTO>.Ok(createdDto, createResult.Message);
            }

            var updateResult = await _orderRepo.UpdateAsync(entity);
            if (!updateResult.Success || updateResult.Data == null)
                return Response<GetPurchaseOrderDTO>.Fail(updateResult.Message);

            var updatedResult = await _orderRepo.GetIncludingAsync(o => o.Id == updateResult.Data.Id, o => o.OrderLines);
            var updatedDto = _mapper.Map<GetPurchaseOrderDTO>(updatedResult.Data.First());
            return Response<GetPurchaseOrderDTO>.Ok(updatedDto, updateResult.Message);
        }

        public async Task<Response<Unit>> Delete(int id)
        {
            return await _orderRepo.DeleteAsync(id);
        }

        public async Task<Response<List<GetPurchaseOrderDTO>>> GetPurchaseOrdersByClientId(int appUserId)
        {
            var result = await _orderRepo.GetIncludingAsync(o => o.AppUserId == appUserId, o => o.OrderLines);
            if (!result.Success)
                return Response<List<GetPurchaseOrderDTO>>.Fail(result.Message);

            var dtos = _mapper.Map<List<GetPurchaseOrderDTO>>(result.Data.ToList());
            return Response<List<GetPurchaseOrderDTO>>.Ok(dtos, result.Message);
        }

        public async Task<Response<List<GetOrderLineDTO>>> GetOrderLinesByOrderId(int orderId)
        {
            var orderResult = await _orderRepo.GetByIdAsync(orderId);
            if (!orderResult.Success || orderResult.Data == null)
                return Response<List<GetOrderLineDTO>>.Fail(orderResult.Message);

            var orderLines = await _context.OrderLines
                .Where(ol => ol.PurchaseOrderId == orderId)
                .ToListAsync();

            var dtos = _mapper.Map<List<GetOrderLineDTO>>(orderLines);
            return Response<List<GetOrderLineDTO>>.Ok(dtos, orderResult.Message);
        }
    }
}
