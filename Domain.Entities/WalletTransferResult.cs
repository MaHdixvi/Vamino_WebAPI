using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WalletTransferResult
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public decimal FromUserNewBalance { get; set; }
        public decimal ToUserNewBalance { get; set; }
        public string ErrorMessage { get; set; }
    }
}
