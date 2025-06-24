using SmartCart.Application.Common;
using SmartCart.Application.Dto;
using SmartCart.Application.Dto.Login;
using SmartCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Interfaces
{
    public interface IUserService
    {
        Task<GenericResult<UserDto>> GetUserById(int userid);
        Task<GenericResult<IEnumerable<UserDto>>> GetAllUsers(int page , int pageSize);
        Task<GenericResult<LoginResult>> Login(LoginDto loginData);
        Task<GenericResult<LoginResult>> CreateToken(User user);
    }
}
