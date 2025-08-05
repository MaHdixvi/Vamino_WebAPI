using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Contracts
{
    /// <summary>
    /// قرارداد برای دسترسی به داده‌های درخواست وام
    /// </summary>
    public interface ILoanApplicationRepository
    {
        Task<LoanApplication> GetByIdAsync(string id);
        Task<IEnumerable<LoanApplication>> GetByUserIdAsync(string userId);
        Task<IEnumerable<LoanApplication>> GetByStatusAsync(string status);
        Task AddAsync(LoanApplication application);
        Task UpdateAsync(LoanApplication application);
        Task DeleteAsync(string id);
        Task<IEnumerable<LoanApplication>> GetAllAsync();
    }
}
