using Core.Application.DTOs;
using Core.Application.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Applicationn.Services;

namespace Core.Application.Contracts
{
    /// <summary>
    /// سرویس پردازش پرداخت و انتقال وجه به کاربر
    /// </summary>
    public class PaymentService : IPaymentProcessor
    {
        private readonly ITransactionLogRepository _transactionLogRepository;
        private readonly ILoanRepository _loanRepository;

        public PaymentService(ITransactionLogRepository transactionLogRepository, ILoanRepository loanRepository)
        {
            _transactionLogRepository = transactionLogRepository;
            _loanRepository = loanRepository;
        }

        public async Task<bool> ProcessPaymentAsync(PaymentRequestDto paymentRequest)
        {
            // شبیه‌سازی اتصال به درگاه پرداخت بانکی
            var success = await SimulateBankPaymentAsync(paymentRequest);

            var log = new TransactionLog
            {
                Action = success ? "Pay" : "PayFailed",
                RelatedEntity = "Loan",
                Details = $"Payment for Loan {paymentRequest.LoanId}: {(success ? "Success" : "Failed")}",
                Timestamp = DateTime.UtcNow
            };
            await _transactionLogRepository.AddAsync(log);

            if (success)
            {
                var loan = await _loanRepository.GetByIdAsync(paymentRequest.LoanId);
                if (loan != null)
                {
                    loan.Status = "Paid";
                    await _loanRepository.UpdateAsync(loan);
                }
            }

            return success;
        }

        private async Task<bool> SimulateBankPaymentAsync(PaymentRequestDto paymentRequest)
        {
            // شبیه‌سازی تراکنش بانکی
            await Task.Delay(1000); // تأخیر شبیه‌سازی
            return true; // فرض موفقیت تراکنش
        }

        public Task<string> GeneratePaymentTrackingCode()
        {
            return Task.FromResult($"TRK-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 8)}");
        }

        public Task<bool> ValidateBankAccountAsync(string bankAccountNumber)
        {
            // اعتبارسنجی شماره حساب (الگوی ساده)
            return Task.FromResult(bankAccountNumber?.Length == 26 && bankAccountNumber.StartsWith("IR"));
        }

        public Task<decimal> CalculateTotalAmountWithCommission(decimal amount)
        {
            var commission = CommissionCalculator.CalculateCommission(amount);
            return Task.FromResult(amount + commission);
        }
    }
}
