using Azure.Core.Pipeline;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SmartCart.Application.Dto;
using SmartCart.Application.Dto.Login;
using SmartCart.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace SmartCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("userById")]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetUserById (int userid)
        {
            var result = await _userService.GetUserById(userid);
            if (result.IsSuccess)
                return Ok(result.Value);
            else
                return BadRequest(result.ErrorMessage);
        }

        [Authorize]
        [HttpGet("userData")]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetUserById()
        {
            var userId = GetUserId();
            var result = await _userService.GetUserById(userId);
            if (result.IsSuccess)
                return Ok(result.Value);
            else
                return BadRequest(result.ErrorMessage);
        }


        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(int page , int pageSize)
        {
            var result = await _userService.GetAllUsers(page, pageSize);
            if (result.IsSuccess)
                return Ok(result.Value);
            else
                return BadRequest(result.ErrorMessage);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login (LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Login(loginDto);
            if (! result.IsSuccess)
                return BadRequest(result.ErrorMessage);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(result.Value.Token);

            //Send to external API
            //using (var httpClient = new HttpClient())
            //{
            //    var payload = new { token = tokenString };
            //    var response = await httpClient.PostAsJsonAsync("http://host.docker.internal:5050/set-token", payload);

            //}
            return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(result.Value.Token),
                    expiration = DateTime.Now.AddHours(2),
                    role = result.Value.Role
                });
    
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Registeration (RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Registeration(registerDto);
            if (result.IsSuccess)
                return Ok();
            else
                return BadRequest(result.ErrorMessage);
        }

        [Authorize]
        [HttpPut("UpdateData")]
        public async Task<IActionResult> UpdateUserData(string? firstName, string? lastName,
            string? phoneNumber, string? birthDate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();

            var result = await _userService.UpdateUserData(userId, firstName, lastName, phoneNumber, birthDate);
            if (result.IsSuccess)
                return Ok();
            else
                return BadRequest(result.ErrorMessage);
        }

        [Authorize]
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();

            var result = await _userService.ChangePassword(userId,currentPassword,newPassword);
            if (result.IsSuccess)
                return Ok();
            else
                return BadRequest(result.ErrorMessage);
        }

        [Authorize (Roles ="Admin")]
        [HttpPut("lockUser")]
        public async Task<IActionResult> LockUser (int userId , int durationInMinutes)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.lockUser(userId , durationInMinutes);
            if (result.IsSuccess)
                return Ok();
            else
                return BadRequest(result.ErrorMessage);

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UnlockUser")]
        public async Task<IActionResult> UnLockUser(int userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.UnLockUser(userId);
            if (result.IsSuccess)
                return Ok();
            else
                return BadRequest(result.ErrorMessage);

        }
    }
}
