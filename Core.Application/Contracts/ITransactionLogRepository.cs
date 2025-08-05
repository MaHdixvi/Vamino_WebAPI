using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Contracts
{
    /// <summary>
    /// قرارداد برای دسترسی به داده‌های لاگ تراکنش
    /// </summary>
    public interface ITransactionLogRepository
    {
        Task<TransactionLog> GetByIdAsync(string id);
        Task<IEnumerable<TransactionLog>> GetByUserIdAsync(string userId);
        Task<IEnumerable<TransactionLog>> GetByActionAsync(string action);
        Task<IEnumerable<TransactionLog>> GetByRelatedEntityAsync(string relatedEntity);
        Task AddAsync(TransactionLog log);
        Task UpdateAsync(TransactionLog log);
        Task DeleteAsync(string id);
    }
}
