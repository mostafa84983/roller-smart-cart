using AutoMapper;
using SmartCart.Application.Common;
using SmartCart.Application.Dto;
using SmartCart.Application.Interfaces;
using SmartCart.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCart.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork , IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GenericResult<IEnumerable<UserDto>>> GetAllUsers(int page, int pageSize)
        {
            var users = await _unitOfWork.User.GetAllUsers(page, pageSize);
            if (users == null || !users.Any())
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
    }
}
