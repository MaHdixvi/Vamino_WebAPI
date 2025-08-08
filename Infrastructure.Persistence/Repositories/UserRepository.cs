using Core.Application.Contracts;
using Domain.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            try
            {
                user.Id = Guid.NewGuid().ToString();
                user.CreatedAt = DateTime.UtcNow;
                user.CreatedBy = user.Id;
                user.UpdatedBy= DateTime.UtcNow.ToString();
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // خطای مربوط به دیتابیس
                throw new Exception($"خطا در ذخیره کاربر: {ex.InnerException.Message}", ex);
            }
            catch (Exception ex)
            {
                // خطاهای دیگر
                throw new Exception($"خطای ناشناخته در ذخیره کاربر: {ex.Message}", ex);
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"خطا در حذف کاربر: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"خطای ناشناخته در حذف کاربر: {ex.Message}", ex);
            }
        }

        public async Task<User> GetByIdAsync(string id)
        {
            try
            {
                return await _context.Users.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"خطا در دریافت کاربر: {ex.Message}", ex);
            }
        }

        public async Task<User> GetByNationalIdAsync(string nationalId)
        {
            try
            {
                return await _context.Users
                    .FirstOrDefaultAsync(u => u.NationalId == nationalId);
            }
            catch (Exception ex)
            {
                throw new Exception($"خطا در جستجوی کاربر با کد ملی: {ex.Message}", ex);
            }
        }

        public async Task<User> GetByPhoneNumberAsync(string phoneNumber)
        {
            try
            {
                return await _context.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            }
            catch (Exception ex)
            {
                throw new Exception($"خطا در جستجوی کاربر با شماره موبایل: {ex.Message}", ex);
            }
        }

        public async Task UpdateAsync(User user)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"خطا در به‌روزرسانی کاربر: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"خطای ناشناخته در به‌روزرسانی کاربر: {ex.Message}", ex);
            }
        }
    }
}