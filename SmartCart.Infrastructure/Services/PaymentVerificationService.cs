/*using SmartCart.Application.Common;
using SmartCart.Application.Interfaces;
using SmartCart.Domain.Interfaces;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Infrastructure.Services
{
    public class PaymentVerificationService : IPaymentVerificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentVerificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResult<(string Status, int OrderId)>> GetPaymentStatus(string sessionId)
        {
            var service = new SessionService();
            var session = await service.GetAsync(sessionId);

            if (!session.Metadata.TryGetValue("orderId", out var orderIdStr) || !int.TryParse(orderIdStr, out int orderId))
                return GenericResult<(string, int)>.Failure("Invalid session metadata");

            return GenericResult<(string, int)>.Success((session.PaymentStatus, orderId));
        }

        public async Task<Result> HandleCheckoutSessionCompleted(string sessionId)
        {
            var service = new SessionService();
            var session = await service.GetAsync(sessionId);

            if (!session.Metadata.TryGetValue("orderId", out var orderIdStr) || !int.TryParse(orderIdStr, out int orderId))
                return Result.Failure("Invalid order ID in session");

            if (session.PaymentStatus != "paid")
                return Result.Failure("Payment not completed");


        }
    }
}
*/