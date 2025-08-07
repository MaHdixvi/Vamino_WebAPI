using Core.Application.Contracts;
using Core.Application.Exceptions;
using Core.Application.Helpers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    /// <summary>
    /// سرویس مدیریت اقساط وام
    /// </summary>
    public class InstallmentManagementService
    {
        private readonly IInstallmentRepository _installmentRepository;
        private readonly ILoanRepository _loanRepository;

        public InstallmentManagementService(IInstallmentRepository installmentRepository, ILoanRepository loanRepository)
        {
            _installmentRepository = installmentRepository;
            _loanRepository = loanRepository;
        }

        /// <summary>
        /// تولید برنامه اقساط برای یک وام
        /// </summary>
        /// <param name="loanId">شناسه وام</param>
        /// <param name="amount">مبلغ کل وام</param>
        /// <param name="numberOfInstallments">تعداد اقساط (مثلاً 36)</param>
        /// <returns>لیست اقساط</returns>
        public async Task<IEnumerable<Installment>> GenerateInstallmentScheduleAsync(string loanId, decimal amount, int numberOfInstallments)
        {
            var loan = await _loanRepository.GetByIdAsync(loanId);
            if (loan == null) throw new NotFoundException($"وام با شناسه {loanId} یافت نشد.");

            var installments = new List<Installment>();
            var principalPerInstallment = amount / numberOfInstallments;
            var interestRate = 0.15m; // نرخ بهره سالانه 15%
            var monthlyInterestRate = interestRate / 12;
            var interestPerInstallment = amount * monthlyInterestRate;

            for (int i = 1; i <= numberOfInstallments; i++)
            {
                var installment = new Installment
                {
                    Id = Guid.NewGuid().ToString(),
                    LoanId = loanId,
                    Number = i,
                    DueDate = DateHelper.CalculateInstallmentDueDate(loan.DisbursementDate, i),
                    PrincipalAmount = MathHelper.RoundToTwoDecimals(principalPerInstallment),
                    InterestAmount = MathHelper.RoundToTwoDecimals(interestPerInstallment),
                    TotalAmount = MathHelper.RoundToTwoDecimals(principalPerInstallment + interestPerInstallment),
                    Status = "Pending"
                };
                installments.Add(installment);
            }

            return installments;
        }

        /// <summary>
        /// دریافت اقساط یک وام
        /// </summary>
        /// <param name="loanId">شناسه وام</param>
        /// <returns>لیست اقساط</returns>
        public async Task<IEnumerable<Installment>> GetInstallmentsByLoanIdAsync(string loanId)
        {
            return await _installmentRepository.GetByLoanIdAsync(loanId);
        }

        /// <summary>
        /// به‌روزرسانی وضعیت یک قسط
        /// </summary>
        /// <param name="installmentId">شناسه قسط</param>
        /// <param name="status">وضعیت جدید (Paid, Pending, Overdue)</param>
        public async Task UpdateInstallmentStatusAsync(string installmentId, string status)
        {
            var installment = await _installmentRepository.GetByIdAsync(installmentId);
            if (installment != null)
            {
                installment.Status = status;
                if (status == "Paid")
                {
                    installment.PaymentDate = DateTime.UtcNow;
                }
                await _installmentRepository.UpdateAsync(installment);
            }
        }

        /// <summary>
        /// دریافت اقساط معوقه
        /// </summary>
        /// <returns>لیست اقساط معوقه</returns>
        public async Task<IEnumerable<Installment>> GetOverdueInstallmentsAsync()
        {
            var allInstallments = await _installmentRepository.GetAllAsync();
            return allInstallments.Where(i => i.Status == "Overdue" && i.DueDate < DateTime.UtcNow);
        }
    }
}