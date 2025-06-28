using SmartCart.Application.Common;
using SmartCart.Application.Dto.Payment;
using SmartCart.Application.Interfaces;
using SmartCart.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStripeService _stripeService;

        public PaymentService(IUnitOfWork unitOfWork, IStripeService stripeService)
        {
            _unitOfWork = unitOfWork;
            _stripeService = stripeService;
        }

        public async Task<GenericResult<string>> CreateStripeSession(int orderId, int userId)
        {
            var order = await _unitOfWork.Order.GetById(orderId);
            if (order == null)
                return GenericResult<string>.Failure("Order not found");

            if(order.UserId != userId)
                return GenericResult<string>.Failure("Unauthorized");

            var orderProducts = await _unitOfWork.Product.GetPaginatedProductsOfOrder(orderId, 1 , 10);

            var stripeProducts = orderProducts.Select(op => new StripeProductDto
            {
                ProductName = op.Product.ProductName,
                ProductPrice = op.Product.ProductPrice,
                ProductQuantity = op.Quantity

            }).ToList();

            var user = await _unitOfWork.User.GetById(userId);
            if (user == null)
                return GenericResult<string>.Failure("User not found");

            var email = user.Email;

            var stripeSessionDto = new CreateStripeSessionDto
            {
                OrderId = orderId,
                Email = email,
                Products = stripeProducts
            };

            var sessionUrl = await _stripeService.CreateCheckoutSessionAsync(stripeSessionDto);

            return GenericResult<string>.Success(sessionUrl);

        }
    }
}
