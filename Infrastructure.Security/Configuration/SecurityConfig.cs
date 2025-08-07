using System;

namespace Infrastructure.Security.Configuration
{
    /// <summary>
    /// تنظیمات امنیتی سیستم شامل کلیدهای JWT، مدت زمان انقضا و محدودیت‌ها
    /// </summary>
    public class SecurityConfig
    {
        /// <summary>
        /// کلید محرمانه برای امضای توکن‌های JWT
        /// </summary>
        public string JwtSecretKey { get; set; } = "YourSuperSecretKeyThatShouldBeLongAndRandom123!";

        /// <summary>
        /// صادرکننده توکن (Issuer)
        /// </summary>
        public string JwtIssuer { get; set; } = "Waminio.Api";

        /// <summary>
        /// مخاطب توکن (Audience)
        /// </summary>
        public string JwtAudience { get; set; } = "Waminio.Client";

        /// <summary>
        /// مدت زمان انقضای توکن دسترسی (به دقیقه)
        /// </summary>
        public int JwtExpiryMinutes { get; set; } = 1440; // 24 ساعت

        /// <summary>
        /// مدت زمان انقضای توکن بازنشانی (به دقیقه)
        /// </summary>
        public int JwtRefreshTokenExpiryMinutes { get; set; } = 43200; // 30 روز

        /// <summary>
        /// طول حداقلی رمز عبور
        /// </summary>
        public int MinimumPasswordLength { get; set; } = 8;

        /// <summary>
        /// الزام استفاده از حروف بزرگ در رمز عبور
        /// </summary>
        public bool RequireUppercase { get; set; } = true;

        /// <summary>
        /// الزام استفاده از اعداد در رمز عبور
        /// </summary>
        public bool RequireNumbers { get; set; } = true;

        /// <summary>
        /// الزام استفاده از نمادها در رمز عبور
        /// </summary>
        public bool RequireSymbols { get; set; } = true;

        /// <summary>
        /// تعداد حداکثر تلاش ناموفق ورود قبل از قفل شدن
        /// </summary>
        public int MaxLoginAttempts { get; set; } = 5;

        /// <summary>
        /// مدت زمان قفل شدن حساب پس از تلاش‌های ناموفق (به دقیقه)
        /// </summary>
        public int LockoutDurationMinutes { get; set; } = 30;

        /// <summary>
        /// فعال بودن احراز هویت دو مرحله‌ای (2FA)
        /// </summary>
        public bool EnableTwoFactorAuthentication { get; set; } = true;

        /// <summary>
        /// طول کد تأیید (مثلاً 6 رقم)
        /// </summary>
        public int VerificationCodeLength { get; set; } = 6;

        /// <summary>
        /// مدت زمان اعتبار کد تأیید (به دقیقه)
        /// </summary>
        public int VerificationCodeExpiryMinutes { get; set; } = 10;
    }
}