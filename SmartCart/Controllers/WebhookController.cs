using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCart.Application.Interfaces;
using Stripe;

namespace SmartCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly IWebhookService _webhookService;

        public WebhookController(IWebhookService webhookService)
        {
            _webhookService = webhookService;
        }

        [HttpPost]
        public async Task<IActionResult> Handle()
        {
            var payload = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signature = Request.Headers["Stripe-Signature"];
            Console.WriteLine("Webhook received from Stripe");

            var result = await _webhookService.ProcessWebhook(payload, signature);

            if (!result.IsSuccess)
            {
                Console.WriteLine($"Webhook error: {result.ErrorMessage}");
                return BadRequest(result.ErrorMessage);

            }
            Console.WriteLine("Webhook processed successfully");
            return Ok();
        }

    }
}
