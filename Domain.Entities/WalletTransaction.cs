using Shared.Kernel;
using System;

namespace Domain.Entities
{
    public class WalletTransaction :BaseEntity
    {
        public string TransactionId { get; set; }   // شناسه تراکنش
        public string UserId { get; set; }      
        public string Type { get; set; }            // Payment, Deposit, TransferIn, TransferOut
        public decimal Amount { get; set; }         // مبلغ تراکنش
        public decimal BalanceAfter { get; set; }   // موجودی بعد از تراکنش
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; }          // Pending, Completed, Failed, Refunded, Cancelled
        public string Description { get; set; }     // توضیحات تراکنش
        public string ReferenceTransactionId { get; set; } // تراکنش مرجع (برای لغو یا انتقال)

        // 🔑 برای جستجو و تایید پرداخت‌ها خیلی مهمه
        public string TrackingCode { get; set; }
        public string WalletId { get; set; } // بهتره کلید خارجی رو هم داشته باشی

        public Wallet Wallet { get; set; }
    }
}
