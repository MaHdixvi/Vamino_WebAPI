using Core.Application.Contracts.Messaging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Messaging
{
    /// <summary>
    /// مدیر کلی اعلان‌ها که از روش‌های مختلف (پیامک، ایمیل، نوتیفیکیشن) استفاده می‌کند
    /// </summary>
    public class NotificationManager : INotificationManager
    {
        private readonly ISmsSender _smsSender;
        private readonly IEmailSender _emailSender;

        public NotificationManager(ISmsSender smsSender, IEmailSender emailSender)
        {
            _smsSender = smsSender;
            _emailSender = emailSender;
        }

        public async Task SendNotificationAsync(string userId, string message)
        {
            // در عمل، این متد باید از پروفایل کاربر بخواند که ترجیح می‌دهد چه روشی را دریافت کند
            await _smsSender.SendSmsAsync($"09{userId}", message); // فرض: userId شامل شماره است
        }

        public async Task SendNotificationAsync(string userId, string message, string method)
        {
            switch (method.ToLower())
            {
                case "sms":
                    await _smsSender.SendSmsAsync($"09{userId}", message);
                    break;
                case "email":
                    await _emailSender.SendEmailAsync($"{userId}@example.com", "اعلان سیستم وامینو", message);
                    break;
                case "push":
                    // در عمل، با سرویس FCM یا APN ادغام می‌شود
                    break;
                default:
                    await SendNotificationAsync(userId, message); // روش پیش‌فرض
                    break;
            }
        }

        public async Task SendInstallmentReminderAsync(string userId, int installmentNumber, DateTime dueDate)
        {
            var daysUntilDue = (dueDate - DateTime.UtcNow).Days;
            var message = $"یادآوری: قسط شماره {installmentNumber} شما در {daysUntilDue} روز آینده سررسید دارد.";

            await SendNotificationAsync(userId, message, "sms");
        }

        public async Task SendPaymentConfirmationAsync(string userId, decimal amount)
        {
            var message = $"مبلغ {amount:N0} تومان به حساب شما واریز شد.";

            await SendNotificationAsync(userId, message, "sms");
        }
    }
}