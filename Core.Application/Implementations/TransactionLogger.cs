using Core.Application.Contracts;
using Core.Application.DTOs;
using Core.Application.Services;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Core.Application.Implementations
{
    public class TransactionLogger : ITransactionLogger
    {
        private readonly ITransactionLogRepository _logRepository;

        public TransactionLogger(ITransactionLogRepository logRepository)
        {
            _logRepository = logRepository ?? throw new ArgumentNullException(nameof(logRepository));
        }

        public async Task LogPaymentResultAsync(PaymentRequestDto paymentRequest, object result)
        {
            if (paymentRequest == null)
                throw new ArgumentNullException(nameof(paymentRequest));

            var log = new TransactionLog
            {
                Id = Guid.NewGuid().ToString(),
                UserId = paymentRequest.UserId,
                EntityId = paymentRequest.OrderId,
                RelatedEntity = "Payment",
                Action = "PaymentAttempt",
                TrackingCode = paymentRequest.TrackingCode,
                Timestamp = DateTime.UtcNow,
                Details = result?.ToString() ?? "No result returned"
            };

            await _logRepository.AddAsync(log);
        }
    }
}
