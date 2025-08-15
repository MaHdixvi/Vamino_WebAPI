using Shared.Kernel;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    /// <summary>
    /// درخواست وام توسط کاربر
    /// </summary>
    public class LoanApplication : BaseEntity
    {
        public string UserId { get; set; }
        public DateTime SubmitDate { get; set; }
        public decimal RequestedAmount { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
        public string? ReasonForRejection { get; set; }
        public string? Purpose { get; set; }

        // ✅ اضافه شده: Navigation Property به کاربر
        public User User { get; set; }

        // ✅ اضافه شده: Navigation Property به وام (یک به یک)
        public Loan Loan { get; set; }

        // ✅ اضافه شده: Navigation Property به اقساط
        public ICollection<Installment> Installments { get; set; } = new List<Installment>();
    }
}