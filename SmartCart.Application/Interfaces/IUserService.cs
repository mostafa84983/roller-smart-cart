using SmartCart.Application.Common;
using SmartCart.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Interfaces
{
    public interface IUserService
    {
        Task<GenericResult<UserDto>> GetUserById(int userid);
        Task<GenericResult<IEnumerable<UserDto>>> GetAllUsers(int page , int pageSize);
    }
}
