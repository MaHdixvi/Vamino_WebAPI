using System;
using System.Threading.Tasks;

namespace Core.Application.Contracts.Messaging
{
    /// <summary>
    /// قرارداد برای ارسال ایمیل به کاربران
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// ارسال ایمیل به یک آدرس ایمیل
        /// </summary>
        /// <param name="email">آدرس ایمیل</param>
        /// <param name="subject">موضوع ایمیل</param>
        /// <param name="message">متن ایمیل</param>
        /// <returns>تسک برای اجرای ناهمزمان</returns>
        Task SendEmailAsync(string email, string subject, string message);
    }
}