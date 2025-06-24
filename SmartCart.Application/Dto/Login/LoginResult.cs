using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Dto.Login
{
    public class LoginResult
    {
        public JwtSecurityToken Token { get; set; }
        public string Role { get; set; }
    }
}
