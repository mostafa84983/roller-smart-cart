using Microsoft.AspNetCore.Mvc;
using SmartCart.Application.Extensions;
using System.Security.Claims;

namespace SmartCart.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected int GetUserId()
        {
            return User.GetUserIdFromClaims();
        }

        protected string GetUserRole()
        {

            return User.GetRoleFromClaims();


        }
    }
}
