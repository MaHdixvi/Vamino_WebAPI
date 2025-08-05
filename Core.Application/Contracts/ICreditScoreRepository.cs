using System;
using System.Threading.Tasks;
using Domain.Entities;

namespace Core.Application.Contracts
{
    public interface ICreditScoreRepository
    {
        Task<CreditScore> GetByIdAsync(string id);
        Task AddAsync(CreditScore creditScore);
        Task UpdateAsync(CreditScore creditScore);
        Task DeleteAsync(string id);
    }
}