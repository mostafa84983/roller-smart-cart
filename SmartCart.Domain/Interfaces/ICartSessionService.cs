using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Domain.Interfaces
{
    public interface ICartSessionService
    {
        Task<int?> GetActiveOrderIdAsync(string cartId, int userId);
        Task SetOrderIdAsync(string cartId, int userId, int orderId);
        Task RemoveCartAsync(string cartId, int userId);
        Task<bool> CompleteOrder (int orderId);
    }
}