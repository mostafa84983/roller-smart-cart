using SmartCart.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Infrastructure.Services
{
    public class CartSessionService : ICartSessionService
    {
        private readonly Dictionary<string,int> _cartOrders = new();
        public int? GetOrderId(string cartId)
        {
            if (_cartOrders.TryGetValue(cartId, out var orderid))
                return orderid;
            else
                return null;
        }

        public void RemoveCart(string cartId)
        {
            _cartOrders.Remove(cartId);
        }

        public void SetOrderId(string cartId, int orderId)
        {
            _cartOrders[cartId] = orderId;
        }
    }
}
