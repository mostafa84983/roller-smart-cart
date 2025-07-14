using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SmartCart.API.Hubs;
using SmartCart.Application.Dto;
using SmartCart.Application.Dto.Product;
using SmartCart.Application.Interfaces;
using SmartCart.Domain.Interfaces;
using System.Data;

namespace SmartCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly ICartSessionService _cartSessionService;
        private readonly IHubContext<CartHub> _hubContext;
        public OrderController(IOrderService orderService  , ICartSessionService cartSessionService , IHubContext<CartHub> hubContext)
        {
            _orderService = orderService;
            _cartSessionService = cartSessionService;
            _hubContext = hubContext;
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

        [HttpPut("CompleteOrder")]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CompleteOrder(int OrderId  , string cartId)
        {
          
            var result = await _cartSessionService.CompleteOrder(OrderId);
            if (result == true)
            { 
                await _hubContext.Clients.Group(cartId)
               .SendAsync("CompleteProduct");

                return Ok();
            }
            else
                return BadRequest();
        }

    }
}
