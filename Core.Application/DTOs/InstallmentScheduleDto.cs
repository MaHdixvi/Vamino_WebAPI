using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.DTOs
{
    /// <summary>
    /// DTO برای جدول اقساط وام
    /// </summary>
    public class InstallmentScheduleDto
    {
        public string LoanId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalInterest { get; set; }
        public int NumberOfInstallments { get; set; }
        public List<InstallmentDto> Installments { get; set; }
    }

    /// <summary>
    /// DTO برای اطلاعات یک قسط
    /// </summary>
    public class InstallmentDto
    {
        public int Number { get; set; }
        public DateTime DueDate { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }
}

