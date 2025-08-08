using Shared.Kernel;
using System;

namespace Domain.Entities
{
    /// <summary>
    /// لاگ تراکنش‌های مالی و عملیاتی سیستم
    /// </summary>
    public class TransactionLog:BaseEntity
    {
        public DateTime Timestamp { get; set; }
        public string Action { get; set; } // Apply, Approve, Pay, Reject
        public string RelatedEntity { get; set; } // LoanApplication, Loan
        public string Details { get; set; }

        // تغییر این قسمت: UserId باید nullable باشد
   public string? UserId { get; set; }

        // Navigation Property به کاربر
        public User User { get; set; }
    }
}