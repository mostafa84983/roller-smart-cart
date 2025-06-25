using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCart.Application.Dto;
using SmartCart.Application.Interfaces;
using System.Data;

namespace SmartCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("OrdersByUser")]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetOrdersByUser(int userid)
        {
            var result = await _orderService.GetOrdersOfUser(userid);
            if (result.IsSuccess)
                return Ok(result.Value);
            else
                return BadRequest(result.ErrorMessage);
        }

        
        [HttpGet("OrdersOfUser")]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetOrdersOfUser()
        {
            var userId = GetUserId();

            var result = await _orderService.GetOrdersOfUser(userId);
            if (result.IsSuccess)
                return Ok(result.Value);
            else
                return BadRequest(result.ErrorMessage);
        }


    }
}
