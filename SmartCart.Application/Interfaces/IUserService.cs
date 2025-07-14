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
        Task<Result> Registeration(RegisterDto registerDto);
        Task<Result> UpdateUserData (int userId, string? firstName, string? lastName,
            string? phoneNumber, string? birthDate);
        Task<Result> ChangePassword(int userId, string currentPassword, string newPassword);
        Task<Result> lockUser(int userId , int durationInMinutes );
        Task<Result> UnLockUser(int userId);
    }
}
