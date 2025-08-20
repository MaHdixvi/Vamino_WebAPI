public class POSPaymentRequest
{
    public string CardNumber { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "IRR";
    public string TrackingCode { get; set; }
}

public class POSPaymentResult
{
    public bool Success { get; set; }
    public string Message { get; set; }

}