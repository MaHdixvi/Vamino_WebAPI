public class LoanApplicationDTO
{
    public string Id { get; set; }                // شناسه وام
    public string UserId { get; set; }            // شناسه کاربر
    public decimal RequestedAmount { get; set; }  // مبلغ وام
    public string Status { get; set; }            // وضعیت: pending, approved, rejected
    public string Purpose { get; set; }           // هدف وام
    public DateTime SubmitDate { get; set; }      // تاریخ ثبت درخواست
    public int NumberOfInstallments { get; set; } // تعداد اقساط
}
