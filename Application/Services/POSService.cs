using Core.Application.Services;
using System.Threading.Tasks;

public class POSService : IPOSService
{
    public async Task<POSPaymentResult> ProcessPaymentAsync(POSPaymentRequest request)
    {
        // ���� ����� ��ǘ�� POS
        return new POSPaymentResult { Success = true };
    }

    public async Task<POSRefundResult> RefundAsync(POSRefundRequest request)
    {
        // ���� ��Ґ�� ��� POS
        return new POSRefundResult { Success = true };
    }
}