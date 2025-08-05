using Shared.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// لاگ تراکنش‌های مالی و عملیاتی سیستم
    /// </summary>
    public class TransactionLog: BaseEntity
    {
        public DateTime Timestamp { get; set; }
        public string Action { get; set; } // Apply, Approve, Pay, Reject
        public string RelatedEntity { get; set; } // LoanApplication, Loan
        public string Details { get; set; }
    }
}
