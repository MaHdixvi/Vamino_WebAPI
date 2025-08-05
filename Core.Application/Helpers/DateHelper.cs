using System;

namespace Core.Application.Helpers
{
    /// <summary>
    /// کلاس کمکی برای عملیات تاریخی
    /// </summary>
    public static class DateHelper
    {
        /// <summary>
        /// محاسبه تاریخ سررسید قسط بعدی
        /// </summary>
        /// <param name="startDate">تاریخ شروع</param>
        /// <param name="installmentNumber">شماره قسط (مثلاً 1, 2, 3)</param>
        /// <returns>تاریخ سررسید</returns>
        public static DateTime CalculateInstallmentDueDate(DateTime startDate, int installmentNumber)
        {
            return startDate.AddMonths(installmentNumber - 1);
        }

        /// <summary>
        /// بررسی می‌کند که آیا تاریخ وارد شده گذشته است
        /// </summary>
        public static bool IsPastDate(DateTime date)
        {
            return date < DateTime.UtcNow;
        }

        /// <summary>
        /// بررسی می‌کند که آیا تاریخ وارد شده آینده است
        /// </summary>
        public static bool IsFutureDate(DateTime date)
        {
            return date > DateTime.UtcNow;
        }

        /// <summary>
        /// محاسبه تعداد روزهای باقی‌مانده تا تاریخ سررسید
        /// </summary>
        public static int DaysUntilDue(DateTime dueDate)
        {
            return (dueDate - DateTime.UtcNow).Days;
        }

        /// <summary>
        /// بررسی می‌کند که آیا تاریخ سررسید معوقه است
        /// </summary>
        public static bool IsOverdue(DateTime dueDate)
        {
            return DateTime.UtcNow > dueDate;
        }

        /// <summary>
        /// تبدیل تاریخ به فرمت استاندارد
        /// </summary>
        public static string ToStandardFormat(DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}