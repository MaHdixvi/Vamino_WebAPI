using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Application.Contracts
{
    /// <summary>
    /// قرارداد برای دسترسی به داده‌های لاگ تراکنش
    /// </summary>
    public interface ITransactionLogRepository
    {
        IQueryable<TransactionLog> GetByActionQueryable(string action);
        IQueryable<TransactionLog> GetAllQueryable();
        IQueryable<TransactionLog> GetByUserIdQueryable(string userId);
        IQueryable<TransactionLog> GetByEntityQueryable(string entityType, string entityId);

        Task<TransactionLog> GetByTrackingCodeAsync(string trackingCode);
        Task UpdateAsync(TransactionLog log);
        Task AddAsync(TransactionLog log);
        Task<IEnumerable<TransactionLog>> GetByEntityAsync(string entityType, string entityId);
        Task<IEnumerable<TransactionLog>> GetByUserIdAsync(string userId);
        Task<IEnumerable<TransactionLog>> GetByActionAsync(string action);
        Task<TransactionLog> GetByIdAsync(string logId);
        Task<IEnumerable<TransactionLog>> GetAllAsync();
    }
}
