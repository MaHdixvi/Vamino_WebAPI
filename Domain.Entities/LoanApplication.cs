using Shared.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// درخواست وام توسط کاربر
    /// </summary>
    public class LoanApplication: BaseEntity
    {
        public string UserId { get; set; }
        public DateTime SubmitDate { get; set; }
        public decimal RequestedAmount { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
        public string ReasonForRejection { get; set; }
    }
}
