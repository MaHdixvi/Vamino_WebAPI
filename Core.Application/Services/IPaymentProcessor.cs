using Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    /// <summary>
    /// سرویس پردازش پرداخت و انتقال وجه به کاربر
    /// </summary>
    public interface IPaymentProcessor
    {
        Task<bool> ProcessPaymentAsync(PaymentRequestDto paymentRequest);
        Task<string> GeneratePaymentTrackingCode();
        Task<bool> ValidateBankAccountAsync(string bankAccountNumber);
        Task<decimal> CalculateTotalAmountWithCommission(decimal amount);
    }
}
