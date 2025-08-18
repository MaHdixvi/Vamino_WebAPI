using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Contracts
{
    public interface IWalletTransactionRepository
    {
        Task<WalletTransaction> GetByTrackingCodeAsync(string trackingCode);
        Task UpdateAsync(WalletTransaction transaction);
        Task AddAsync(WalletTransaction transaction);
    }
}
