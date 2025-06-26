using Microsoft.EntityFrameworkCore;
using SmartCart.Domain.Interfaces;
using SmartCart.Domain.Models;
using SmartCart.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository (DataContext context) : base(context)
        {
           
        }
        public async Task<IEnumerable<Order>> GetOrdersOfUser(int userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId ).ToListAsync();
        }

        public async Task<Order> GetOrderWithProducts(int orderId)
        {
            return await _context.Orders.Include(o=> o.OrderProducts).FirstOrDefaultAsync(o => o.OrderId == orderId);

        }
    }
}
