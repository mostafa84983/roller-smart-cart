using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCart.Application.Dto.Payment;
using SmartCart.Application.Interfaces;

namespace SmartCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("checkout")]
        [Authorize]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CheckoutRequestDto request)
        {
            var userIdClaims = GetUserId();
            var orderId = request.OrderId;

            var result = await _paymentService.CreateStripeSession(orderId, userIdClaims);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(new { value = result.Value });
        }

    }
}
