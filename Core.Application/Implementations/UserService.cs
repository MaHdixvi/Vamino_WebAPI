using Core.Application.Contracts;
using Core.Application.DTOs;
using Core.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            var userEntity = await _userRepository.GetByIdAsync(id.ToString());

            if (userEntity == null) return null;

            // تبدیل دستی به DTO
            var userDto = new UserDto
            {
                Id = userEntity.Id,
                Name = userEntity.Name,
                Email = userEntity.Email
            };

            return userDto;
        }

    }
}
