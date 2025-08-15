using Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Contracts
{

        /// <summary>
        /// رابط سرویس احراز هویت و مدیریت کاربران
        /// </summary>
        public interface IUserAuthenticationService
        {
            /// <summary>
            /// ورود کاربر با نام کاربری و رمز عبور
            /// </summary>
            /// <param name="dto">اطلاعات ورود</param>
            /// <returns>پاسخ احراز هویت شامل توکن و اطلاعات کاربر</returns>
            /// <exception cref="AuthenticationException">خطا در احراز هویت</exception>
            Task<AuthResponseDto> LoginAsync(UserLoginDto dto);

            /// <summary>
            /// ثبت‌نام کاربر جدید
            /// </summary>
            /// <param name="dto">اطلاعات ثبت‌نام</param>
            /// <returns>پاسخ ثبت‌نام شامل شناسه کاربر و توکن</returns>
            /// <exception cref="ArgumentException">خطا در اطلاعات ورودی</exception>
            Task<RegistrationResponseDto> RegisterAsync(UserRegistrationDto dto);

            /// <summary>
            /// به‌روزرسانی پروفایل کاربر
            /// </summary>
            /// <param name="userId">شناسه کاربر</param>
            /// <param name="dto">اطلاعات جدید پروفایل</param>
            /// <returns>اطلاعات به‌روزرسانی شده پروفایل</returns>
            /// <exception cref="KeyNotFoundException">کاربر یافت نشد</exception>
            Task<UserProfileDto> UpdateProfileAsync(UserProfileDto dto);

            /// <summary>
            /// تغییر رمز عبور کاربر
            /// </summary>
            /// <param name="userId">شناسه کاربر</param>
            /// <param name="dto">اطلاعات تغییر رمز عبور</param>
            /// <exception cref="KeyNotFoundException">کاربر یافت نشد</exception>
            /// <exception cref="AuthenticationException">رمز عبور فعلی نادرست است</exception>
            Task ChangePasswordAsync(string userId, ChangePasswordDto dto);

            /// <summary>
            /// دریافت اطلاعات پروفایل کاربر
            /// </summary>
            /// <param name="userId">شناسه کاربر</param>
            /// <returns>اطلاعات کامل پروفایل</returns>
            /// <exception cref="KeyNotFoundException">کاربر یافت نشد</exception>
            Task<UserProfileDto> GetProfileAsync(string userId);
        }
    
}
