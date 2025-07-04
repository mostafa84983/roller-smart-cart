using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SmartCart.Domain.Interfaces;
using SmartCart.Domain.Models;
using SmartCart.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Infrastructure.Services
{
    public class CartSessionService : ICartSessionService
    {
        private readonly DataContext _context;
        public CartSessionService(DataContext context) 
        {
            _context = context;
        }

        public async Task<bool> CompleteOrder(int orderId)
        {
            var CartOrder = await _context.CartOrders
                .Where(co => co.OrderId == orderId && co.IsActive)
                .OrderByDescending(co => co.CreatedAt)
                .FirstOrDefaultAsync();

             if(CartOrder!= null)
            {
                CartOrder.IsActive = false;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
            
        }

        public async Task<int?> GetActiveOrderIdAsync(string cartId, int userId)
        {
            var cartOrder = await _context.CartOrders
                .Where(co => co.CartId == cartId && co.UserId == userId && co.IsActive)
                .OrderByDescending(co => co.CreatedAt)
                .FirstOrDefaultAsync();
            return cartOrder?.OrderId;
        }

        public async Task RemoveCartAsync(string cartId, int userId)
        {
            var cartOrders = await _context.CartOrders.Where(co => co.CartId == cartId && co.UserId == userId).ToListAsync();
            if (cartOrders.Any())
            {
                _context.CartOrders.RemoveRange(cartOrders);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SetOrderIdAsync(string cartId, int userId, int orderId)
        {
            var existingOrders = await _context.CartOrders
                .Where(co => co.CartId == cartId && co.UserId == userId && co.IsActive)
               .ToListAsync();

            foreach( var order in existingOrders)
            {
                order.IsActive = false;
                order.UpdatedAt = DateTime.UtcNow;
                _context.CartOrders.Update(order);
            }

            var cartOrder = new CartOrder
            {
                UserId = userId,
                CartId = cartId,
                OrderId = orderId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _context.CartOrders.AddAsync(cartOrder);
            await _context.SaveChangesAsync();
        }
    }
}
