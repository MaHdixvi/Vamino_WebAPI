using Core.Application.Contracts;
using Domain.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class LoanApplicationRepository : ILoanApplicationRepository
    {
        private readonly AppDbContext _context;

        public LoanApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LoanApplication application)
        {
            await _context.LoanApplications.AddAsync(application);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var application = await _context.LoanApplications.FindAsync(id);
            if (application != null)
            {
                _context.LoanApplications.Remove(application);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<LoanApplication>> GetAllAsync()
        {
            return await _context.LoanApplications
                .Include(la => la.User)  // اگر لازم بود User رو لود کنیم
                .ToListAsync();
        }

        public async Task<LoanApplication> GetByIdAsync(string id)
        {
            return await _context.LoanApplications
                .Include(la => la.User)
                .Include(la => la.Loan)
                .Include(la => la.Installments)
                .FirstOrDefaultAsync(la => la.Id == id);
        }

        public async Task<IEnumerable<LoanApplication>> GetByStatusAsync(string status)
        {
            return await _context.LoanApplications
                .Where(la => la.Status == status)
                .Include(la => la.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<LoanApplication>> GetByUserIdAsync(string userId)
        {
            return await _context.LoanApplications
                .Where(la => la.UserId == userId)
                .Include(la => la.Loan)
                .ToListAsync();
        }

        public async Task UpdateAsync(LoanApplication application)
        {
            _context.LoanApplications.Update(application);
            await _context.SaveChangesAsync();
        }
    }
}
