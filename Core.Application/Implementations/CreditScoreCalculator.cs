using Core.Application.Contracts;
using Core.Application.DTOs;
using Core.Application.Exceptions;
using Core.Application.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    /// <summary>
    /// محاسبه‌کننده نمره اعتباری کاربر بر اساس سابقه مالی و رفتار کاربری
    /// </summary>
    public class CreditScoreCalculator : ICreditScoreCalculator
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly ICreditScoreRepository _creditScoreRepository; // ✅ اضافه شد

        // ✅ اضافه کردن ICreditScoreRepository به سازنده
        public CreditScoreCalculator(
            IUserRepository userRepository,
            ILoanRepository loanRepository,
            ICreditScoreRepository creditScoreRepository)
        {
            _userRepository = userRepository;
            _loanRepository = loanRepository;
            _creditScoreRepository = creditScoreRepository; // ✅ تخصیص
        }

        public async Task<CreditScoreResponseDto> CalculateCreditScoreAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException($"کاربر با شناسه {userId} یافت نشد.");
            }

            var userLoans = await _loanRepository.GetByUserIdAsync(userId);
            var totalLoans = userLoans.Count();
            var paidLoans = userLoans.Count(l => l.Status == "Paid");
            var overdueLoans = userLoans.Count(l => l.Status == "Overdue");

            // محاسبه نمره پایه بر اساس پرداخت به موقع
            int baseScore = 500;
            if (totalLoans > 0)
            {
                var onTimePaymentRate = (double)paidLoans / totalLoans;
                baseScore += (int)(onTimePaymentRate * 300); // حداکثر 300 امتیاز از پرداخت به موقع
            }

            // کاهش نمره برای وام‌های معوقه
            if (overdueLoans > 0)
            {
                baseScore -= overdueLoans * 50; // هر وام معوقه 50 امتیاز کم می‌کند
                baseScore = Math.Max(baseScore, 300); // حداقل نمره 300
            }

            // افزایش نمره برای کاربران قدیمی
            var userAgeInDays = (DateTime.UtcNow - user.CreatedAt).Days;
            if (userAgeInDays > 365)
            {
                baseScore += 50; // 50 امتیاز برای کاربران بالای یک سال
            }

            // ایجاد شیء نمره اعتباری
            var creditScore = new CreditScore
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                Score = baseScore,
                RiskLevel = baseScore > 700 ? "Low" : (baseScore > 500 ? "Medium" : "High"),
                EvaluationDate = DateTime.UtcNow,
                ReasonForScore = GenerateReason(baseScore, paidLoans, overdueLoans),
                EvaluatedBy = "AI"
            };

            // ✅ ذخیره نمره اعتباری در دیتابیس
            await _creditScoreRepository.AddAsync(creditScore);

            // تبدیل به DTO برای ارسال به کنترلر
            return new CreditScoreResponseDto
            {
                UserId = creditScore.UserId,
                Score = creditScore.Score,
                RiskLevel = creditScore.RiskLevel,
                EvaluationDate = creditScore.EvaluationDate,
                EvaluatedBy = creditScore.EvaluatedBy
            };
        }

        public async Task<decimal> GetInterestRateForScore(int creditScore)
        {
            // بازگرداندن نرخ بهره بر اساس نمره اعتباری
            if (creditScore > 700) return 0.15m; // 15% برای اعتبار بالا
            if (creditScore > 500) return 0.18m; // 18% برای اعتبار متوسط
            return 0.22m; // 22% برای اعتبار پایین
        }

        public async Task<bool> IsUserEligibleForLoanAsync(string userId, decimal requestedAmount)
        {
            var creditScoreDto = await CalculateCreditScoreAsync(userId);

            // شرایط واجدیت: نمره بالای 500 و مبلغ درخواستی کمتر از 100 میلیون تومان
            return creditScoreDto.Score >= 500 && requestedAmount <= 100_000_000;
        }

        private string GenerateReason(int score, int paidLoans, int overdueLoans)
        {
            if (score >= 700)
            {
                return $"نمره بالا بر اساس {paidLoans} وام پرداخت شده و عدم وجود وام معوقه.";
            }
            else if (score >= 500)
            {
                return $"نمره متوسط بر اساس {paidLoans} وام پرداخت شده و {overdueLoans} وام معوقه.";
            }
            else
            {
                return $"نمره پایین بر اساس {overdueLoans} وام معوقه و سابقه پرداخت ضعیف.";
            }
        }
    }
}