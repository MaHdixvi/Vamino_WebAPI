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
    public class RecipientRepository : IRecipientRepository
    {
        private readonly AppDbContext _context;

        public RecipientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Recipient recipient)
        {
            await _context.Recipients.AddAsync(recipient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Recipient recipient)
        {
            _context.Recipients.Update(recipient);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string recipientId)
        {
            var recipient = await _context.Recipients.FirstOrDefaultAsync(r => r.Id == recipientId);
            if (recipient != null)
            {
                _context.Recipients.Remove(recipient);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Recipient> GetByIdAsync(string recipientId)
        {
            return await _context.Recipients.FirstOrDefaultAsync(r => r.Id == recipientId);
        }

        public async Task<IEnumerable<Recipient>> GetAllAsync()
        {
            return await _context.Recipients.ToListAsync();
        }
    }
}
