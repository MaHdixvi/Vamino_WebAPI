using System;

namespace Core.Application.Helpers
{
    /// <summary>
    /// کلاس کمکی برای عملیات ریاضی
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// محاسبه بهره وام بر اساس نرخ سالانه
        /// </summary>
        /// <param name="principal">مبلغ اصل وام</param>
        /// <param name="annualInterestRate">نرخ بهره سالانه (مثلاً 0.15 برای 15%)</param>
        /// <param name="numberOfMonths">تعداد ماه‌ها</param>
        /// <returns>مبلغ کل بهره</returns>
        public static decimal CalculateTotalInterest(decimal principal, decimal annualInterestRate, int numberOfMonths)
        {
            return principal * annualInterestRate * numberOfMonths / 12;
        }

        /// <summary>
        /// محاسبه مبلغ قسط وام (روش خطی)
        /// </summary>
        /// <param name="principal">مبلغ اصل وام</param>
        /// <param name="annualInterestRate">نرخ بهره سالانه</param>
        /// <param name="numberOfInstallments">تعداد اقساط</param>
        /// <returns>مبلغ هر قسط</returns>
        public static decimal CalculateInstallmentAmount(decimal principal, decimal annualInterestRate, int numberOfInstallments)
        {
            var monthlyInterestRate = annualInterestRate / 12;
            var interestAmount = principal * monthlyInterestRate;
            var principalAmount = principal / numberOfInstallments;
            return principalAmount + interestAmount;
        }

        /// <summary>
        /// محاسبه کمیسیون بر اساس درصد
        /// </summary>
        /// <param name="amount">مبلغ</param>
        /// <param name="commissionRate">نرخ کمیسیون (مثلاً 0.02 برای 2%)</param>
        /// <returns>مبلغ کمیسیون</returns>
        public static decimal CalculateCommission(decimal amount, decimal commissionRate)
        {
            return amount * commissionRate;
        }

        /// <summary>
        /// گرد کردن به دو رقم اعشار
        /// </summary>
        public static decimal RoundToTwoDecimals(decimal value)
        {
            return Math.Round(value, 2);
        }
    }
}