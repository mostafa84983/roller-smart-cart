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
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
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
                return GenericResult<UserDto>.Failure("no user found");

            var userDto = _mapper.Map<UserDto>(user);
            return GenericResult<UserDto>.Success(userDto);

        }

        public async Task<GenericResult<LoginResult>> Login(LoginDto loginData)
        {
            var user = await _userManager.FindByEmailAsync(loginData.Email);
            if (user == null)
                return GenericResult<LoginResult>.Failure("Email is incorrect");

            var isLocked = await _userManager.IsLockedOutAsync(user);
            if (isLocked)
                return GenericResult<LoginResult>.Failure("Account is Locked out ");

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginData.Password);
            if (!isPasswordCorrect)
            {
                await _userManager.AccessFailedAsync(user);
                return GenericResult<LoginResult>.Failure("Password is incorrect");
            }

            await _userManager.ResetAccessFailedCountAsync(user);
            var result = await CreateToken(user);
            if (result.IsSuccess)
                return GenericResult<LoginResult>.Success(result.Value);

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
                signingCredentials: signingCredentials
                );

            var result = new LoginResult
            {
                Token = token,
                Role = role.FirstOrDefault()
            };

            return GenericResult<LoginResult>.Success(result);

        }

        public async Task<Result> Registeration(RegisterDto registerDto)
        {
            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            if (user != null)
                return Result.Failure("This email is already registered");

            var newUser = new User
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
                Gender = registerDto.Gender,
                BirthDate = registerDto.BirthDate,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(newUser, registerDto.Password);
            if (!result.Succeeded)
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result.Failure(errorMessage);
            }

            await _userManager.AddToRoleAsync(newUser, "User");
            return Result.Success();

        }

        public async Task<Result> UpdateUserData(int userId, string? firstName, string? lastName, string? phoneNumber, string? birthDate)
        {
            var updatedUser = await _unitOfWork.User.GetById(userId);
            if (updatedUser == null)
                return Result.Failure("user not found");

            if (!string.IsNullOrWhiteSpace(firstName))
                updatedUser.FirstName = firstName;

            if (!string.IsNullOrWhiteSpace(lastName))
                updatedUser.LastName = lastName;

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                updatedUser.PhoneNumber = phoneNumber;

            if (!string.IsNullOrWhiteSpace(birthDate) && DateTime.TryParse(birthDate, out var birthdate))
                updatedUser.BirthDate = birthdate;

            _unitOfWork.User.Update(updatedUser);
            var result = _unitOfWork.Save() > 0;

            if (result)
                return Result.Success();
            else
                return Result.Failure("Failed to Update User Data");


        }

        public async Task<Result> ChangePassword(int userId, string currentPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                return Result.Failure("Invalid empty password");

            if (string.IsNullOrWhiteSpace(currentPassword))
                return Result.Failure("Current password is required");

            if (newPassword.Length < 8)
                return Result.Failure("Invalid password less than 8 characters");

            if (!newPassword.Any(char.IsUpper) || !newPassword.Any(char.IsDigit))
                return Result.Failure("Password must contain at least one uppercase letter and one digit.");

            var user = await _unitOfWork.User.GetById(userId);
            if (user == null)
                return Result.Failure("Failed to change password");

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result.Failure(errorMessage);
            }
            return Result.Success();
        }

        public async Task<Result> lockUser(int userId, int durationInMinutes)
        {
            if (durationInMinutes <= 0)
                return Result.Failure("Invalid duration");

            var user = await _unitOfWork.User.GetById(userId);
            if (user == null)
                return Result.Failure("Invalid user");

            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.UtcNow.AddMinutes(durationInMinutes);

            var result = _unitOfWork.Save() > 0;
            if (result)
                return Result.Success();
            else
                return Result.Failure("Failed to Lock User");

        }

        public async Task<Result> UnLockUser(int userId)
        {
          
            var user = await _unitOfWork.User.GetById(userId);
            if (user == null)
                return Result.Failure("Invalid user");

            user.LockoutEnd = null;
            user.LockoutEnabled = true;
            user.AccessFailedCount = 0;

            var result = _unitOfWork.Save() > 0;
            if (result)
                return Result.Success();
            else
                return Result.Failure("Failed to Lock User");
        }
    }
}
