using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Contracts
{
    /// <summary>
    /// قرارداد برای دسترسی به داده‌های اقساط وام
    /// </summary>
    public interface IInstallmentRepository
    {
        Task<Installment> GetByIdAsync(string id);
        Task<IEnumerable<Installment>> GetByLoanIdAsync(string loanId);
        Task<IEnumerable<Installment>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Installment>> GetPendingInstallmentsAsync();
        Task<IEnumerable<Installment>> GetOverdueInstallmentsAsync();
        Task AddAsync(Installment installment);
        Task UpdateAsync(Installment installment);
        Task DeleteAsync(string id);
    }
}
