using Core.Application.Contracts;
using Core.Application.DTOs;
using Core.Application.Exceptions;
using Core.Application.Helpers;
using Core.Application.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    /// <summary>
    /// سرویس اصلی برای مدیریت فرآیند وام از ثبت تا پرداخت
    /// </summary>
    public class LoanProcessingService : ILoanApplicationService
    {
        private readonly ILoanApplicationRepository _loanApplicationRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly ICreditScoreCalculator _creditScoreCalculator;
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly IInstallmentRepository _installmentRepository;

        public LoanProcessingService(
            ILoanApplicationRepository loanApplicationRepository,
            ILoanRepository loanRepository,
            ICreditScoreCalculator creditScoreCalculator,
            IPaymentProcessor paymentProcessor,
            IInstallmentRepository installmentRepository)
        {
            _loanApplicationRepository = loanApplicationRepository;
            _loanRepository = loanRepository;
            _creditScoreCalculator = creditScoreCalculator;
            _paymentProcessor = paymentProcessor;
            _installmentRepository = installmentRepository;
        }

        public async Task<LoanRequestDto> CreateLoanApplicationAsync(LoanRequestDto loanRequest)
        {
            // بررسی اعتبار کاربر
            var isEligible = await _creditScoreCalculator.IsUserEligibleForLoanAsync(
                loanRequest.UserId,
                loanRequest.RequestedAmount);

            if (!isEligible)
            {
                throw new BusinessLogicException("کاربر واجد شرایط دریافت وام نیست.");
            }

            var application = new LoanApplication
            {
                Id = Guid.NewGuid().ToString(),
                UserId = loanRequest.UserId,
                RequestedAmount = loanRequest.RequestedAmount,
                SubmitDate = DateTime.UtcNow,
                Status = "Pending",
                ReasonForRejection = null
            };

            await _loanApplicationRepository.AddAsync(application);

            return new LoanRequestDto
            {
                UserId = application.UserId,
                RequestedAmount = application.RequestedAmount,
                NumberOfInstallments = loanRequest.NumberOfInstallments
            };
        }

        public async Task<LoanRequestDto> GetLoanApplicationByIdAsync(string id)
        {
            var application = await _loanApplicationRepository.GetByIdAsync(id);
            if (application == null)
            {
                throw new NotFoundException($"درخواست وام با شناسه {id} یافت نشد.");
            }

            return new LoanRequestDto
            {
                UserId = application.UserId,
                RequestedAmount = application.RequestedAmount
            };
        }

        public async Task<IEnumerable<LoanRequestDto>> GetLoanApplicationsByUserIdAsync(string userId)
        {
            var applications = await _loanApplicationRepository.GetByUserIdAsync(userId);
            var dtos = new List<LoanRequestDto>();

            foreach (var app in applications)
            {
                dtos.Add(new LoanRequestDto
                {
                    UserId = app.UserId,
                    RequestedAmount = app.RequestedAmount
                });
            }

            return dtos;
        }

        public async Task<IEnumerable<LoanRequestDto>> GetAllLoanApplicationsAsync()
        {
            var applications = await _loanApplicationRepository.GetAllAsync();
            var dtos = new List<LoanRequestDto>();

            foreach (var app in applications)
            {
                dtos.Add(new LoanRequestDto
                {
                    UserId = app.UserId,
                    RequestedAmount = app.RequestedAmount
                });
            }

            return dtos;
        }

        public async Task UpdateLoanApplicationStatusAsync(string id, string status, string reason = null)
        {
            var application = await _loanApplicationRepository.GetByIdAsync(id);
            if (application == null)
            {
                throw new NotFoundException($"درخواست وام با شناسه {id} یافت نشد.");
            }

            application.Status = status;
            application.ReasonForRejection = reason;

            if (status == "Approved")
            {
                // ایجاد وام نهایی
                var loan = new Loan
                {
                    Id = Guid.NewGuid().ToString(),
                    LoanApplicationId = application.Id,
                    Amount = application.RequestedAmount,
                    DisbursementDate = DateTime.UtcNow,
                    DueDate = DateTime.UtcNow.AddYears(3),
                    Status = "Active"
                };

                await _loanRepository.AddAsync(loan);

                // ایجاد اقساط
                var installments = await GenerateInstallmentSchedule(loan.Id, loan.Amount, 36);
                foreach (var installment in installments)
                {
                    await _installmentRepository.AddAsync(installment);
                }
            }

            await _loanApplicationRepository.UpdateAsync(application);
        }

        public async Task DeleteLoanApplicationAsync(string id)
        {
            var application = await _loanApplicationRepository.GetByIdAsync(id);
            if (application != null)
            {
                await _loanApplicationRepository.DeleteAsync(id);
            }
        }

        private async Task<IEnumerable<Installment>> GenerateInstallmentSchedule(string loanId, decimal amount, int numberOfInstallments)
        {
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
                    DueDate = DateTime.UtcNow.AddMonths(i),
                    PrincipalAmount = MathHelper.RoundToTwoDecimals(principalPerInstallment),
                    InterestAmount = MathHelper.RoundToTwoDecimals(interestPerInstallment),
                    TotalAmount = MathHelper.RoundToTwoDecimals(principalPerInstallment + interestPerInstallment),
                    Status = "Pending"
                };
                installments.Add(installment);
            }

            return installments;
        }
    }
}