using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public interface IInstallmentManagementService
    {
        Task<IEnumerable<Installment>> GenerateInstallmentScheduleAsync(string loanId, decimal amount, int numberOfInstallments);
        Task<IEnumerable<Installment>> GetInstallmentsByLoanIdAsync(string loanId);
        Task UpdateInstallmentStatusAsync(string installmentId, string status);
        Task<IEnumerable<Installment>> GetOverdueInstallmentsAsync();
    }
}
