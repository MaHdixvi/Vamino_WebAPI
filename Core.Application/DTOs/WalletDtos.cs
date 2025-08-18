namespace Core.Application.DTOs
{
    public class WalletPaymentRequest
    {
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string TrackingCode { get; set; }
        public string Description { get; set; }
        public string IPAddress { get; set; }
        public string DeviceInfo { get; set; }
    }

    public class WalletPaymentResult
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public decimal NewBalance { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class WalletDepositRequest
    {
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string DepositMethod { get; set; } // BankTransfer, Gateway, etc.
        public string ReferenceNumber { get; set; }
    }

    public class WalletDepositResult
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public decimal NewBalance { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class WalletTransactionStatus
    {
        public string Status { get; set; } // Pending, Completed, Failed, Refunded
        public DateTime StatusDate { get; set; }
        public decimal Amount { get; set; }
    }

    public class WalletTransactionHistory
    {
        public List<WalletTransactionItem> Transactions { get; set; }
        public int TotalCount { get; set; }
    }

    public class WalletTransactionItem
    {
        public string TransactionId { get; set; }
        public string Type { get; set; } // Payment, Deposit, Transfer
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }

    public class WalletTransferRequest
    {
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }

    public class WalletTransferResultDto
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public decimal FromUserNewBalance { get; set; }
        public decimal ToUserNewBalance { get; set; }
        public string ErrorMessage { get; set; }
    }
}