using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.DTOs
{
    /// <summary>
    /// DTO برای درخواست ثبت وام جدید
    /// </summary>
    public class LoanRequestDto
    {
        public string UserId { get; set; }
        public decimal RequestedAmount { get; set; }
        public int NumberOfInstallments { get; set; } // مثلاً 12, 24, 36
        public string Purpose { get; set; } // هدف از درخواست وام
    }
}
