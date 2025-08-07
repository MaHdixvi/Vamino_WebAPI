using Shared.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// کاربر عادی سیستم که درخواست وام می‌دهد
    /// </summary>
    public class User: BaseEntity
    {
        public string Name { get; set; }
        public string NationalId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BankAccountNumber { get; set; }

        // ✅ اضافه شده: Navigation Property برای ارتباط با وام‌ها
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();

        // اضافه شده: Navigation Property برای درخواست‌های وام
        public ICollection<LoanApplication> LoanApplications { get; set; } = new List<LoanApplication>();

        // اضافه شده: Navigation Property برای لاگ‌های تراکنش
        public ICollection<TransactionLog> TransactionLogs { get; set; } = new List<TransactionLog>();
        public ICollection<CreditScore> CreditScores { get; set; } = new List<CreditScore>();

    }
}
