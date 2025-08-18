using Core.Application.Contracts;
using Domain.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class TransactionLogRepository : ITransactionLogRepository
    {
        private readonly AppDbContext _context;

        public TransactionLogRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(TransactionLog log)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log));

            // تنظیم مقادیر پیش‌فرض
            log.Id = Guid.NewGuid().ToString();
            log.Timestamp = DateTime.UtcNow;

            await _context.TransactionLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TransactionLog>> GetByEntityAsync(string entityType, string entityId)
        {
            if (string.IsNullOrWhiteSpace(entityType))
                throw new ArgumentException("Entity type cannot be null or empty", nameof(entityType));

            var query = _context.TransactionLogs
                .AsNoTracking()
                .Where(l => l.RelatedEntity == entityType);

            if (!string.IsNullOrWhiteSpace(entityId))
            {
                query = query.Where(l => l.EntityId == entityId);
            }

            return await query
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<TransactionLog>> GetByUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

            return await _context.TransactionLogs
                .AsNoTracking()
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<TransactionLog>> GetByActionAsync(string action)
        {
            if (string.IsNullOrWhiteSpace(action))
                throw new ArgumentException("Action cannot be null or empty", nameof(action));

            return await _context.TransactionLogs
                .AsNoTracking()
                .Where(l => l.Action == action)
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }

        public async Task<TransactionLog> GetByIdAsync(string logId)
        {
            if (string.IsNullOrWhiteSpace(logId))
                throw new ArgumentException("Log ID cannot be null or empty", nameof(logId));

            return await _context.TransactionLogs
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == logId);
        }

        public async Task<IEnumerable<TransactionLog>> GetAllAsync()
        {
            return await _context.TransactionLogs
                .AsNoTracking()
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }
        public async Task<TransactionLog> GetByTrackingCodeAsync(string trackingCode)
        {
            if (string.IsNullOrWhiteSpace(trackingCode))
                throw new ArgumentException("Tracking code cannot be null or empty", nameof(trackingCode));

            return await _context.TransactionLogs
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.TrackingCode == trackingCode);
        }

        public async Task UpdateAsync(TransactionLog log)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log));

            var existingLog = await _context.TransactionLogs
                .FirstOrDefaultAsync(l => l.Id == log.Id);

            if (existingLog == null)
                throw new InvalidOperationException($"TransactionLog with ID '{log.Id}' not found.");

            // بروزرسانی فیلدها
            existingLog.Action = log.Action;
            existingLog.UserId = log.UserId;
            existingLog.EntityId = log.EntityId;
            existingLog.RelatedEntity = log.RelatedEntity;
            existingLog.TrackingCode = log.TrackingCode;
            existingLog.Timestamp = log.Timestamp;

            _context.TransactionLogs.Update(existingLog);
            await _context.SaveChangesAsync();
        }

        public IQueryable<TransactionLog> GetByEntityQueryable(string entityType, string entityId)
        {
            if (string.IsNullOrWhiteSpace(entityType))
                throw new ArgumentException("Entity type cannot be null or empty", nameof(entityType));

            var query = _context.TransactionLogs.AsNoTracking()
                        .Where(l => l.RelatedEntity == entityType);

            if (!string.IsNullOrWhiteSpace(entityId))
            {
                query = query.Where(l => l.EntityId == entityId);
            }

            return query;
        }

        public IQueryable<TransactionLog> GetByUserIdQueryable(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

            return _context.TransactionLogs
                .AsNoTracking()
                .Where(l => l.UserId == userId);
        }

        public IQueryable<TransactionLog> GetByActionQueryable(string action)
        {
            if (string.IsNullOrWhiteSpace(action))
                throw new ArgumentException("Action cannot be null or empty", nameof(action));

            return _context.TransactionLogs
                           .AsNoTracking()
                           .Where(l => l.Action == action)
                           .OrderByDescending(l => l.Timestamp);
        }

        public IQueryable<TransactionLog> GetAllQueryable()
        {
            return _context.TransactionLogs
                           .AsNoTracking()
                           .OrderByDescending(l => l.Timestamp);
        }

    }
}