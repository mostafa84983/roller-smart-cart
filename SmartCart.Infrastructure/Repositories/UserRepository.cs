using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
    public class UserRepository : GenericRepository<User> , IUserRepository
    {
        private UserManager<User> _userManager;
        public UserRepository (DataContext context , UserManager<User> userManager) : base(context)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<User>> GetAllUsers(int page, int pageSize)
        {
            var role = await _context.Roles.Where(r => r.Name == "User").Select(r => r.Id)
                .FirstOrDefaultAsync();


            var userIds = _context.UserRoles
               .Where(ur => ur.RoleId == role)
               .Select(ur => ur.UserId);

            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .OrderBy(u => u.Id) 
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return users;

        }
    }
}
