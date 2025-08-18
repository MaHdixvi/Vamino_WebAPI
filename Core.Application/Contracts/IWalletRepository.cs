using Domain.Entities;
using Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Contracts
{
    /// <summary>
    /// ریپازیتوری مدیریت کیف پول و تراکنش‌ها
    /// </summary>
    public interface IWalletRepository
    {
        /// <summary>
        /// کسر اعتبار از کیف پول کاربر
        /// </summary>
        Task<decimal> DeductAsync(string userId, decimal amount);

        /// <summary>
        /// دریافت تراکنش بر اساس Tracking Code
        /// </summary>
        Task<WalletTransaction> GetTransactionByTrackingCodeAsync(string trackingCode);

        Task<List<WalletTransaction>> GetTransactionsAsync(string userId, DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// لغو یک پرداخت (در صورت نیاز)
        /// </summary>
        Task<bool> CancelPaymentAsync(string transactionId, string reason);

        /// <summary>
        /// دریافت موجودی کاربر
        /// </summary>
        Task<decimal> GetBalanceAsync(string userId);

        /// <summary>
        /// برداشت از کیف پول (Debit)
        /// </summary>
        Task<decimal> DebitAsync(string userId, decimal amount, string description, string referenceTransactionId = null);

        /// <summary>
        /// واریز به کیف پول (Credit)
        /// </summary>
        Task<decimal> CreditAsync(string userId, decimal amount, string description, string referenceTransactionId = null);

        /// <summary>
        /// دریافت تراکنش بر اساس TransactionId
        /// </summary>
        Task<WalletTransaction> GetTransactionAsync(string transactionId);

        /// <summary>
        /// برگشت تراکنش (Reverse)
        /// </summary>
        Task<bool> ReverseTransactionAsync(string transactionId, string reason);

        /// <summary>
        /// دریافت کیف پول کاربر بر اساس UserId
        /// </summary>
        Task<Wallet> GetWalletByUserIdAsync(string userId);

        /// <summary>
        /// به‌روزرسانی موجودی کیف پول
        /// </summary>
        Task UpdateWalletAsync(Wallet wallet);

        /// <summary>
        /// اضافه کردن تراکنش جدید
        /// </summary>
        Task AddTransactionAsync(WalletTransaction transaction);

        /// <summary>
        /// دریافت تاریخچه تراکنش‌های کاربر
        /// </summary>
        Task<IEnumerable<WalletTransaction>> GetTransactionsByUserIdAsync(
            string userId, DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// ذخیره تغییرات (برای EF)
        /// </summary>
        Task SaveChangesAsync();

        /// <summary>
        /// انتقال اعتبار بین دو کاربر
        /// </summary>
        Task<WalletTransferResultDto> TransferAsync(string fromUserId, string toUserId, decimal amount, string description);
    }
}
