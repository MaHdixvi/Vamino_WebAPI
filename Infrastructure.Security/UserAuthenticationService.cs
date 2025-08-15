using Core.Application.Contracts;
using Core.Application.DTOs;
using Domain.Entities;
using BCrypt.Net;
using System;
using System.Threading.Tasks;
using System.Security.Authentication;

namespace Infrastructure.Security
{
    /// <summary>
    /// سرویس احراز هویت و مدیریت پروفایل کاربران
    /// </summary>
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenGenerator _tokenGenerator;
        private readonly string _defaultRole;

        public UserAuthenticationService(
            IUserRepository userRepository,
            JwtTokenGenerator tokenGenerator,
            string defaultRole = "User")
        {
            _userRepository = userRepository;
            _tokenGenerator = tokenGenerator;
            _defaultRole = defaultRole;
        }

        /// <summary>
        /// ورود کاربر با نام کاربری و رمز عبور
        /// </summary>
        public async Task<AuthResponseDto> LoginAsync(UserLoginDto dto)
        {
            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null)
            {
                throw new AuthenticationException("اعتبارسنجی ناموفق: کاربر یافت نشد");
            }

            if (!VerifyPassword(user, dto.Password))
            {
                throw new AuthenticationException("اعتبارسنجی ناموفق: رمز عبور نادرست");
            }

            var token = _tokenGenerator.GenerateToken(user.Id);

            return new AuthResponseDto
            {
                Token = token,
                UserId = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
        }

        /// <summary>
        /// ثبت‌نام کاربر جدید
        /// </summary>
        public async Task<RegistrationResponseDto> RegisterAsync(UserRegistrationDto dto)
        {
            if (await _userRepository.GetByPhoneNumberAsync(dto.PhoneNumber) != null)
            {
                throw new ArgumentException("شماره موبایل تکراری است");
            }

            if (await _userRepository.GetByEmailAsync(dto.Email) != null)
            {
                throw new ArgumentException("آدرس ایمیل تکراری است");
            }

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = dto.Name,
                Name = dto.Name,
                NationalId = dto.NationalId,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                BankAccountNumber = dto.BankAccountNumber,
                CreatedAt = DateTime.UtcNow,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _userRepository.AddAsync(user);

            return new RegistrationResponseDto
            {
                UserId = user.Id,
                Token = _tokenGenerator.GenerateToken(user.Id)
            };
        }

        /// <summary>
        /// به‌روزرسانی پروفایل کاربر
        /// </summary>
        public async Task<UserProfileDto> UpdateProfileAsync(UserProfileDto dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException("کاربر یافت نشد");
            }

            // به‌روزرسانی فیلدها
            user.Name = dto.Name ?? user.Name;
            user.Email = dto.Email ?? user.Email;
            user.PhoneNumber = dto.PhoneNumber ?? user.PhoneNumber;
            user.BankAccountNumber = dto.BankAccountNumber ?? user.BankAccountNumber;

            await _userRepository.UpdateAsync(user);

            return MapToProfileDto(user);
        }

        /// <summary>
        /// تغییر رمز عبور
        /// </summary>
        public async Task ChangePasswordAsync(string userId, ChangePasswordDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("کاربر یافت نشد");
            }

            if (!VerifyPassword(user, dto.CurrentPassword))
            {
                throw new AuthenticationException("رمز عبور فعلی نادرست است");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _userRepository.UpdateAsync(user);
        }

        /// <summary>
        /// دریافت اطلاعات پروفایل کاربر
        /// </summary>
        public async Task<UserProfileDto> GetProfileAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("کاربر یافت نشد");
            }

            return MapToProfileDto(user);
        }

        private bool VerifyPassword(User user, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }

        private UserProfileDto MapToProfileDto(User user)
        {
            return new UserProfileDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                BankAccountNumber = user.BankAccountNumber,
                NationalId = user.NationalId,
                CreatedAt = user.CreatedAt
            };
        }
    }
}