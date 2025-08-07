using System;

namespace Core.Applicationn.Services
{
    /// <summary>
    /// سرویس محاسبه کمیسیون بانکی و درآمد پروژه
    /// </summary>
    public class CommissionCalculator
    {
        private readonly decimal _commissionRate = 0.02m; // 2% کمیسیون از مبلغ وام
        private readonly decimal _transactionFeeRate = 0.01m; // 1% کارمزد تراکنش

        /// <summary>
        /// محاسبه کمیسیون بانکی بر اساس مبلغ وام
        /// </summary>
        /// <param name="loanAmount">مبلغ وام</param>
        /// <returns>مبلغ کمیسیون</returns>
        public decimal CalculateCommission(decimal loanAmount)
        {
            return Math.Round(loanAmount * _commissionRate, 2);
        }

        /// <summary>
        /// محاسبه کارمزد تراکنش برای هر پرداخت
        /// </summary>
        /// <param name="paymentAmount">مبلغ پرداخت</param>
        /// <returns>مبلغ کارمزد</returns>
        public decimal CalculateTransactionFee(decimal paymentAmount)
        {
            return Math.Round(paymentAmount * _transactionFeeRate, 2);
        }

        /// <summary>
        /// محاسبه کل درآمد پروژه از کمیسیون و کارمزد
        /// </summary>
        /// <param name="loanAmount">مبلغ وام</param>
        /// <param name="numberOfInstallments">تعداد اقساط</param>
        /// <returns>مجموع درآمد پروژه</returns>
        public decimal CalculateTotalRevenue(decimal loanAmount, int numberOfInstallments)
        {
            var commission = CalculateCommission(loanAmount);
            var totalTransactionFees = CalculateTransactionFee(loanAmount / numberOfInstallments) * numberOfInstallments;
            return commission + totalTransactionFees;
        }

        /// <summary>
        /// محاسبه کمیسیون با احتساب مالیات بر ارزش افزوده (مثلاً 9%)
        /// </summary>
        /// <param name="loanAmount">مبلغ وام</param>
        /// <param name="vatRate">نرخ مالیات (مثلاً 0.09 برای 9%)</param>
        /// <returns>مبلغ کمیسیون با مالیات</returns>
        public decimal CalculateCommissionWithVAT(decimal loanAmount, decimal vatRate = 0.09m)
        {
            var commission = CalculateCommission(loanAmount);
            return Math.Round(commission * (1 + vatRate), 2);
        }
    }
}