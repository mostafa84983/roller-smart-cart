using SmartCart.Application.Common;
using SmartCart.Application.Dto;
using SmartCart.Domain.Interfaces;
using SmartCart.Domain.Models;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Interfaces
{
    public interface IOrderService
    {
        Task<GenericResult<IEnumerable<OrderDto>>> GetOrdersOfUser(int userId);
        Task<GenericResult<int>> CreateOrder(int userId);
    }
}
