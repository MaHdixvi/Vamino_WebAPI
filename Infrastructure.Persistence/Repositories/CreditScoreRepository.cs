using Core.Application.Contracts;
using Domain.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    internal class CreditScoreRepository : ICreditScoreRepository
    {
        private readonly AppDbContext _context;

        public CreditScoreRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CreditScore creditScore)
        {
            await _context.CreditScores.AddAsync(creditScore);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var creditScore = await _context.CreditScores.FindAsync(id);
            if (creditScore != null)
            {
                _context.CreditScores.Remove(creditScore);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<CreditScore> GetByIdAsync(string id)
        {
            return await _context.CreditScores
                .Include(cs => cs.User)
                .FirstOrDefaultAsync(cs => cs.Id == id);
        }

        public async Task UpdateAsync(CreditScore creditScore)
        {
            _context.CreditScores.Update(creditScore);
            await _context.SaveChangesAsync();
        }
    }
}
