using Core.Application.Contracts;
using Core.Application.DTOs;
using Domain.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<WalletRepository> _logger;

        public WalletRepository(AppDbContext context, ILogger<WalletRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<decimal> GetBalanceAsync(string userId)
        {
            try
            {
                var wallet = await _context.Wallets
                    .AsNoTracking()
                    .FirstOrDefaultAsync(w => w.UserId == userId);

                return wallet?.Balance ?? 0m;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطا در دریافت موجودی کیف پول کاربر {userId}");
                throw;
            }
        }

        public async Task<Wallet> GetWalletByUserIdAsync(string userId)
        {
            try
            {
                return await _context.Wallets
                    .AsNoTracking()
                    .FirstOrDefaultAsync(w => w.UserId == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطا در دریافت کیف پول کاربر {userId}");
                throw;
            }
        }

        public async Task<decimal> CreditAsync(string userId, decimal amount, string description, string referenceTransactionId = null)
        {
            if (amount <= 0)
                throw new ArgumentException("مبلغ باید بزرگتر از صفر باشد", nameof(amount));

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // دریافت یا ایجاد کیف پول
                var wallet = await _context.Wallets
                    .FirstOrDefaultAsync(w => w.UserId == userId);

                if (wallet == null)
                {
                    wallet = new Wallet
                    {
                        UserId = userId,
                        Balance = amount,
                        LastUpdated = DateTime.UtcNow
                    };
                    await _context.Wallets.AddAsync(wallet);
                }
                else
                {
                    wallet.Balance += amount;
                    wallet.LastUpdated = DateTime.UtcNow;
                    _context.Wallets.Update(wallet);
                }

                // ثبت تراکنش
                var walletTransaction = new WalletTransaction
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    UserId = userId,
                    Type = "Deposit",
                    Amount = amount,
                    BalanceAfter = wallet.Balance,
                    TransactionDate = DateTime.UtcNow,
                    Status = "Completed",
                    Description = description,
                    ReferenceTransactionId = referenceTransactionId
                };

                await _context.WalletTransactions.AddAsync(walletTransaction);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return wallet.Balance;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, $"خطا در واریز به کیف پول کاربر {userId}");
                throw;
            }
        }

        public async Task<decimal> DebitAsync(string userId, decimal amount, string description, string referenceTransactionId = null)
        {
            if (amount <= 0)
                throw new ArgumentException("مبلغ باید بزرگتر از صفر باشد", nameof(amount));

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // دریافت کیف پول
                var wallet = await _context.Wallets
                    .FirstOrDefaultAsync(w => w.UserId == userId);

                if (wallet == null || wallet.Balance < amount)
                    throw new InvalidOperationException("موجودی کیف پول کافی نیست");

                // کسر از موجودی
                wallet.Balance -= amount;
                wallet.LastUpdated = DateTime.UtcNow;
                _context.Wallets.Update(wallet);

                // ثبت تراکنش
                var walletTransaction = new WalletTransaction
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    UserId = userId,
                    Type = "Withdrawal",
                    Amount = -amount,
                    BalanceAfter = wallet.Balance,
                    TransactionDate = DateTime.UtcNow,
                    Status = "Completed",
                    Description = description,
                    ReferenceTransactionId = referenceTransactionId
                };

                await _context.WalletTransactions.AddAsync(walletTransaction);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return wallet.Balance;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, $"خطا در برداشت از کیف پول کاربر {userId}");
                throw;
            }
        }

        public async Task<WalletTransferResultDto> TransferAsync(string fromUserId, string toUserId, decimal amount, string description)
        {
            if (amount <= 0)
                throw new ArgumentException("مبلغ باید بزرگتر از صفر باشد", nameof(amount));

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // دریافت کیف پول مبدا
                var fromWallet = await _context.Wallets
                    .FirstOrDefaultAsync(w => w.UserId == fromUserId);

                if (fromWallet == null || fromWallet.Balance < amount)
                    throw new InvalidOperationException("موجودی کیف پول مبدا کافی نیست");

                // دریافت یا ایجاد کیف پول مقصد
                var toWallet = await _context.Wallets
                    .FirstOrDefaultAsync(w => w.UserId == toUserId);

                if (toWallet == null)
                {
                    toWallet = new Wallet
                    {
                        UserId = toUserId,
                        Balance = 0,
                        LastUpdated = DateTime.UtcNow
                    };
                    await _context.Wallets.AddAsync(toWallet);
                }

                // انجام انتقال
                fromWallet.Balance -= amount;
                toWallet.Balance += amount;
                fromWallet.LastUpdated = DateTime.UtcNow;
                toWallet.LastUpdated = DateTime.UtcNow;

                _context.Wallets.Update(fromWallet);
                _context.Wallets.Update(toWallet);

                // ثبت تراکنش انتقال
                var transferTransactionId = Guid.NewGuid().ToString();
                var fromTransaction = new WalletTransaction
                {
                    TransactionId = transferTransactionId,
                    UserId = fromUserId,
                    Type = "TransferOut",
                    Amount = -amount,
                    BalanceAfter = fromWallet.Balance,
                    TransactionDate = DateTime.UtcNow,
                    Status = "Completed",
                    Description = $"انتقال به کاربر {toUserId} - {description}",
                    ReferenceTransactionId = transferTransactionId
                };

                var toTransaction = new WalletTransaction
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    UserId = toUserId,
                    Type = "TransferIn",
                    Amount = amount,
                    BalanceAfter = toWallet.Balance,
                    TransactionDate = DateTime.UtcNow,
                    Status = "Completed",
                    Description = $"دریافت از کاربر {fromUserId} - {description}",
                    ReferenceTransactionId = transferTransactionId
                };

                await _context.WalletTransactions.AddAsync(fromTransaction);
                await _context.WalletTransactions.AddAsync(toTransaction);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new WalletTransferResultDto
                {
                    Success = true,
                    FromUserNewBalance = fromWallet.Balance,
                    ToUserNewBalance = toWallet.Balance,
                    TransactionId = transferTransactionId
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, $"خطا در انتقال وجه از {fromUserId} به {toUserId}");
                throw;
            }
        }

        public async Task<bool> ReverseTransactionAsync(string transactionId, string reason)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // یافتن تراکنش اصلی
                var originalTransaction = await _context.WalletTransactions
                    .FirstOrDefaultAsync(t => t.TransactionId == transactionId);

                if (originalTransaction == null || originalTransaction.Amount == 0)
                    return false;

                // ایجاد تراکنش معکوس
                var reverseTransaction = new WalletTransaction
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    UserId = originalTransaction.UserId,
                    Type = "Reversal",
                    Amount = -originalTransaction.Amount,
                    BalanceAfter = originalTransaction.BalanceAfter + originalTransaction.Amount,
                    TransactionDate = DateTime.UtcNow,
                    Status = "Completed",
                    Description = $"برگشت تراکنش {transactionId} - {reason}",
                    ReferenceTransactionId = transactionId
                };

                // بروزرسانی موجودی کیف پول
                var wallet = await _context.Wallets
                    .FirstOrDefaultAsync(w => w.UserId == originalTransaction.UserId);

                if (wallet != null)
                {
                    wallet.Balance += originalTransaction.Amount;
                    wallet.LastUpdated = DateTime.UtcNow;
                    _context.Wallets.Update(wallet);
                }

                await _context.WalletTransactions.AddAsync(reverseTransaction);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, $"خطا در برگشت تراکنش {transactionId}");
                throw;
            }
        }

        public async Task<WalletTransaction> GetTransactionAsync(string transactionId)
        {
            try
            {
                return await _context.WalletTransactions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.TransactionId == transactionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطا در دریافت تراکنش {transactionId}");
                throw;
            }
        }

        public async Task<WalletTransaction> GetTransactionByTrackingCodeAsync(string trackingCode)
        {
            try
            {
                return await _context.WalletTransactions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.ReferenceTransactionId == trackingCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطا در دریافت تراکنش با کد رهگیری {trackingCode}");
                throw;
            }
        }

        public async Task<List<WalletTransaction>> GetTransactionsAsync(string userId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var query = _context.WalletTransactions
                    .AsNoTracking()
                    .Where(t => t.UserId == userId);

                if (fromDate.HasValue)
                    query = query.Where(t => t.TransactionDate >= fromDate.Value);

                if (toDate.HasValue)
                    query = query.Where(t => t.TransactionDate <= toDate.Value);

                return await query
                    .OrderByDescending(t => t.TransactionDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطا در دریافت تراکنش‌های کاربر {userId}");
                throw;
            }
        }

        public async Task<IEnumerable<WalletTransaction>> GetTransactionsByUserIdAsync(string userId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            return await GetTransactionsAsync(userId, fromDate, toDate);
        }

        public async Task AddTransactionAsync(WalletTransaction transaction)
        {
            try
            {
                if (transaction == null)
                    throw new ArgumentNullException(nameof(transaction));

                await _context.WalletTransactions.AddAsync(transaction);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در ثبت تراکنش کیف پول");
                throw;
            }
        }

        public async Task UpdateWalletAsync(Wallet wallet)
        {
            try
            {
                if (wallet == null)
                    throw new ArgumentNullException(nameof(wallet));

                _context.Wallets.Update(wallet);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطا در بروزرسانی کیف پول کاربر {wallet.UserId}");
                throw;
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در ذخیره تغییرات کیف پول");
                throw;
            }
        }

        public async Task<bool> CancelPaymentAsync(string transactionId, string reason)
        {
            return await ReverseTransactionAsync(transactionId, reason);
        }

        public async Task<decimal> DeductAsync(string userId, decimal amount)
        {
            return await DebitAsync(userId, amount, "کسر اعتبار");
        }
    }
}