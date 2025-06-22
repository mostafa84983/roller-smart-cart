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
        private readonly IUnitOfWork _unitOfWork1;
        private readonly IMapper _mapper;
        public OrderService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork1= unitOfWork;
            _mapper = mapper;
        }

        public async Task<GenericResult<IEnumerable<OrderDto>>> GetOrdersOfUser(int userId)
        {
            var orders = await _unitOfWork1.Order.GetOrdersOfUser(userId);
            if (orders == null)
                return GenericResult<IEnumerable<OrderDto>>.Failure("No Order is Found");

            var ordersDto = _mapper.Map<List<OrderDto>>(orders);
            return GenericResult<IEnumerable<OrderDto>>.Success(ordersDto);
        }
    }
}
