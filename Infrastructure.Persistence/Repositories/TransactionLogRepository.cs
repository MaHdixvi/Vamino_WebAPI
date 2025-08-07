using Core.Application.Contracts;
using Domain.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class TransactionLogRepository : ITransactionLogRepository
    {
        private readonly AppDbContext _context;

        public TransactionLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TransactionLog log)
        {
            await _context.TransactionLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var log = await _context.TransactionLogs.FindAsync(id);
            if (log != null)
            {
                _context.TransactionLogs.Remove(log);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TransactionLog>> GetByActionAsync(string action)
        {
            return await _context.TransactionLogs
                .Where(l => l.Action == action)
                .ToListAsync();
        }

        public async Task<TransactionLog> GetByIdAsync(string id)
        {
            return await _context.TransactionLogs.FindAsync(id);
        }

        public async Task<IEnumerable<TransactionLog>> GetByRelatedEntityAsync(string relatedEntity)
        {
            return await _context.TransactionLogs
                .Where(l => l.RelatedEntity == relatedEntity)
                .ToListAsync();
        }

        public async Task<IEnumerable<TransactionLog>> GetByUserIdAsync(string userId)
        {
            return await _context.TransactionLogs
                .Where(l => l.UserId == userId)
                .ToListAsync();
        }

        public async Task UpdateAsync(TransactionLog log)
        {
            _context.TransactionLogs.Update(log);
            await _context.SaveChangesAsync();
        }
    }
}
