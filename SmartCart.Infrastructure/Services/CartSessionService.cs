using SmartCart.Domain.Interfaces;
using SmartCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Infrastructure.Services
{
    public class CartSessionService : ICartSessionService
    {
        private readonly Dictionary<(string cartId, int userId), int> _cartOrders = new();

        public int? GetOrderId(string cartId , int userId)
        {
            if (_cartOrders.TryGetValue((cartId, userId), out var orderId))
                return orderId;

            return null;
        }

        public void RemoveCart(string cartId , int userId)
        {
            _cartOrders.Remove((cartId, userId));
        }

        public void SetOrderId(string cartId,int userId, int orderId)
        {
            _cartOrders[(cartId, userId)] = orderId;
        }
    }
}
