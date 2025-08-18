using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enums
{
    public enum TransactionType
    {
        Payment,
        Deposit,
        TransferIn,
        TransferOut
    }

    public enum TransactionStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded,
        Cancelled
    }
}
