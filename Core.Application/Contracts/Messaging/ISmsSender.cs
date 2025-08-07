using System;
using System.Threading.Tasks;

namespace Core.Application.Contracts.Messaging
{
    /// <summary>
    /// قرارداد برای ارسال پیامک به کاربران
    /// </summary>
    public interface ISmsSender
    {
        /// <summary>
        /// ارسال پیامک به یک شماره موبایل
        /// </summary>
        /// <param name="phoneNumber">شماره موبایل</param>
        /// <param name="message">متن پیامک</param>
        /// <returns>تسک برای اجرای ناهمزمان</returns>
        Task SendSmsAsync(string phoneNumber, string message);
    }
}