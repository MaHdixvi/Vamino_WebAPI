using Core.Application.DTOs;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public interface IPaymentValidator
    {
        Task<PaymentValidationResult> ValidateAsync(PaymentRequestDto paymentRequest);
        Task<bool> ValidateAmountAsync(decimal amount, string currency);
        Task<bool> ValidatePaymentMethodAsync(string paymentMethod, string userId);
        Task<bool> ValidateRecipientAsync(string recipientAccount);
    }
}