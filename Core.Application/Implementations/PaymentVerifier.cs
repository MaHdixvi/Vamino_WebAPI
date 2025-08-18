using Core.Application.Contracts;
using Core.Application.DTOs;
using Core.Application.Services;
using Domain.Entities.Enums;
using System;
using System.Threading.Tasks;

namespace Core.Application.Implementations
{
    public class PaymentVerifier : IPaymentVerifier
    {
        private readonly IWalletTransactionRepository _transactionRepository;

        public PaymentVerifier(IWalletTransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<WalletTransactionStatus> VerifyPaymentAsync(string trackingCode)
        {
            var transaction = await _transactionRepository.GetByTrackingCodeAsync(trackingCode);

            if (transaction == null)
            {
                return new WalletTransactionStatus
                {
                    Status = "NotFound",
                    StatusDate = DateTime.UtcNow,
                    Amount = 0
                };
            }

            return new WalletTransactionStatus
            {
                Status = "Completed", // فرض بر موفق بودن
                StatusDate = transaction.TransactionDate,
                Amount = transaction.Amount
            };
        }

        public async Task<bool> CancelPaymentAsync(string trackingCode)
        {
            var transaction = await _transactionRepository.GetByTrackingCodeAsync(trackingCode);

            if (transaction == null || transaction.Type == TransactionType.Deposit.ToString())
                return false;

            // برگرداندن مبلغ به کیف پول
            transaction.Wallet.Balance += transaction.Amount;

            // تغییر وضعیت تراکنش
            transaction.Description += " [Cancelled]";
            transaction.Status = "Cancelled";

            await _transactionRepository.UpdateAsync(transaction);
            return true;
        }
    }
}
