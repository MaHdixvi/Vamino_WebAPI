using Core.Application.Contracts;
using Core.Application.DTOs;
using Core.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.utils
{
    public class WalletPaymentStrategy : IPaymentStrategy
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionLogService _transactionLogService;

        public WalletPaymentStrategy(IWalletRepository walletRepository, ITransactionLogService transactionLogService)
        {
            _walletRepository = walletRepository;
            _transactionLogService = transactionLogService;
        }

        public Task<PaymentResult> PayAsync(decimal amount, string userId, string description)
        {
            throw new NotImplementedException();
        }

        public async Task<PaymentResult> ProcessAsync(PaymentRequestDto paymentRequest, string trackingCode)
        {
            // منطق برداشت از کیف پول و ثبت تراکنش
            var balance = await _walletRepository.GetBalanceAsync(paymentRequest.UserId);
            if (balance < paymentRequest.Amount)
                return PaymentResult.Failed("موجودی کافی نیست");

            await _walletRepository.DeductAsync(paymentRequest.UserId, paymentRequest.Amount);
            await _transactionLogService.LogWalletTransactionAsync(
                paymentRequest.UserId,
                trackingCode,
                "WalletPayment",
                paymentRequest.Amount,
                balance - paymentRequest.Amount,
                "پرداخت از کیف پول"
            );

            return PaymentResult.Success(trackingCode, Guid.NewGuid().ToString());
        }
    }

}
