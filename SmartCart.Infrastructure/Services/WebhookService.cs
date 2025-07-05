using Microsoft.Extensions.Configuration;
using SmartCart.Application.Common;
using SmartCart.Application.Interfaces;
using SmartCart.Domain.Interfaces;
using Stripe;
using Stripe.Checkout;
using System.Text.Json;

namespace SmartCart.Infrastructure.Services
{
    public class WebhookService : IWebhookService
    {
        private readonly IConfiguration _configuration;
        private readonly ICartSessionService _cartSessionService;

        public WebhookService(IConfiguration configuration, ICartSessionService cartSessionService)
        {
            _configuration = configuration;
            _cartSessionService = cartSessionService;
        }

        public async Task<Result> ProcessWebhook(string payload, string signature)
        {
            var webhookSecret = _configuration["Stripe:WebhookSecret"];

            var stripeEvent = EventUtility.ConstructEvent(
                payload,
                signature,
                webhookSecret,
                tolerance: 600
            );
            Console.WriteLine($"Stripe event type: {stripeEvent.Type}");

            if (stripeEvent.Type == "checkout.session.completed") 
            {
                var session = stripeEvent.Data.Object as Session;
                if (session == null)
                {
                    Console.WriteLine("Session is null");
                    return Result.Failure("Session is null");
                }

                Console.WriteLine($"Metadata found: {JsonSerializer.Serialize(session.Metadata)}");

                if (session.Metadata != null && session.Metadata.TryGetValue("orderId", out string? orderIdStr))
                {
                    Console.WriteLine($"orderId from metadata: {orderIdStr}");

                    if (int.TryParse(orderIdStr, out int orderId))
                    {
                        Console.WriteLine($"Parsed orderId: {orderId}");
                        var success = await _cartSessionService.CompleteOrder(orderId);
                        if (success)
                        {
                            Console.WriteLine("Order completed successfully");
                            return Result.Success();
                        }
                        else
                        {
                            Console.WriteLine("Failed to complete order in cart session service");
                            return Result.Failure("Failed to complete order");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to parse orderId");
                        return Result.Failure("Invalid orderId format");
                    }
                }
                else
                {
                    Console.WriteLine("Metadata is null or orderId not found");
                    return Result.Failure("Missing orderId in metadata");
                }
            }

            Console.WriteLine("Event type not handled, returning success");
            return Result.Success();
        }
    }
}