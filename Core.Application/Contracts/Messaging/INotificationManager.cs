using System;
using System.Threading.Tasks;

namespace Core.Application.Contracts.Messaging
{
    /// <summary>
    /// مدیر کلی اعلان‌ها که از روش‌های مختلف (پیامک، ایمیل، نوتیفیکیشن) استفاده می‌کند
    /// </summary>
    public interface INotificationManager
    {
        /// <summary>
        /// ارسال یک اعلان به کاربر با اولویت پیش‌فرض
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="message">متن اعلان</param>
        /// <returns>تسک برای اجرای ناهمزمان</returns>
        Task SendNotificationAsync(string userId, string message);

        /// <summary>
        /// ارسال یک اعلان به کاربر با انتخاب روش (SMS, Email, Push)
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="message">متن اعلان</param>
        /// <param name="method">روش ارسال (SMS, Email, Push)</param>
        /// <returns>تسک برای اجرای ناهمزمان</returns>
        Task SendNotificationAsync(string userId, string message, string method);

        /// <summary>
        /// ارسال یادآوری پرداخت قسط
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="installmentNumber">شماره قسط</param>
        /// <param name="dueDate">تاریخ سررسید</param>
        /// <returns>تسک برای اجرای ناهمزمان</returns>
        Task SendInstallmentReminderAsync(string userId, int installmentNumber, DateTime dueDate);

        /// <summary>
        /// ارسال تأییدیه واریز وجه
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="amount">مبلغ واریز شده</param>
        /// <returns>تسک برای اجرای ناهمزمان</returns>
        Task SendPaymentConfirmationAsync(string userId, decimal amount);
    }
}