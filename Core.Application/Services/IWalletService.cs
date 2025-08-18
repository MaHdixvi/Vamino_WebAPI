using Core.Application.DTOs;
using System;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    /// <summary>
    /// سرویس مدیریت کیف پول الکترونیکی کاربران
    /// </summary>
    public interface IWalletService
    {
        /// <summary>
        /// پرداخت از طریق کیف پول
        /// </summary>
        Task<WalletPaymentResult> ProcessPaymentAsync(WalletPaymentRequest request);

        /// <summary>
        /// واریز وجه به کیف پول
        /// </summary>
        Task<WalletDepositResult> DepositAsync(WalletDepositRequest request);

        /// <summary>
        /// دریافت موجودی کیف پول کاربر
        /// </summary>
        Task<decimal> GetBalanceAsync(string userId);

        /// <summary>
        /// بررسی وضعیت یک تراکنش کیف پول
        /// </summary>
        Task<WalletTransactionStatus> VerifyPaymentAsync(string transactionId);

        /// <summary>
        /// لغو تراکنش در حالت Pending
        /// </summary>
        Task<bool> CancelTransactionAsync(string transactionId, string userId);

        /// <summary>
        /// دریافت تاریخچه تراکنش‌های کیف پول (با امکان فیلتر تاریخ)
        /// </summary>
        Task<WalletTransactionHistory> GetTransactionHistoryAsync(
            string userId,
            DateTime? fromDate = null,
            DateTime? toDate = null);

        /// <summary>
        /// انتقال وجه بین دو کیف پول
        /// </summary>
        Task<WalletTransferResultDto> TransferAsync(WalletTransferRequest request);
        Task<bool> CancelPaymentAsync(string trackingCode);
    }
}
