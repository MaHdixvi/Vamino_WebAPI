using Core.Application.DTOs;
using Domain.Entities;
using System.Threading.Tasks;

namespace Core.Application.Contracts
{
    public interface IPaymentRepository
    {
        Task<WalletTransaction> GetTransactionByTrackingCodeAsync(string trackingCode);
        Task<CurrencyInfo> GetCurrencyInfoAsync(string currencyCode);
        Task UpdateTransactionAsync(WalletTransaction transaction);
        Task<bool> IsPaymentMethodValidAsync(string userId,string paymentMethod);
        Task SaveChangesAsync();
    }
}
