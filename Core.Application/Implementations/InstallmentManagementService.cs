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
    public class InstallmentManagementService:IInstallmentManagementService
    {
        private readonly IInstallmentRepository _installmentRepository;
        private readonly ILoanApplicationRepository _loanApplicationRepository;

        public InstallmentManagementService(IInstallmentRepository installmentRepository, ILoanApplicationRepository loanApplicationRepository)
        {
            _installmentRepository = installmentRepository;
            _loanApplicationRepository = loanApplicationRepository;
        }

        /// <summary>
        /// تولید برنامه اقساط برای یک وام
        /// </summary>
        /// <param name="loanId">شناسه وام</param>
        /// <param name="amount">مبلغ کل وام</param>
        /// <param name="numberOfInstallments">تعداد اقساط (مثلاً 36)</param>
        /// <returns>لیست اقساط</returns>
        public async Task<IEnumerable<Installment>> GenerateInstallmentScheduleAsync(string loanAppId, decimal amount, int numberOfInstallments)
        {
            var loan = await _loanApplicationRepository.GetByIdAsync(loanAppId);
            if (loan == null) throw new NotFoundException($"وام با شناسه {loanAppId} یافت نشد.");

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
                    LoanApplicationId = loanAppId,
                    Number = i,
                    DueDate = DateHelper.CalculateInstallmentDueDate(loan.CreatedAt, i),
                    PrincipalAmount = MathHelper.RoundToTwoDecimals(principalPerInstallment),
                    InterestAmount = MathHelper.RoundToTwoDecimals(interestPerInstallment),
                    TotalAmount = MathHelper.RoundToTwoDecimals(principalPerInstallment + interestPerInstallment),
                    Status = "Pending"
                };
                await _installmentRepository.AddAsync(installment);
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