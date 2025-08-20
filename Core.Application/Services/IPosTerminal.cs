using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public record PosSaleRequest(decimal Amount, string Currency, string TrackingCode);
    public record PosResult(bool Success, string HostResponseCode, string Message, string RRN, string Stan, string CardMasked, string TerminalId, string MerchantId, string ExtraPayload);

    public interface IPosTerminal
    {
        Task ConnectAsync();
        Task<PosResult> SaleAsync(PosSaleRequest request);
        Task<PosResult> CancelAsync(string rrnOrStan, string trackingCode);
        Task<bool> IsConnectedAsync();
    }
}
