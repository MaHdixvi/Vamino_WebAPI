using Core.Application.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Pos
{
    public class PosConfig
    {
        public string Vendor { get; set; } = "PAX";
        public string CommType { get; set; } = "Serial"; // Serial | USB | TCP
        public string Port { get; set; }                 // COM3, ...
        public string Ip { get; set; }                   // اگر TCP
        public int PortNumber { get; set; } = 10009;     // اگر TCP
        public int TimeoutMs { get; set; } = 60000;
        public string Currency { get; set; } = "IRR";
    }

    public sealed class PaxPosTerminal : IPosTerminal
    {
        private readonly PosConfig _cfg;
        private readonly ILogger<PaxPosTerminal> _logger;
        private bool _connected;

        // (نمونه) شئ ارتباطی SDK
        // private Pax.POSLink.PosLink _link;   // مثال اگر از POSLink استفاده کنی

        public PaxPosTerminal(IOptions<PosConfig> cfg, ILogger<PaxPosTerminal> logger)
        {
            _cfg = cfg.Value;
            _logger = logger;
        }

        public async Task ConnectAsync()
        {
            try
            {
                _logger.LogInformation("Connecting to PAX via {Type}...", _cfg.CommType);

                // TODO: _link = new Pax.POSLink.PosLink(...); _link.SetCommSetting(...); _link.Open();

                _connected = true;
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _connected = false;
                _logger.LogError(ex, "POS connection failed");
                throw;
            }
        }

        public Task<bool> IsConnectedAsync() => Task.FromResult(_connected);

        public async Task<PosResult> SaleAsync(PosSaleRequest request)
        {
            if (!_connected) await ConnectAsync();

            try
            {
                // TODO: Call SDK sale/purchase method

                return new PosResult(
                    Success: true,
                    HostResponseCode: "00",
                    Message: "APPROVED",
                    RRN: "123456789012",
                    Stan: "000123",
                    CardMasked: "6037*********1234",
                    TerminalId: "T1234567",
                    MerchantId: "M1234567",
                    ExtraPayload: "{}"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POS Sale failed");
                return new PosResult(false, "XX", ex.Message, null, null, null, null, null, null);
            }
        }

        public async Task<PosResult> CancelAsync(string rrnOrStan, string trackingCode)
        {
            if (!_connected) await ConnectAsync();
            try
            {
                // TODO: Call SDK void/refund by RRN or STAN
                return new PosResult(true, "00", "VOIDED", rrnOrStan, null, null, null, null, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POS Cancel failed");
                return new PosResult(false, "XX", ex.Message, null, null, null, null, null, null);
            }
        }
    }
}
