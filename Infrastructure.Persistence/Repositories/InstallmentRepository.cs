using Core.Application.Contracts;
using Domain.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class InstallmentRepository : IInstallmentRepository
    {
        private readonly AppDbContext _context;

        public InstallmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Installment installment)
        {
            await _context.Installments.AddAsync(installment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var installment = await _context.Installments.FindAsync(id);
            if (installment != null)
            {
                _context.Installments.Remove(installment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Installment>> GetAllAsync()
        {
            return await _context.Installments
                .Include(i => i.Loan)
                .ToListAsync();
        }

        public async Task<Installment> GetByIdAsync(string id)
        {
            return await _context.Installments
                .Include(i => i.Loan)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Installment>> GetByLoanIdAsync(string loanId)
        {
            return await _context.Installments
                .Where(i => i.LoanId == loanId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Installment>> GetByUserIdAsync(string userId)
        {
            return await _context.Installments
                .Include(i => i.Loan)
                .Where(i => i.Loan.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Installment>> GetOverdueInstallmentsAsync()
        {
            return await _context.Installments
                .Where(i => i.Status == "Overdue")
                .ToListAsync();
        }

        public async Task<IEnumerable<Installment>> GetPendingInstallmentsAsync()
        {
            return await _context.Installments
                .Where(i => i.Status == "Pending")
                .ToListAsync();
        }

        public async Task UpdateAsync(Installment installment)
        {
            _context.Installments.Update(installment);
            await _context.SaveChangesAsync();
        }
    }
}
