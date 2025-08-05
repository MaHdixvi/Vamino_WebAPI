using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// قسط وام که به صورت ماهانه پرداخت می‌شود
    /// </summary>
    public class Installment
    {
        public string Id { get; set; }
        public string LoanId { get; set; }
        public int Number { get; set; } // شماره قسط (مثلاً 1 از 36)
        public DateTime DueDate { get; set; } // تاریخ سررسید
        public decimal PrincipalAmount { get; set; } // مبلغ اصل
        public decimal InterestAmount { get; set; } // مبلغ بهره
        public decimal TotalAmount { get; set; } // مجموع
        public string Status { get; set; } // Paid, Pending, Overdue
        public DateTime? PaymentDate { get; set; } // تاریخ پرداخت (در صورت پرداخت)
    }
}
