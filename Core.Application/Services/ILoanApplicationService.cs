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
        Task<LoanRequestDto> CreateLoanApplicationAsync(LoanRequestDto loanRequest);
        Task<LoanRequestDto> GetLoanApplicationByIdAsync(string id);
        Task<IEnumerable<LoanRequestDto>> GetLoanApplicationsByUserIdAsync(string userId);
        Task<IEnumerable<LoanRequestDto>> GetAllLoanApplicationsAsync();
        Task UpdateLoanApplicationStatusAsync(string id, string status, string reason = null);
        Task DeleteLoanApplicationAsync(string id);
    }
}