using Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    /// <summary>
    /// سرویس اصلی برای مدیریت درخواست‌های وام
    /// </summary>
    public interface ILoanApplicationService
    {
        Task<LoanApplicationDTO> CreateLoanApplicationAsync(LoanRequestDto loanRequest);
        Task<LoanApplicationDTO> GetLoanApplicationByIdAsync(string id);
        Task<IEnumerable<LoanApplicationDTO>> GetLoanApplicationsByUserIdAsync(string userId);
        Task<IEnumerable<LoanApplicationDTO>> GetAllLoanApplicationsAsync();
        Task UpdateLoanApplicationStatusAsync(string id, string status, string reason = null);
        Task DeleteLoanApplicationAsync(string id);
    }
}