using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Domain.Interfaces
{
    public interface ICartSessionService
    {
        int? GetOrderId(string cartId, int userId);
        void RemoveCart(string cartId, int userId);
        void SetOrderId(string cartId, int userId, int orderId);
    }
}