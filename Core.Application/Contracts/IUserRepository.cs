using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Contracts
{
    /// <summary>
    /// قرارداد برای دسترسی به داده‌های کاربران
    /// </summary>
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string id);
        Task<User> GetByNationalIdAsync(string nationalId);
        Task<User> GetByPhoneNumberAsync(string phoneNumber);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(string id);
    }
}
