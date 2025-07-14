using SmartCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserIdFromClaims(this ClaimsPrincipal user)
        {
            var claim = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(claim, out var id))
                return id;

            throw new UnauthorizedAccessException("Invalid or missing user ID.");
        }


        public static string GetRoleFromClaims(this ClaimsPrincipal user)
        {

            string role = user.FindFirstValue(ClaimTypes.Role);
            if(string.IsNullOrEmpty(role))
                throw new InvalidOperationException("Role is not found in token claims");
            return role;


        }

    }
}
