using Core.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Implementations
{
    public class POSService : IPOSService
    {
        private readonly IPosTerminal _terminal;

        public POSService(IPosTerminal terminal)
        {
            _terminal = terminal;
        }

        public async Task<bool> ConnectAsync() => await _terminal.ConnectAsync().ContinueWith(_ => true);

        public async Task<PosResult> SaleAsync(decimal amount, string currency, string trackingCode)
        {
            return await _terminal.SaleAsync(new PosSaleRequest(amount, currency, trackingCode));
        }

        public async Task<PosResult> CancelAsync(string rrnOrStan, string trackingCode)
        {
            return await _terminal.CancelAsync(rrnOrStan, trackingCode);
        }

        public Task<bool> IsConnectedAsync() => _terminal.IsConnectedAsync();

        // پیاده‌سازی متد جدید
        public async Task<PosResult> ProcessPayment(POSPaymentRequest request)
        {
            return await SaleAsync(request.Amount, request.Currency, request.TrackingCode);
        }
    }

}
