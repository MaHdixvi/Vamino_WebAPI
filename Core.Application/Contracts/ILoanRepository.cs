using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Contracts
{
    /// <summary>
    /// قرارداد برای دسترسی به داده‌های وام
    /// </summary>
    public interface ILoanRepository
    {
        Task<Loan> GetByIdAsync(string id);
        Task<IEnumerable<Loan>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Loan>> GetByLoanApplicationIdAsync(string loanApplicationId);
        Task AddAsync(Loan loan);
        Task UpdateAsync(Loan loan);
        Task DeleteAsync(string id);
    }
}
