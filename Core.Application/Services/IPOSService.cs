using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services
{

    public interface IPOSService
    {
        Task<bool> ConnectAsync();
        Task<PosResult> SaleAsync(decimal amount, string currency, string trackingCode);
        Task<PosResult> CancelAsync(string rrnOrStan, string trackingCode);
        Task<bool> IsConnectedAsync();
        Task<PosResult> ProcessPayment(POSPaymentRequest request);

    }
}
