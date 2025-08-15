using Shared.Kernel;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    /// <summary>
    /// وام نهایی پس از تأیید
    /// </summary>
    public class Loan : BaseEntity
    {
        public string LoanApplicationId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DisbursementDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } // Active, Paid, Overdue
        public string? InterestRate { get; set; }

        // ✅ اضافه شده: کلید خارجی
        public string UserId { get; set; }

        // ✅ اضافه شده: Navigation Property به کاربر
        public User User { get; set; }

        // ✅ اضافه شده: Navigation Property به درخواست وام
        public LoanApplication LoanApplication { get; set; }


    }
}