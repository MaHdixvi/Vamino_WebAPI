using Core.Application.Contracts;
using Domain.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        public async Task<User> GetByEmailAsync(string email)
        {
            // Validate email format
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("آدرس ایمیل نمی‌تواند خالی باشد", nameof(email));
            }

            if (!IsValidEmail(email))
            {
                throw new ArgumentException("فرمت آدرس ایمیل نامعتبر است", nameof(email));
            }

            // Normalize email
            email = email.Trim().ToLowerInvariant();

            try
            {
                var user = await _context.Users
                    .AsNoTracking() // Recommended for read-only operations
                    .FirstOrDefaultAsync(u => u.Email == email);

                return user ?? throw new KeyNotFoundException("کاربر یافت نشد");
            }
            catch (KeyNotFoundException)
            {
                // Re-throw with original message
                throw;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("خطا در پردازش درخواست دریافت کاربر");
            }
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // More comprehensive email validation
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
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

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
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