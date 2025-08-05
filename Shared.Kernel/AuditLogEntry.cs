using System;

namespace Shared.Kernel
{
    /// <summary>
    /// لاگ فعالیت‌های سیستم برای ردیابی عملیات مهم
    /// </summary>
    public class AuditLogEntry
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Action { get; set; } // مثلاً Create, Update, Delete
        public string EntityName { get; set; } // نام موجودیت (مثلاً Loan, User)
        public string EntityId { get; set; } // شناسه موجودیت
        public string OldValues { get; set; } // مقادیر قبلی (به صورت JSON)
        public string NewValues { get; set; } // مقادیر جدید (به صورت JSON)
        public DateTime Timestamp { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }
}