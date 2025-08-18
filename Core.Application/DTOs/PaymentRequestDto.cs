using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.DTOs
{
    public enum PaymentMethod { POS, Gateway, Wallet }
    public class PaymentValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public Dictionary<string, string> ValidationErrors { get; set; } = new();

        public static PaymentValidationResult Valid() => new() { IsValid = true };
        public static PaymentValidationResult Invalid(string message) => new()
        {
            IsValid = false,
            ErrorMessage = message
        };
    }
    public class PaymentRequestDto
    {
        public string PaymentId { get; set; } // شناسه سفارش/وام
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public string Currency { get; set; }   // مثل "IRR" یا "USD"
        public string RecipientAccount { get; set; }

        public string BankAccountNumber { get; set; }
        public string CallbackUrl { get; set; }
        public string Description { get; set; }
        public string UserIP { get; set; }
        public string DeviceId { get; set; }
        public string OrderId { get; set; }
        public string TrackingCode { get; set; }
    }

    public class PaymentResult
    {
        public bool IsSuccess { get; set; }
        public string TrackingCode { get; set; }
        public string TransactionId { get; set; }
        public string RedirectUrl { get; set; }
        public string Message { get; set; }

        public static PaymentResult Success(string trackingCode, string transactionId)
            => new() { IsSuccess = true, TrackingCode = trackingCode, TransactionId = transactionId };

        public static PaymentResult Failed(string message)
            => new() { IsSuccess = false, Message = message };

        public static PaymentResult Redirect(string url, string trackingCode)
            => new() { RedirectUrl = url, TrackingCode = trackingCode, IsSuccess = true };
    }

    public class PaymentStatusResult
    {
        public string TrackingCode { get; set; }
        public bool IsVerified { get; set; }
        public string Status { get; set; } // Pending, Completed, Failed, Refunded
        public DateTime VerificationDate { get; set; }
        public decimal VerifiedAmount { get; set; }
        public string Message { get; set; }

        public static PaymentStatusResult Failed(string message)
            => new() { IsVerified = false, Message = message };
    }

    public class BankAccountValidationResult
    {
        public bool IsValid { get; set; }
        public string BankName { get; set; }
        public string CardType { get; set; } // Debit/Credit
        public string ValidationMessage { get; set; }
    }

    public class PaymentAmountBreakdown
    {
        public decimal OriginalAmount { get; set; }
        public decimal Commission { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class PaymentGatewayRequest
    {
        public string PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string CallbackUrl { get; set; }
        public string Description { get; set; }
    }

    public class PaymentGatewayResponse
    {
        public bool Success { get; set; }
        public string GatewayUrl { get; set; }
        public string PaymentToken { get; set; }
        public string Error { get; set; }
    }

    public class PaymentCallbackDto
    {
        public string PaymentId { get; set; }
        public string TrackingCode { get; set; }
        public string TransactionId { get; set; }
        public bool IsSuccess { get; set; }
        public string BankResponse { get; set; }
        public string DigitalSignature { get; set; }
    }

    public class TransactionLogCreateDto
    {
        public string Action { get; set; }
        public string RelatedEntity { get; set; }
        public string EntityId { get; set; }
        public string Details { get; set; }
        public string UserId { get; set; }
        public string TrackingCode { get; set; }
        public decimal? Amount { get; set; }
        public string PaymentMethod { get; set; }
    }

    public class TransactionLogSearchDto
    {
        public string Action { get; set; }
        public string RelatedEntity { get; set; }
        public string EntityId { get; set; }
        public string UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class PaginatedList<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
