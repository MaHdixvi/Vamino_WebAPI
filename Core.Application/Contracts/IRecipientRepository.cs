using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Contracts
{
    public interface IRecipientRepository
    {
        Task AddAsync(Recipient recipient);
        Task UpdateAsync(Recipient recipient);
        Task DeleteAsync(string recipientId);
        Task<Recipient> GetByIdAsync(string recipientId);
        Task<IEnumerable<Recipient>> GetAllAsync();
    }
}
