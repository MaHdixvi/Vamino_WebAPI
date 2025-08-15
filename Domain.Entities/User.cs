using Shared.Kernel;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    /// <summary>
    /// کاربر عادی سیستم که درخواست وام می‌دهد
    /// </summary>
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string NationalId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BankAccountNumber { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation Properties
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public ICollection<LoanApplication> LoanApplications { get; set; } = new List<LoanApplication>();
        public ICollection<TransactionLog> TransactionLogs { get; set; } = new List<TransactionLog>();
        public ICollection<CreditScore> CreditScores { get; set; } = new List<CreditScore>();
    }
}