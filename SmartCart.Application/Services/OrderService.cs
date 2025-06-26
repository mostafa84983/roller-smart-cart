using AutoMapper;
using SmartCart.Application.Common;
using SmartCart.Application.Dto;
using SmartCart.Application.Interfaces;
using SmartCart.Domain.Interfaces;
using SmartCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OrderService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork= unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> createOrder(int userId)
        {
            var user = await _unitOfWork.User.GetById(userId);
            if (user == null)
                return Result.Failure("User not found");

            var order = new Order
            {
                UserId = userId,
                OrderPrice = 0,
                OrderDiscount = 0 ,
                OrderNumber = $"ORD-{Guid.NewGuid().ToString().Substring(0, 8)}",
                CreationDate = DateTime.UtcNow,
            };

             await _unitOfWork.Order.Add(order);
            var result = _unitOfWork.Save() > 0;
            if (result)
                return Result.Success();
            else
                return Result.Failure("New order creation failed");
        }

        public async Task<GenericResult<IEnumerable<OrderDto>>> GetOrdersOfUser(int userId)
        {
            var orders = await _unitOfWork.Order.GetOrdersOfUser(userId);
            if (orders == null)
                return GenericResult<IEnumerable<OrderDto>>.Failure("No Order is Found");

            var ordersDto = _mapper.Map<List<OrderDto>>(orders);
            return GenericResult<IEnumerable<OrderDto>>.Success(ordersDto);
        }
    }
}
