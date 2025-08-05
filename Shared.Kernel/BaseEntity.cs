using System;

namespace Shared.Kernel
{
    /// <summary>
    /// کلاس پایه برای تمام موجودیت‌های دامنه
    /// </summary>
    public abstract class BaseEntity
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}