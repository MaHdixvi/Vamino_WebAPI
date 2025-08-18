using Shared.Kernel;
using System;

namespace Domain.Entities
{
    public class TransactionLog : BaseEntity
    {
        public DateTime Timestamp { get; set; }
        public string Action { get; set; }
        public string RelatedEntity { get; set; }
        public string EntityId { get; set; }
        public string Details { get; set; }
        public string? UserId { get; set; }
        public string TrackingCode { get; set; }
        public decimal? Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string IPAddress { get; set; }
        public virtual User User { get; set; }
    }
}