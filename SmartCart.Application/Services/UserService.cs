using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartCart.Application.Common;
using SmartCart.Application.Dto;
using SmartCart.Application.Dto.Login;
using SmartCart.Application.Interfaces;
using SmartCart.Domain.Interfaces;
using SmartCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        public UserService(IUnitOfWork unitOfWork , IMapper mapper , UserManager<User> userManager , IConfiguration configuration) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<GenericResult<IEnumerable<UserDto>>> GetAllUsers(int page, int pageSize)
        {
            var users = await _unitOfWork.User.GetAllUsers(page, pageSize);
            if (!users.Any())
                return GenericResult<IEnumerable<UserDto>>.Failure("No users found");

            var usersDto = _mapper.Map<List<UserDto>>(users);
            return GenericResult<IEnumerable<UserDto>>.Success(usersDto);
        }

        public async Task<GenericResult<UserDto>> GetUserById(int userid)
        {
            var user = await _unitOfWork.User.GetById(userid);
            if (user == null)
                return  GenericResult<UserDto>.Failure("no user found");

            var userDto = _mapper.Map<UserDto>(user);
            return GenericResult<UserDto>.Success(userDto);

        }

        public async Task<GenericResult<LoginResult>> Login(LoginDto loginData)
        {
            var user = await _userManager.FindByEmailAsync(loginData.Email);
           if (user == null)
                return GenericResult<LoginResult>.Failure("Email is incorrect");

            var truePass = await _userManager.CheckPasswordAsync(user, loginData.Password);
            if(! truePass)
            {
                await _userManager.AccessFailedAsync(user);
                return GenericResult<LoginResult>.Failure("Password is incorrect");
            }

            await _userManager.ResetAccessFailedCountAsync(user);
            var result = await CreateToken(user);
            if(result.IsSuccess)
               return  GenericResult<LoginResult>.Success(result.Value);

            return GenericResult<LoginResult>.Failure("login failed");

        }
        public async Task<GenericResult<LoginResult>> CreateToken(User user)
        {
            var role = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier , user.Id.ToString()),
                new Claim(ClaimTypes.Email , user.Email),
                new Claim(ClaimTypes.Role,role.FirstOrDefault())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials : signingCredentials
                );

            var result = new LoginResult
            {
                Token = token,
                Role = role.FirstOrDefault()
            };

            return GenericResult < LoginResult >.Success(result);
   
        }
    }
}
