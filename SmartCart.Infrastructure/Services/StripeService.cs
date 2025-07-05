using Microsoft.Extensions.Configuration;
using SmartCart.Application.Dto.Payment;
using SmartCart.Application.Interfaces;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Infrastructure.Services
{
    public class StripeService : IStripeService
    {
        private readonly IConfiguration _configuration;

        public StripeService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CreateCheckoutSessionAsync(CreateStripeSessionDto dto)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = BuildLineItems(dto.Products),
                Mode = "payment",
                CustomerEmail = dto.Email,
                PhoneNumberCollection = new SessionPhoneNumberCollectionOptions { Enabled = false },
                SuccessUrl = $"{_configuration["Stripe:SuccessUrl"]}?sessionId={{CHECKOUT_SESSION_ID}}",
                /*      SuccessUrl = $"{_configuration["Stripe:SuccessUrl"]}?orderId={dto.OrderId}",  */
                /*      SuccessUrl = $"{_configuration["Stripe:SuccessUrl"]}?orderId={dto.OrderId}&cartId={dto.CartId}",  */

                CancelUrl = _configuration["Stripe:CancelUrl"],
                Metadata = new Dictionary<string, string>
                {
                 {   "orderId", dto.OrderId.ToString() }
                }
          /*      Metadata = new Dictionary<string, string>
                    {
                        { "orderId", "2002" }  
                    }*/
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session.Url;
        }

        private List<SessionLineItemOptions> BuildLineItems(List<StripeProductDto> products)
        {
            return products.Select(p => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(p.ProductPrice * 100),
                    Currency = "egp",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = p.ProductName
                    }
                },
                Quantity = p.ProductQuantity
            }).ToList();
        }
    }
}
