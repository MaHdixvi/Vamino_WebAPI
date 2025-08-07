using System;
using System.Threading.Tasks;
using Domain.Entities;
using Core.Application.Contracts;

namespace Infrastructure.Security
{
    /// <summary>
    /// سرویس احراز هویت کاربران با استفاده از شماره موبایل و کد تأیید
    /// </summary>
    public class UserAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenGenerator _tokenGenerator;
        private readonly string _defaultRole;

        public UserAuthenticationService(IUserRepository userRepository, JwtTokenGenerator tokenGenerator, string defaultRole = "User")
        {
            _userRepository = userRepository;
            _tokenGenerator = tokenGenerator;
            _defaultRole = defaultRole;
        }

        /// <summary>
        /// ورود کاربر با شماره موبایل و کد تأیید
        /// </summary>
        /// <param name="phoneNumber">شماره موبایل</param>
        /// <param name="verificationCode">کد تأیید</param>
        /// <returns>توکن JWT در صورت موفقیت</returns>
        public async Task<string> LoginAsync(string phoneNumber, string verificationCode)
        {
            // در عمل، کد تأیید باید از سیستم پیامک یا کش (مثل Redis) بررسی شود
            // برای سادگی، فرض می‌کنیم کد 1234 معتبر است
            if (verificationCode != "1234")
            {
                throw new Exception("کد تأیید نامعتبر است.");
            }

            var user = await _userRepository.GetByPhoneNumberAsync(phoneNumber);
            if (user == null)
            {
                throw new Exception("کاربری با این شماره موبایل یافت نشد.");
            }

            return _tokenGenerator.GenerateToken(user.Id, _defaultRole);
        }

        /// <summary>
        /// ثبت‌نام کاربر جدید
        /// </summary>
        /// <param name="user">اطلاعات کاربر</param>
        /// <returns>شناسه کاربر جدید</returns>
        public async Task<string> RegisterAsync(User user)
        {
            if (await _userRepository.GetByPhoneNumberAsync(user.PhoneNumber) != null)
            {
                throw new Exception("کاربری با این شماره موبایل قبلاً ثبت‌نام کرده است.");
            }

            user.Id = Guid.NewGuid().ToString();
            user.CreatedAt = DateTime.UtcNow;
            await _userRepository.AddAsync(user);

            return user.Id;
        }
    }
}