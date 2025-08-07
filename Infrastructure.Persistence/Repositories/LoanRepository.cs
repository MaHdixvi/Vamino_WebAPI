using Core.Application.Contracts;
using Domain.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly AppDbContext _context;

        public LoanRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Loan loan)
        {
            await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan != null)
            {
                _context.Loans.Remove(loan);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Loan> GetByIdAsync(string id)
        {
            return await _context.Loans
                .Include(l => l.LoanApplication)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Loan>> GetByLoanApplicationIdAsync(string loanApplicationId)
        {
            return await _context.Loans
                .Where(l => l.LoanApplicationId == loanApplicationId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetByUserIdAsync(string userId)
        {
            return await _context.Loans
                .Include(l => l.LoanApplication)
                .Where(l => l.LoanApplication.UserId == userId)
                .ToListAsync();
        }

        public async Task UpdateAsync(Loan loan)
        {
            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();
        }
    }
}
