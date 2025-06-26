using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Domain.Interfaces
{
    public interface ICartSessionService
    {
        int? GetOrderId(string cartId);
        void SetOrderId (string cartId, int orderId);
        void RemoveCart(string cartId);
    }
}
