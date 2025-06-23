using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SmartCart.Application.Dto;
using SmartCart.Application.Interfaces;

namespace SmartCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        
        //[Authorize(Roles ="Admin")]
        [HttpGet("userData")]
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

        //[Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(int page , int pageSize)
        {
            var result = await _userService.GetAllUsers(page, pageSize);
            if (result.IsSuccess)
                return Ok(result.Value);
            else
                return BadRequest(result.ErrorMessage);
        }

    }
}
