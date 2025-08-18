using Core.Application.DTOs;

namespace Core.Application.Services
{
    public interface IPaymentStrategy
    {
        /// <summary>
        /// پردازش پرداخت بر اساس درخواست و کد پیگیری
        /// </summary>
        /// <param name="paymentRequest">اطلاعات پرداخت</param>
        /// <param name="trackingCode">کد پیگیری پرداخت</param>
        /// <returns>نتیجه پرداخت</returns>
        Task<PaymentResult> ProcessAsync(PaymentRequestDto paymentRequest, string trackingCode);
        Task<PaymentResult> PayAsync(decimal amount, string userId, string description);


    }
}