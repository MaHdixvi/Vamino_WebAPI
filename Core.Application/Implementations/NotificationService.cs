using Core.Application.Contracts;
using Core.Application.DTOs;
using Core.Application.Services;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Application.Services
{
    /// <summary>
    /// سرویس ارسال اعلان‌های پیامکی و ایمیلی به کاربران
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly ITransactionLogRepository _transactionLogRepository;

        public NotificationService(ITransactionLogRepository transactionLogRepository)
        {
            _transactionLogRepository = transactionLogRepository;
        }

        /// <summary>
        /// ارسال یک اعلان عمومی
        /// </summary>
        public async Task SendNotificationAsync(NotificationDto notification)
        {
            // شبیه‌سازی ارسال اعلان
            var success = await SimulateSendAsync(notification);

            var log = new TransactionLog
            {
                Action = "Notify",
                RelatedEntity = "User",
                Details = $"Notification to {notification.UserId}: {(success ? "Sent" : "Failed")}",
                Timestamp = DateTime.UtcNow
            };
            await _transactionLogRepository.AddAsync(log);
        }

        /// <summary>
        /// ارسال به‌روزرسانی وضعیت درخواست وام به کاربر
        /// </summary>
        public async Task SendLoanApplicationStatusUpdateAsync(string userId, string status, string loanId)
        {
            var message = status switch
            {
                "Approved" => $"درخواست وام شما با شناسه {loanId} تأیید شد.",
                "Rejected" => $"درخواست وام شما با شناسه {loanId} رد شد.",
                _ => $"وضعیت درخواست وام شما با شناسه {loanId} به {status} تغییر کرد."
            };

            var notification = new NotificationDto
            {
                UserId = userId,
                Message = message,
                Subject = "به‌روزرسانی وضعیت وام",
                Type = "SMS"
            };

            await SendNotificationAsync(notification);
        }

        /// <summary>
        /// ارسال یادآوری پرداخت قسط
        /// </summary>
        public async Task SendInstallmentReminderAsync(string userId, string installmentId, DateTime dueDate)
        {
            var daysUntilDue = (dueDate - DateTime.UtcNow).Days;
            var message = $"یادآوری: قسط شماره {installmentId} شما در {daysUntilDue} روز آینده سررسید دارد.";

            var notification = new NotificationDto
            {
                UserId = userId,
                Message = message,
                Subject = "یادآوری پرداخت قسط",
                Type = "SMS"
            };

            await SendNotificationAsync(notification);
        }

        /// <summary>
        /// ارسال تأییدیه واریز وجه به کاربر
        /// </summary>
        public async Task SendPaymentConfirmationAsync(string userId, decimal amount, DateTime paymentDate)
        {
            var message = $"مبلغ {amount:N0} تومان به حساب شما در تاریخ {paymentDate:yyyy/MM/dd} واریز شد.";

            var notification = new NotificationDto
            {
                UserId = userId,
                Message = message,
                Subject = "تأیید واریز وجه",
                Type = "SMS"
            };

            await SendNotificationAsync(notification);
        }

        /// <summary>
        /// بررسی اینکه آیا کاربر تمایل به دریافت پیامک دارد
        /// </summary>
        public Task<bool> IsUserOptedInForSms(string userId)
        {
            // در عمل از دیتابیس بررسی می‌شود
            return Task.FromResult(true);
        }

        /// <summary>
        /// بررسی اینکه آیا کاربر تمایل به دریافت ایمیل دارد
        /// </summary>
        public Task<bool> IsUserOptedInForEmail(string userId)
        {
            // در عمل از دیتابیس بررسی می‌شود
            return Task.FromResult(true);
        }

        private async Task<bool> SimulateSendAsync(NotificationDto notification)
        {
            await Task.Delay(500); // تأخیر شبیه‌سازی
            return true;
        }
    }
}