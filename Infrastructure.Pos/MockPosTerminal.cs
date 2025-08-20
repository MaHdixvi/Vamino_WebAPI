using Core.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Pos
{
    public class MockPosTerminal : IPosTerminal
    {
        private bool _connected = false;

        public Task ConnectAsync()
        {
            _connected = true;
            return Task.CompletedTask;
        }

        public Task<bool> IsConnectedAsync()
        {
            return Task.FromResult(_connected);
        }

        public Task<PosResult> SaleAsync(PosSaleRequest request)
        {
            if (!_connected)
                throw new InvalidOperationException("POS is not connected.");

            return Task.FromResult(new PosResult(
                Success: true,
                HostResponseCode: "000000",
                Message: "پرداخت تستی موفق بود",
                RRN: Guid.NewGuid().ToString("N").Substring(0, 12),
                Stan: new Random().Next(100000, 999999).ToString(),
                CardMasked: "6037********1234",
                TerminalId: "12345678",
                MerchantId: "87654321",
                ExtraPayload: "{}"
            ));
        }

        public Task<PosResult> CancelAsync(string rrnOrStan, string trackingCode)
        {
            if (!_connected)
                throw new InvalidOperationException("POS is not connected.");

            return Task.FromResult(new PosResult(
                Success: true,
                HostResponseCode: "000000",
                Message: $"تراکنش {rrnOrStan} لغو شد",
                RRN: rrnOrStan,
                Stan: trackingCode,
                CardMasked: "",
                TerminalId: "12345678",
                MerchantId: "87654321",
                ExtraPayload: "{}"
            ));
        }
    }


}
