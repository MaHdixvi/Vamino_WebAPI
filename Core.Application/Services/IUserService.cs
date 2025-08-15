using Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(Guid id);
    }

}
