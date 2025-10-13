using ECommerce.Order.Domain.Interfaces.Services;
using ECommerce.Common.Response;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Order.Domain.DTOs.Core.Order;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        // GET: api/purchaseorder
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderService.GetAll();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // GET: api/purchaseorder/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _orderService.Get(id);
            return result.Success ? Ok(result) : NotFound(result);
        }
       
        // POST: api/purchaseorder/save
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] AddOrUpdatePurchaseOrderDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(Response<GetPurchaseOrderDTO>.Fail("Invalid input."));

            var result = await _orderService.Save(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // DELETE: api/purchaseorder/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _orderService.Delete(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        // GET: api/purchaseorder/client/{appUserId}
        [HttpGet("client/{appUserId:int}")]
        public async Task<IActionResult> GetByClientId(string appUserId)
        {
            var result = await _orderService.GetPurchaseOrdersByClientId(appUserId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        // GET: api/purchaseorder/{orderId}/orderlines
        [HttpGet("{orderId:int}/orderlines")]
        public async Task<IActionResult> GetOrderLinesByOrderId(int orderId)
        {
            var result = await _orderService.GetOrderLinesByOrderId(orderId);
            return result.Success ? Ok(result) : NotFound(result);
        }
    
}
}
