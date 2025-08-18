using Core.Application.Contracts;
using Domain.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<WalletTransaction> GetTransactionByTrackingCodeAsync(string trackingCode)
    {
        return await _context.WalletTransactions
            .Include(t => t.Wallet)
            .FirstOrDefaultAsync(t => t.TrackingCode == trackingCode);
    }

    public async Task UpdateTransactionAsync(WalletTransaction transaction)
    {
        _context.WalletTransactions.Update(transaction);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<CurrencyInfo> GetCurrencyInfoAsync(string currencyCode)
    {
        return await _context.Currencies
            .FirstOrDefaultAsync(c => c.CurrencyCode == currencyCode);
    }

    public async Task<bool> IsPaymentMethodValidAsync(string userId, string paymentMethod)
    {
        // بررسی اینکه آیا کاربر با userId تراکنشی با paymentMethod داشته است
        var isValid = await _context.WalletTransactions
            .AnyAsync(t => t.UserId == userId && t.Type == "Payment" && t.Status == "Completed" && t.Description.Contains(paymentMethod));
        return isValid;
    }
}