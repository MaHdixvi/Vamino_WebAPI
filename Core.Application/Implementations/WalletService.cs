using Core.Application.DTOs;
using Core.Application.Contracts;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransactionLogService _transactionLogService;
        private readonly ILogger<WalletService> _logger;

        public WalletService(
            IWalletRepository walletRepository,
            IUserRepository userRepository,
            ITransactionLogService transactionLogService,
            ILogger<WalletService> logger)
        {
            _walletRepository = walletRepository;
            _userRepository = userRepository;
            _transactionLogService = transactionLogService;
            _logger = logger;
        }

        public async Task<WalletPaymentResult> ProcessPaymentAsync(WalletPaymentRequest request)
        {
            try
            {
                if (request == null || request.Amount <= 0)
                    return new WalletPaymentResult { ErrorMessage = "درخواست نامعتبر است" };

                var balance = await _walletRepository.GetBalanceAsync(request.UserId);
                if (balance < request.Amount)
                    return new WalletPaymentResult { ErrorMessage = "موجودی کیف پول کافی نیست" };

                var transactionId = Guid.NewGuid().ToString();
                var newBalance = await _walletRepository.DebitAsync(
                    request.UserId,
                    request.Amount,
                    request.Description,
                    transactionId);

                await _transactionLogService.LogWalletTransactionAsync(
                    request.UserId,
                    transactionId,
                    "Payment",
                    request.Amount,
                    newBalance,
                    request.Description);

                return new WalletPaymentResult
                {
                    Success = true,
                    TransactionId = transactionId,
                    NewBalance = newBalance
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در پردازش پرداخت کیف پول");
                return new WalletPaymentResult { ErrorMessage = "خطای سیستمی در پردازش پرداخت" };
            }
        }

        public async Task<WalletDepositResult> DepositAsync(WalletDepositRequest request)
        {
            try
            {
                if (request == null || request.Amount <= 0)
                    return new WalletDepositResult { ErrorMessage = "درخواست نامعتبر است" };

                var transactionId = Guid.NewGuid().ToString();
                var newBalance = await _walletRepository.CreditAsync(
                    request.UserId,
                    request.Amount,
                    $"Deposit via {request.DepositMethod}",
                    transactionId);

                await _transactionLogService.LogWalletTransactionAsync(
                    request.UserId,
                    transactionId,
                    "Deposit",
                    request.Amount,
                    newBalance,
                    $"Deposit via {request.DepositMethod}");

                return new WalletDepositResult
                {
                    Success = true,
                    TransactionId = transactionId,
                    NewBalance = newBalance
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در واریز به کیف پول");
                return new WalletDepositResult { ErrorMessage = "خطای سیستمی در واریز وجه" };
            }
        }

        public async Task<decimal> GetBalanceAsync(string userId)
        {
            return await _walletRepository.GetBalanceAsync(userId);
        }

        public async Task<WalletTransactionStatus> VerifyPaymentAsync(string transactionId)
        {
            var transaction = await _walletRepository.GetTransactionAsync(transactionId);
            if (transaction == null)
                return new WalletTransactionStatus { Status = "NotFound" };

            return new WalletTransactionStatus
            {
                Status = transaction.Status,
                StatusDate = transaction.TransactionDate,
                Amount = transaction.Amount
            };
        }

        public async Task<bool> CancelTransactionAsync(string transactionId, string userId)
        {
            try
            {
                var transaction = await _walletRepository.GetTransactionAsync(transactionId);
                if (transaction == null || transaction.UserId != userId || transaction.Status != "Pending")
                    return false;

                await _walletRepository.ReverseTransactionAsync(transactionId, "User cancelled transaction");

                await _transactionLogService.LogWalletTransactionAsync(
                    userId,
                    transactionId,
                    "Refund",
                    transaction.Amount,
                    await _walletRepository.GetBalanceAsync(userId),
                    $"Transaction cancelled: {transactionId}");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطا در لغو تراکنش {transactionId}");
                return false;
            }
        }

        public async Task<WalletTransactionHistory> GetTransactionHistoryAsync(string userId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var transactions = await _walletRepository.GetTransactionsAsync(userId, fromDate, toDate);

            return new WalletTransactionHistory
            {
                Transactions = transactions.Select(t => new WalletTransactionItem
                {
                    TransactionId = t.TransactionId,
                    Type = t.Type,
                    Amount = t.Amount,
                    TransactionDate = t.TransactionDate,
                    Status = t.Status,
                    Description = t.Description
                }).ToList(),
                TotalCount = transactions.Count
            };
        }

        public async Task<WalletTransferResultDto> TransferAsync(WalletTransferRequest request)
        {
            try
            {
                if (request == null || request.Amount <= 0)
                    return new WalletTransferResultDto { ErrorMessage = "درخواست نامعتبر است" };

                var fromUserBalance = await _walletRepository.GetBalanceAsync(request.FromUserId);
                if (fromUserBalance < request.Amount)
                    return new WalletTransferResultDto { ErrorMessage = "موجودی کافی نیست" };

                var toUser = await _userRepository.GetByIdAsync(request.ToUserId);
                if (toUser == null)
                    return new WalletTransferResultDto { ErrorMessage = "کاربر مقصد یافت نشد" };

                var transactionId = Guid.NewGuid().ToString();
                var result = await _walletRepository.TransferAsync(
                    request.FromUserId,
                    request.ToUserId,
                    request.Amount,
                    transactionId);

                await _transactionLogService.LogWalletTransactionAsync(
                    request.FromUserId,
                    transactionId,
                    "TransferOut",
                    -request.Amount,
                    result.FromUserNewBalance,
                    $"Transfer to {toUser.Name}");

                await _transactionLogService.LogWalletTransactionAsync(
                    request.ToUserId,
                    transactionId,
                    "TransferIn",
                    request.Amount,
                    result.ToUserNewBalance,
                    $"Transfer from {request.FromUserId}");

                return new WalletTransferResultDto()
                {
                    Success = true,
                    TransactionId = transactionId,
                    FromUserNewBalance = result.FromUserNewBalance,
                    ToUserNewBalance = result.ToUserNewBalance
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در انتقال وجه بین کیف پول‌ها");
                return new WalletTransferResultDto { ErrorMessage = "خطای سیستمی در انتقال وجه" };
            }
        }

        public async Task<bool> CancelPaymentAsync(string trackingCode)
        {
            try
            {
                var transaction = await _walletRepository.GetTransactionByTrackingCodeAsync(trackingCode);
                if (transaction == null || transaction.Status != "Pending")
                    return false;

                await _walletRepository.ReverseTransactionAsync(transaction.TransactionId, "Payment cancelled by system/user");

                await _transactionLogService.LogWalletTransactionAsync(
                    transaction.UserId,
                    transaction.TransactionId,
                    "Refund",
                    transaction.Amount,
                    await _walletRepository.GetBalanceAsync(transaction.UserId),
                    $"Payment cancelled: {trackingCode}");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطا در لغو پرداخت با کد پیگیری {trackingCode}");
                return false;
            }
        }

    }
}
