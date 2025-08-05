using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.DTOs
{
    /// <summary>
    /// DTO برای درخواست پرداخت وجه
    /// </summary>
    public class PaymentRequestDto
    {
        public string LoanId { get; set; }
        public decimal Amount { get; set; }
        public string BankAccountNumber { get; set; }
        public string TrackingCode { get; set; }
    }
}
