using Core.Application.DTOs;
using Core.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.utils
{

    public class BankPaymentStrategy : IPaymentStrategy
    {
        public async Task<PaymentResult> PayAsync(decimal amount, string userId, string description)
        {
            // شبیه‌سازی پرداخت با بانک
            await Task.Delay(500); // فرض کنید زمان انجام تراکنش طول می‌کشد
            return new PaymentResult
            {
                IsSuccess = true,
                TransactionId = Guid.NewGuid().ToString(),
                Message = "پرداخت با موفقیت انجام شد"
            };
        }

        public Task<PaymentResult> ProcessAsync(PaymentRequestDto paymentRequest, string trackingCode)
        {
            throw new NotImplementedException();
        }
    }
}
