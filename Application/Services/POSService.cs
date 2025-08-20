using Core.Application.Services;
using System.Threading.Tasks;

public class POSService : IPOSService
{
    public async Task<POSPaymentResult> ProcessPaymentAsync(POSPaymentRequest request)
    {
        // „‰ÿﬁ «‰Ã«„  —«ò‰‘ POS
        return new POSPaymentResult { Success = true };
    }

    public async Task<POSRefundResult> RefundAsync(POSRefundRequest request)
    {
        // „‰ÿﬁ »«“ê‘  ÊÃÂ POS
        return new POSRefundResult { Success = true };
    }
}