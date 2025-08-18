using Core.Application.Contracts;
using Domain.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class WalletTransactionRepository : IWalletTransactionRepository
    {
        private readonly AppDbContext _context;

        public WalletTransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<WalletTransaction> GetByTrackingCodeAsync(string trackingCode)
        {
            return await _context.WalletTransactions
                .Include(t => t.Wallet)
                .FirstOrDefaultAsync(t => t.TrackingCode == trackingCode);
        }

        public async Task UpdateAsync(WalletTransaction transaction)
        {
            _context.WalletTransactions.Update(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(WalletTransaction transaction)
        {
            await _context.WalletTransactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }
    }

}
