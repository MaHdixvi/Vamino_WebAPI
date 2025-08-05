using Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    /// <summary>
    /// سرویس ارسال اعلان‌های پیامکی و ایمیلی به کاربران
    /// </summary>
    public interface INotificationService
    {
        Task SendNotificationAsync(NotificationDto notification);
        Task SendLoanApplicationStatusUpdateAsync(string userId, string status, string loanId);
        Task SendInstallmentReminderAsync(string userId, string installmentId, DateTime dueDate);
        Task SendPaymentConfirmationAsync(string userId, decimal amount, DateTime paymentDate);
        Task<bool> IsUserOptedInForSms(string userId);
        Task<bool> IsUserOptedInForEmail(string userId);
    }
}
