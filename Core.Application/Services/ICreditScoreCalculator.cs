using Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    /// <summary>
    /// سرویس محاسبه نمره اعتباری کاربر با استفاده از الگوریتم هوش مصنوعی
    /// </summary>
    public interface ICreditScoreCalculator
    {
        Task<CreditScoreResponseDto> CalculateCreditScoreAsync(string userId);
        Task<bool> IsUserEligibleForLoanAsync(string userId, decimal requestedAmount);
        Task<decimal> GetInterestRateForScore(int creditScore);
    }
}
