using Shared.Kernel;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public string UserId { get; set; }              // کلید اصلی (می‌تونه همون شناسه کاربر باشه)
        public decimal Balance { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // 🔗 Navigation Property (یک کیف پول → چند تراکنش)
        public ICollection<WalletTransaction> Transactions { get; set; } = new List<WalletTransaction>();
    }
}
