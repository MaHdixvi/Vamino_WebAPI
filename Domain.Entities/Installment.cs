using Shared.Kernel;
using System;

namespace Domain.Entities
{
    /// <summary>
    /// قسط وام که به صورت ماهانه پرداخت می‌شود
    /// </summary>
    public class Installment:BaseEntity
    {
        // ✅ تغییر این قسمت: باید طول 36 کاراکتر باشد
        public string LoanId { get; set; }

        public int Number { get; set; }
        public DateTime DueDate { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // Paid, Pending, Overdue
        public DateTime? PaymentDate { get; set; }

        // ✅ اضافه شده: Navigation Property به وام
        public Loan Loan { get; set; }
    }
}