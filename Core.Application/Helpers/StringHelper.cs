using System;

namespace Core.Application.Helpers
{
    /// <summary>
    /// کلاس کمکی برای عملیات رشته‌ای
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// بررسی می‌کند که آیا رشته خالی یا null است
        /// </summary>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// بررسی می‌کند که آیا رشته خالی، null یا شامل فضای سفید است
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// حذف فضاهای سفید از ابتدا و انتهای رشته
        /// </summary>
        public static string TrimSafe(this string value)
        {
            return value?.Trim();
        }

        /// <summary>
        /// تبدیل رشته به فرمت کد ملی (1234567890)
        /// </summary>
        public static string ToNationalIdFormat(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            return value.Replace("-", "").Replace(" ", "");
        }

        /// <summary>
        /// تبدیل رشته به فرمت شماره موبایل (09123456789)
        /// </summary>
        public static string ToPhoneNumberFormat(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            return value.Replace("+98", "0").Replace("-", "").Replace(" ", "");
        }
    }
}