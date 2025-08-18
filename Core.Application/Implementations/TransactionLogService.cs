using Core.Application.Contracts;
using Core.Application.DTOs;
using Core.Application.Services;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Application.Implementations
{
    public class TransactionLogService : ITransactionLogService
    {
        private readonly ITransactionLogRepository _repository;
        private readonly ILogger<TransactionLogService> _logger;

        public TransactionLogService(
            ITransactionLogRepository repository,
            ILogger<TransactionLogService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<TransactionLog> LogAsync(TransactionLogCreateDto logDto)
        {
            try
            {
                var log = new TransactionLog
                {
                    Action = logDto.Action,
                    RelatedEntity = logDto.RelatedEntity,
                    EntityId = logDto.EntityId,
                    Details = logDto.Details,
                    UserId = logDto.UserId,
                    TrackingCode = logDto.TrackingCode,
                    Amount = logDto.Amount,
                    PaymentMethod = logDto.PaymentMethod,
                    Timestamp = DateTime.UtcNow
                };

                await _repository.AddAsync(log);
                return log;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در ثبت لاگ تراکنش");
                throw;
            }
        }

        public async Task<IEnumerable<TransactionLog>> GetLogsByEntityAsync(string entityType, string entityId)
        {
            return await _repository.GetByEntityAsync(entityType, entityId);
        }

        public async Task<IEnumerable<TransactionLog>> GetLogsByUserAsync(string userId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _repository.GetByUserIdQueryable(userId);
            query = FilterByDateRange(query, fromDate, toDate);
            return await Task.FromResult(query.OrderByDescending(l => l.Timestamp).ToList());
        }

        public async Task<IEnumerable<TransactionLog>> GetLogsByActionAsync(string action, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _repository.GetByActionQueryable(action);
            query = FilterByDateRange(query, fromDate, toDate);
            return await Task.FromResult(query.OrderByDescending(l => l.Timestamp).ToList());
        }

        public async Task<TransactionLog> GetLogByIdAsync(string logId)
        {
            return await _repository.GetByIdAsync(logId);
        }

        public async Task<PaginatedList<TransactionLog>> SearchLogsAsync(TransactionLogSearchDto searchDto)
        {
            var query = _repository.GetAllQueryable();

            if (!string.IsNullOrEmpty(searchDto.Action))
                query = query.Where(l => l.Action == searchDto.Action);

            if (!string.IsNullOrEmpty(searchDto.RelatedEntity))
                query = query.Where(l => l.RelatedEntity == searchDto.RelatedEntity);

            if (!string.IsNullOrEmpty(searchDto.EntityId))
                query = query.Where(l => l.EntityId == searchDto.EntityId);

            if (!string.IsNullOrEmpty(searchDto.UserId))
                query = query.Where(l => l.UserId == searchDto.UserId);

            query = FilterByDateRange(query, searchDto.FromDate, searchDto.ToDate);

            var totalCount = query.Count();
            var items = query
                .OrderByDescending(l => l.Timestamp)
                .Skip((searchDto.PageNumber - 1) * searchDto.PageSize)
                .Take(searchDto.PageSize)
                .ToList();

            return new PaginatedList<TransactionLog>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = searchDto.PageNumber,
                PageSize = searchDto.PageSize
            };
        }

        public async Task<TransactionLog> GetLatestStatusAsync(string entityType, string entityId)
        {
            var query = _repository.GetByEntityQueryable(entityType, entityId);
            return await Task.FromResult(query.OrderByDescending(l => l.Timestamp).FirstOrDefault());
        }

        private IQueryable<TransactionLog> FilterByDateRange(IQueryable<TransactionLog> query, DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate.HasValue)
                query = query.Where(l => l.Timestamp >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(l => l.Timestamp <= toDate.Value);

            return query;
        }

        public async Task<bool> VerifyOwnershipAsync(string logId, string userId)
        {
            var log = await _repository.GetByIdAsync(logId);
            return log != null && log.UserId == userId;
        }

        public async Task UpdatePaymentStatusAsync(string transactionId, string newStatus, string description = null)
        {
            var log = await _repository.GetByTrackingCodeAsync(transactionId);
            if (log != null)
            {
                log.Details = description ?? log.Details;
                log.Action = newStatus;
                await _repository.UpdateAsync(log);
            }
        }

        public async Task LogSuspiciousActivityAsync(string userId, string transactionId, string reason)
        {
            var logDto = new TransactionLogCreateDto
            {
                UserId = userId,
                Action = "SuspiciousActivity",
                RelatedEntity = "Transaction",
                EntityId = transactionId,
                Details = reason,
                TrackingCode = transactionId
            };

            await LogAsync(logDto);
        }

        public async Task LogPaymentResultAsync(string transactionId, bool success, string description = null)
        {
            var logDto = new TransactionLogCreateDto
            {
                Action = success ? "PaymentSuccess" : "PaymentFailed",
                RelatedEntity = "Transaction",
                EntityId = transactionId,
                Details = description,
                TrackingCode = transactionId
            };

            await LogAsync(logDto);
        }

        public async Task<TransactionLog> GetByTrackingCodeAsync(string trackingCode)
        {
            return await _repository.GetByTrackingCodeAsync(trackingCode);
        }

        public async Task<IEnumerable<TransactionLog>> GetPaymentHistoryAsync(string userId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _repository.GetByUserIdQueryable(userId)
                                   .Where(l => l.RelatedEntity == "Transaction" &&
                                               (l.Action == "PaymentSuccess" || l.Action == "PaymentFailed"));
            query = FilterByDateRange(query, fromDate, toDate);
            return await Task.FromResult(query.OrderByDescending(l => l.Timestamp).ToList());
        }

        public async Task LogWalletTransactionAsync(string userId, string transactionId, string action, decimal amount, decimal newBalance, string description)
        {
            var logDto = new TransactionLogCreateDto
            {
                UserId = userId,
                Action = action,
                RelatedEntity = "WalletTransaction",
                EntityId = transactionId,
                Amount = amount,
                Details = $"{description}. New Balance: {newBalance}",
                TrackingCode = transactionId
            };

            await LogAsync(logDto);
        }
    }
}
