using Core.Application.DTOs;
using System;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    /// <summary>
    /// سرویس پردازش پرداخت و مدیریت تراکنش‌های مالی
    /// </summary>
    public interface IPaymentProcessor
    {
        /// <summary>
        /// پردازش پرداخت با جزئیات کامل
        /// </summary>
        Task<PaymentResult> ProcessPaymentAsync(PaymentRequestDto paymentRequest);

        /// <summary>
        /// بررسی وضعیت پرداخت
        /// </summary>
        Task<PaymentStatusResult> VerifyPaymentAsync(string trackingCode);

        /// <summary>
        /// لغو پرداخت معلق
        /// </summary>
        Task<bool> CancelPaymentAsync(string trackingCode, string userId);

        /// <summary>
        /// تولید کد رهگیری منحصر به فرد برای پرداخت
        /// </summary>
        string GenerateTrackingCode();

        /// <summary>
        /// اعتبارسنجی شماره حساب/کارت بانکی
        /// </summary>
        BankAccountValidationResult ValidateBankAccount(string bankAccountNumber);

        /// <summary>
        /// محاسبه مبلغ نهایی با احتساب کارمزد و مالیات
        /// </summary>
        PaymentAmountBreakdown CalculatePaymentAmount(decimal amount, string currency = "IRR");

        /// <summary>
        /// دریافت آدرس درگاه پرداخت برای پرداخت آنلاین
        /// </summary>
        Task<PaymentGatewayResponse> GetPaymentGatewayUrlAsync(PaymentGatewayRequest request);

        /// <summary>
        /// پردازش بازگشت از درگاه پرداخت
        /// </summary>
        Task<PaymentResult> ProcessGatewayCallbackAsync(PaymentCallbackDto callbackData);
        Task<PaymentResult> PayWithPosAsync(PaymentRequestDto request, string trackingCode);
        Task<bool> CancelPosAsync(string rrnOrStan, string trackingCode);
        Task<PaymentResult> ProcessPosPaymentAsync(PaymentRequestDto request);


    }
}