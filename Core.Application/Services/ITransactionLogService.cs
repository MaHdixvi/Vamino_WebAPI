using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Application.DTOs;
using Domain.Entities;

namespace Core.Application.Services
{
    /// <summary>
    /// سرویس مدیریت لاگ تراکنش‌ها
    /// </summary>
    public interface ITransactionLogService
    {
        /// <summary>
        /// بررسی مالکیت یک لاگ (آیا این لاگ متعلق به کاربر هست یا نه)
        /// </summary>
        Task<bool> VerifyOwnershipAsync(string logId, string userId);

        /// <summary>
        /// ثبت لاگ جدید
        /// </summary>
        Task<TransactionLog> LogAsync(TransactionLogCreateDto logDto);

        /// <summary>
        /// بروزرسانی وضعیت پرداخت (مثلاً Pending → Completed → Failed)
        /// </summary>
        Task UpdatePaymentStatusAsync(string transactionId, string newStatus, string description = null);

        /// <summary>
        /// ثبت فعالیت مشکوک برای کاربر/تراکنش
        /// </summary>
        Task LogSuspiciousActivityAsync(string userId, string transactionId, string reason);

        /// <summary>
        /// ثبت نتیجه پرداخت (موفق یا ناموفق)
        /// </summary>
        Task LogPaymentResultAsync(string transactionId, bool success, string description = null);

        /// <summary>
        /// دریافت لاگ بر اساس کد پیگیری
        /// </summary>
        Task<TransactionLog> GetByTrackingCodeAsync(string trackingCode);

        /// <summary>
        /// دریافت تاریخچه تراکنش‌های یک موجودیت
        /// </summary>
        Task<IEnumerable<TransactionLog>> GetLogsByEntityAsync(string entityType, string entityId);

        /// <summary>
        /// دریافت تاریخچه تراکنش‌های یک کاربر
        /// </summary>
        Task<IEnumerable<TransactionLog>> GetLogsByUserAsync(string userId, DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// دریافت تاریخچه پرداخت‌های یک کاربر
        /// </summary>
        Task<IEnumerable<TransactionLog>> GetPaymentHistoryAsync(string userId, DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// دریافت تاریخچه تراکنش‌های یک نوع عملیات خاص
        /// </summary>
        Task<IEnumerable<TransactionLog>> GetLogsByActionAsync(string action, DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// دریافت لاگ با شناسه
        /// </summary>
        Task<TransactionLog> GetLogByIdAsync(string logId);

        /// <summary>
        /// جستجوی پیشرفته در لاگ‌ها
        /// </summary>
        Task<PaginatedList<TransactionLog>> SearchLogsAsync(TransactionLogSearchDto searchDto);

        /// <summary>
        /// دریافت آخرین وضعیت یک موجودیت
        /// </summary>
        Task<TransactionLog> GetLatestStatusAsync(string entityType, string entityId);

        /// <summary>
        /// ثبت لاگ تراکنش کیف پول
        /// </summary>
        Task LogWalletTransactionAsync(
            string userId,
            string transactionId,
            string action,
            decimal amount,
            decimal newBalance,
            string description);
    }
}
