using System.Threading.Tasks;
using Core.Application.DTOs;

namespace Core.Application.Services
{
    public interface IPaymentVerifier
    {
        /// <summary>
        /// بررسی وضعیت پرداخت بر اساس Tracking Code
        /// </summary>
        Task<WalletTransactionStatus> VerifyPaymentAsync(string trackingCode);

        /// <summary>
        /// لغو یک پرداخت در صورت نیاز
        /// </summary>
        Task<bool> CancelPaymentAsync(string trackingCode);
    }
}
